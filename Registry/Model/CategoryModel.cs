using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public string Category { get; set; }

        public CategoryModel()
        {

        }

        public CategoryModel(Categories categories)
        {
            ID = categories.ID;
            Category = categories.Category;
        }
    }
}
