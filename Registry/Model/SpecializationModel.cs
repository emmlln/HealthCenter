using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class SpecializationModel
    {
        public string SpecializationName { get; set; }

        public int ID { get; set; }

        public SpecializationModel()
        {

        }
        public SpecializationModel(Specialization sp)
        {
            SpecializationName = sp.SpecializationName;
            ID = sp.ID;
        }
    }
}
