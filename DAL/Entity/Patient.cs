namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Patient")]
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            DoctorSee = new HashSet<DoctorSee>();
            DoProc = new HashSet<DoProc>();
            RecordInPatientFile = new HashSet<RecordInPatientFile>();
        }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Male { get; set; }

        public int ID { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(50)]
        public string PlaceOfWork { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Document { get; set; }

        public int StreetID { get; set; }

        public bool Closed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoctorSee> DoctorSee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoProc> DoProc { get; set; }

        public virtual Streets Streets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecordInPatientFile> RecordInPatientFile { get; set; }
    }
}
