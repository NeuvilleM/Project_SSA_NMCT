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
        public List<Stage> stages { get; set; }
        public List<String> days { get; set; }
        public StageRepository()
        {
            stages = GetStages();
            days = FestivalRepository.datesFestival();
        }
        public static List<Stage> GetStages()
        {   //inladen alle stages
            List<Stage> stages = new List<Stage>();
            string stageSql = "SELECT * FROM stages";
            DbDataReader stagereader = Database.GetData(stageSql);
            while (stagereader.Read())
            { 
                stages.Add(GetStages(stagereader));            
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
            return GetArtist(artistreader);
        }
    }

}