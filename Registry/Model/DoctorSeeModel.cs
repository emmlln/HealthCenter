using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace Registry.Model
{
    public class DoctorSeeModel
    {
        public int DoctorID { get; set; }

        public int PatientID { get; set; }

        public DateTime DateTime { get; set; }

        public bool? Visited { get; set; }

        public int ID { get; set; }
        public int? ZamID { get; set; }

        public bool Closed { get; set; }

        public DoctorSeeModel()
        {

        }
        public DoctorSeeModel(DoctorSee doctorSee)
        {
            DBAccess DB = new DBAccess();
            DoctorID = doctorSee.DoctorID;
            PatientID = doctorSee.PatientID;
            DateTime = doctorSee.DateTime;
            Visited = doctorSee.Visited;
            ID = doctorSee.ID;
            ZamID = doctorSee.ZamID;
            Closed = doctorSee.Closed;
        }
    }
}
