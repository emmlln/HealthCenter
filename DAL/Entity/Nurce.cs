namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Nurce")]
    public partial class Nurce
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nurce()
        {
            MakingProcedure = new HashSet<MakingProcedure>();
            RecordInPatientFile = new HashSet<RecordInPatientFile>();
        }

        public int ID { get; set; }

        [Required]
        public string FullName { get; set; }

        public int WorkEx { get; set; }

        public bool Closed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MakingProcedure> MakingProcedure { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecordInPatientFile> RecordInPatientFile { get; set; }
    }
}
