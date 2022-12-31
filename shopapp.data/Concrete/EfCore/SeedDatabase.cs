using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static Category[] Categories = {
            new Category(){Name="Film",Url="Film"},
            new Category(){Name="Kitap",Url="Kitap"},
            new Category(){Name="Müzik",Url="Müzik"},
            new Category(){Name="Oyun",Url="Oyun"}
        };

        private static Product[] Products = {
            new Product(){Name="Film1",Url="film-1",ImageUrl="1.jpg",Year="2010",Description="film1"},
            new Product(){Name="Film2",Url="film-2",ImageUrl="2.jpg",Year="2012",Description="film2"},
            new Product(){Name="Kitap1",Url="kitap-1",ImageUrl="3.jpg",Year="2014",Description="kitap1"},
            new Product(){Name="Müzik1",Url="müzik-1",ImageUrl="4.jpg",Year="2015",Description="müzik1"},
            new Product(){Name="Oyun1",Url="oyun-1",ImageUrl="5.jpg",Year="2012",Description="oyun1"},
        };

        private static ProductCategory[] ProductCategories={
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[1]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},
            new ProductCategory(){Product=Products[4],Category=Categories[3]},
        };
    }
}