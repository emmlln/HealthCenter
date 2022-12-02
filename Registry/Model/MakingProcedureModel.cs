using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class MakingProcedureModel
    {
        public int ID { get; set; }

        public int ProcedureID { get; set; }

        public int NurceID { get; set; }

        public MakingProcedureModel()
        {

        }

        public MakingProcedureModel(MakingProcedure proc)
        {
            ID = proc.ID;
            ProcedureID = proc.ProcedureID;
            NurceID = proc.NurceID;
        }
    }
}
