namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Schedule")]
    public partial class Schedule
    {
        public int ID { get; set; }

        public int DoctorID { get; set; }

        public int Room { get; set; }

        public int dayofWeek { get; set; }

        public int? ZamID { get; set; }

        public int ShiftID { get; set; }

        public virtual Days Days { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Shifts Shifts { get; set; }
    }
}
