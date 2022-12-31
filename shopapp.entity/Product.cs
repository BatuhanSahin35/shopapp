using System.Collections.Generic;

namespace shopapp.entity
{
    public class Product
    {
        public int ProductId { get; set; }  
        public string Name { get; set; }       
        public string Url { get; set; }       
        public string Description { get; set; }
        public string Year { get; set; }        
        public string ImageUrl { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}