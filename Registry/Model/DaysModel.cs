using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class DaysModel
    {
        public int ID { get; set; }
        public string DayOfWeek { get; set; }

        public DaysModel(Days day)
        {
            ID = day.ID;
            DayOfWeek = day.DayOfWeek;
        }
    }
}
