namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Doctor")]
    public partial class Doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctor()
        {
            DoctorSee = new HashSet<DoctorSee>();
            RecordInPatientFile = new HashSet<RecordInPatientFile>();
            Schedule = new HashSet<Schedule>();
            Users = new HashSet<Users>();
        }

        public int PlaceOfSee { get; set; }

        public int SpecializationID { get; set; }

        [Required]
        public string FullName { get; set; }

        public int ID { get; set; }

        public int CategoryID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ZamEnd { get; set; }

        public bool Certificate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ZamStart { get; set; }

        public bool Closed { get; set; }

        public int? ZamID { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual Specialization Specialization { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoctorSee> DoctorSee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecordInPatientFile> RecordInPatientFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users> Users { get; set; }
    }
}
