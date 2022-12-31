using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface IListRepository:IRepository<List>
    {
        List GetListByUserId(string userId);
        void DeleteFromList(int listId, int productId);
        
    }
}