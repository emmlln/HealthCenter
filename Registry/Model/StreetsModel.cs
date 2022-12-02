using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class StreetsModel
    {
        public int ID { get; set; }

        public string Street { get; set; }

        public int PlaceOfSee { get; set; }

        public StreetsModel()
        {

        }
        public StreetsModel(Streets street)
        {
            ID = street.ID;
            Street = street.Street;
            PlaceOfSee = street.PlaceOfSee;
        }
    }
}
