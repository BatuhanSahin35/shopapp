using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IListService
    {
        void InitializeList(string userId);
        List GetListByUserId(string userId);
        void AddToList(string userId, int productId);
        void DeleteFromList(string userId, int productId);
    }
}