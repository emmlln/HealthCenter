namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TypeofProc")]
    public partial class TypeofProc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TypeofProc()
        {
            DoProc = new HashSet<DoProc>();
            MakingProcedure = new HashSet<MakingProcedure>();
            ScheduleProcedure = new HashSet<ScheduleProcedure>();
        }

        public int ID { get; set; }

        [Required]
        public string Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoProc> DoProc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MakingProcedure> MakingProcedure { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduleProcedure> ScheduleProcedure { get; set; }
    }
}
