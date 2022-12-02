using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class ProcedureModel
    {
        public int ID { get; set; }

        public int ProcID { get; set; }

        public DateTime Date { get; set; }

        public bool? Visited { get; set; }

        public int PatientID { get; set; }

        public DoProcModel DoProc { get; set; }
        public int ScheduleID { get; set; }
        public string FIO { get; set; }
        public string ProcedureType { get; set; }
        public ProcedureModel()
        {

        }

        public ProcedureModel(Procedure procedure)
        {
            DBAccess DB = new DBAccess();
            ID = procedure.ID;
            Date = procedure.Date;
            Visited = procedure.Visited;
            DoProc = DB.GetDoProcByID(procedure.DoProcID);
            PatientID = DB.GetDoProcByID(DoProc.ID).PatientID;
            FIO = DB.GetPatient(PatientID).FullName;
            ProcID = DoProc.ProcID;
            ScheduleID = procedure.ScheduleID;
            ProcedureType = DB.GetTypeProc(ProcID).Type;
        }
    }
}
