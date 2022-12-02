namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MakingProcedure")]
    public partial class MakingProcedure
    {
        public int ID { get; set; }

        public int ProcedureID { get; set; }

        public int NurceID { get; set; }

        public virtual Nurce Nurce { get; set; }

        public virtual TypeofProc TypeofProc { get; set; }
    }
}
