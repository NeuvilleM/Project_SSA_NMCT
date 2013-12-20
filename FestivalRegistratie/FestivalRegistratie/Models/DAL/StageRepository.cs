using DBHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models.DAL
{
    public class StageRepository
    {

        public List<Stage> stagesDropDown { get; set; }
        public List<Stage> stages { get; set; }
        public List<String> days { get; set; }
        public StageRepository()
        {
            stages = GetStages();
            stagesDropDown = GetStages();
            days = FestivalRepository.datesFestival();
        }

        public StageRepository(string stage, string selectedDay)
        {
            // vaste members invullen:
            stagesDropDown = GetStages();
            days = FestivalRepository.datesFestival();
            // variabele members zoeken en invullen
            List<Stage> selectedStages = new List<Stage>();
            // TODO: Complete membe
            selectedStages = SearchSelectedLineUps(stage, selectedDay);
            stages = selectedStages;
        }

        private List<Stage> SearchSelectedLineUps(string stage, string selectedDay)
        {
            List<Stage> stages = GetStages();
            if (stage != null && stage != "all")
            {
                List<Stage> ToRemoveStages = new List<Stage>();
                foreach (Stage s in stages)
                {
                    if (!s.ID.Equals(stage)) { ToRemoveStages.Add(s); }
                }
                foreach (Stage s in ToRemoveStages)
                {
                    stages.Remove(s);
                }
            }
            if (selectedDay != null && selectedDay != "all")
            {
                foreach (Stage s in stages)
                {
                    List<lineup> ToRemoveLineups = new List<lineup>();
                    foreach (lineup l in s.Linups)
                    {
                        if (!l.DateOfPlay.Equals(selectedDay)) { ToRemoveLineups.Add(l); }                    
                    }
                    foreach(lineup l in ToRemoveLineups)
                    {
                        s.Linups.Remove(l);
                    }
                
                }
            }
            return stages;
        }
        public static List<Stage> GetStages()
        {   //inladen alle stages
            List<Stage> stages = new List<Stage>();
            string stageSql = "SELECT * FROM stages";
            DbDataReader stagereader = Database.GetData(stageSql);
            while (stagereader.Read())
            {
                Stage s = GetStages(stagereader);
                if (stages.IndexOf(s) < 0)
                {
                    stages.Add(s);
                }    
            }
            stagereader.Close();
            return stages;
        }
        private static Stage GetStages(DbDataReader stagereader)
        {
            // voor iedere stage de bijhorende linups opvragen
            List<lineup> lineups = new List<lineup>();
            Stage s = new Stage();
            s.ID = stagereader["Id"].ToString();
            s.Naam = stagereader["StageName"].ToString();
            string lineupSql = "SELECT * FROM lineup WHERE Stage = @stageID";
            DbParameter stageID = Database.AddParameter("@stageID", stagereader["Id"].ToString());
            DbDataReader lineupreader = Database.GetData(lineupSql, stageID);
            while (lineupreader.Read())
            {
                lineups.Add(GetLineup(lineupreader));

            }
            lineupreader.Close();
            s.Linups = lineups;
            return s;
        }
        private static lineup GetLineup(DbDataReader lineupreader)
        {
            lineup l = new lineup();
            DateTime dt = Convert.ToDateTime(lineupreader["DateOfPlay"].ToString());
            l.DateOfPlay = dt.ToShortDateString();
            l.Einde = lineupreader["Einde"].ToString();
            l.StageId = lineupreader["Stage"].ToString();
            l.Start = lineupreader["Start"].ToString();
            string artistSql = "SELECT * FROM artist WHERE Id = @artistID";
            DbParameter artistID = Database.AddParameter("@artistID", lineupreader["Artist"]);
            DbDataReader artistreader = Database.GetData(artistSql, artistID);
            while (artistreader.Read())
            {
                l.Artist = GetArtist(artistreader);
            }
            artistreader.Close();
            return l;
        }
        private static Artist GetArtist(DbDataReader artistreader)
        {
            Artist a = new Artist();
            a.ID = artistreader["Id"].ToString();
            a.Naam = artistreader["Naam"].ToString();
            a.Description = artistreader["Description"].ToString();
            a.Facebook = artistreader["Facebook"].ToString();
            a.Twitter = artistreader["Twitter"].ToString();
            a.Picture = artistreader["Picture"].ToString();
            a.genres = GetGenres(artistreader["Id"].ToString());
            return a;
        }
        private static List<string> GetGenres(string artistId)
        {
            List<string> genres = new List<string>();
            string sql = "select genres.GenreNaam from artist_genre INNER JOIN genres ON  artist_genre.GenreID = genres.Id where ArtistID = @artistID;";
            DbParameter artistID = Database.AddParameter("@artistID", artistId);
            DbDataReader genresReader = Database.GetData(sql, artistID);
            while (genresReader.Read())
            {
                genres.Add(genresReader["GenreNaam"].ToString());
            }
            genresReader.Close();
            return genres;
        }
        public static Artist GetArtistById(string id)
        {
            string sql = "SELECT * FROM artist WHERE Id = @id";
            DbParameter artistid = Database.AddParameter("@id", id);
            DbDataReader artistreader = Database.GetData(sql, artistid);
            artistreader.Read();
            Artist a = GetArtist(artistreader);
            artistreader.Close();
            return a;
        }
    }

}