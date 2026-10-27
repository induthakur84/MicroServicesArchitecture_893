using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domains
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        //Many to many relationship

        //one product can belond to multiple categories

        //one category can contain muiltip proudcct
        public ICollection<Category> Categories { get; set; }
        = new List<Category>(); 

    }
}
