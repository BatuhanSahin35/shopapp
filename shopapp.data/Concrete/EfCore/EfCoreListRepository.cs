using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreListRepository : EfCoreGenericRepository<List, ShopContext>, IListRepository
    {
        public void DeleteFromList(int listId, int productId)
        {
            using(var context = new ShopContext()){
                var cmd = @"delete from ListItems where ListId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd, listId, productId);
            }
        }

        public List GetListByUserId(string userId)
            {
                using (var context = new ShopContext())
                {
                    return context.Lists
                            .Include(i => i.ListItems)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefault(i => i.UserId == userId);
                }
            }
        public override void Update(List entity)
        {
            using (var context = new ShopContext())
            {
                context.Lists.Update(entity);
                context.SaveChanges();   
            }
        }
    }
}