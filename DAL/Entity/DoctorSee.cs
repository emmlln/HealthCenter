namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DoctorSee")]
    public partial class DoctorSee
    {
        public int DoctorID { get; set; }

        public int PatientID { get; set; }

        public DateTime DateTime { get; set; }

        public bool? Visited { get; set; }

        public int ID { get; set; }

        public int? ZamID { get; set; }

        public bool Closed { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
