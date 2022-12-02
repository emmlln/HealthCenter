using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class DoProcModel
    {
        public int ID { get; set; }
        public int ProcID { get; set; }
        public int RecordID { get; set; }
        public int PatientID { get; set; }

        public DoProcModel()
        {

        }
        public DoProcModel(DoProc proc)
        {
            ID = proc.ID;
            ProcID = proc.ProcID;
            RecordID = proc.RecordID;
            PatientID = proc.PatientID;
        }
    }
}
