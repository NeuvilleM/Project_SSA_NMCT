using DBHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace FestivalRegistratie.Models.DAL
{
    public class FestivalRepository
    {
        public static Festival getFestival()
        {
            string sql = "SELECT * FROM festival";
            DbDataReader festivalreader = Database.GetData(sql);
            festivalreader.Read();
            
                return CreateFestival(festivalreader);
            
        }

        private static Festival CreateFestival(DbDataReader festivalreader)
        {
            Festival f = new Festival();
            f.Festivalnaam = festivalreader["FestivalNaam"].ToString();
            f.PicturePath = festivalreader["Picture"].ToString();
            string start = festivalreader["Start"].ToString();
            f.Start = Convert.ToDateTime(start);
            f.End = Convert.ToDateTime(festivalreader["End"].ToString());
            return f;
        }
        public static List<String> datesFestival()
        {
           List<Stage> stages = StageRepository.GetStages();
           List<String> dates = new List<string>();
           foreach (Stage s in stages)
           {
               foreach (lineup l in s.Linups)
               {
                   if (!(dates.IndexOf(l.DateOfPlay) > -1))
                   {
                       dates.Add(l.DateOfPlay);
                   }
               }
           }
           dates.Sort();
            return dates;        
        }
    }
}