using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domains
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        //Many to many relationship
        //menas one category can containg mutiple products

        //one product can belond to multiple category
        public ICollection<Product> Products { get; set; }
        = new List<Product>();
    }
}
