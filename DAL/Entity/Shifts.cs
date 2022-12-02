namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shifts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shifts()
        {
            Schedule = new HashSet<Schedule>();
        }

        public int ID { get; set; }

        public int number { get; set; }

        public TimeSpan TimeofBegin { get; set; }

        public TimeSpan TimeofEnd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}
