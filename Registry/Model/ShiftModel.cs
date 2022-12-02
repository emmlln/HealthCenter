using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class ShiftModel
    {
        public int ID { get; set; }
        public int number { get; set; }
        public TimeSpan TimeofBegin { get; set; }
        public TimeSpan TimeofEnd { get; set; }

        public ShiftModel(Shifts shift)
        {
            ID = shift.ID;
            number = shift.number;
            TimeofBegin = shift.TimeofBegin;
            TimeofEnd = shift.TimeofEnd;
        }
    }
}
