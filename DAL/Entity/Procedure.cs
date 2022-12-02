namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Procedure")]
    public partial class Procedure
    {
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public bool? Visited { get; set; }

        public int DoProcID { get; set; }

        public int ScheduleID { get; set; }

        public virtual DoProc DoProc { get; set; }

        public virtual ScheduleProcedure ScheduleProcedure { get; set; }
    }
}
