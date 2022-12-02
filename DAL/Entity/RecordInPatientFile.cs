namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RecordInPatientFile")]
    public partial class RecordInPatientFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecordInPatientFile()
        {
            DoProc = new HashSet<DoProc>();
        }

        public int ID { get; set; }

        public int PatientID { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Result { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        public int? DoctorID { get; set; }

        public int? NurseID { get; set; }

        public virtual Doctor Doctor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoProc> DoProc { get; set; }

        public virtual Nurce Nurce { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
