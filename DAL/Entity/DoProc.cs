namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DoProc")]
    public partial class DoProc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DoProc()
        {
            Procedure = new HashSet<Procedure>();
        }

        public int ID { get; set; }

        public int ProcID { get; set; }

        public int RecordID { get; set; }

        public int PatientID { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual RecordInPatientFile RecordInPatientFile { get; set; }

        public virtual TypeofProc TypeofProc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Procedure> Procedure { get; set; }
    }
}
