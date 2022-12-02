namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScheduleProcedure")]
    public partial class ScheduleProcedure
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScheduleProcedure()
        {
            Procedure = new HashSet<Procedure>();
        }

        public int ID { get; set; }

        public int Room { get; set; }

        public int DayID { get; set; }

        public int ProcedureID { get; set; }

        public int Count { get; set; }

        public bool Closed { get; set; }

        public virtual Days Days { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Procedure> Procedure { get; set; }

        public virtual TypeofProc TypeofProc { get; set; }
    }
}
