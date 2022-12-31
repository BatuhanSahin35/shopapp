using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class ListManager : IListService
    {
        private IListRepository _listRepository;
        public ListManager(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public void AddToList(string userId, int productId)
        {
            var list = GetListByUserId(userId);
            if(list != null)
            {
                var index = list.ListItems.FindIndex(i => i.ProductId == productId);
                if(index < 0)
                {
                    list.ListItems.Add(new ListItem()
                    {
                        ProductId = productId,
                        ListId = list.Id
                    });
                }
                else
                {   
                    //burda zaten ürün listede var uyarısı verebilir
                    //sorun çıkarsa buraya ayar çekilecek
                }
                _listRepository.Update(list);
            }
        }

        public void DeleteFromList(string userId, int productId)
        {
            var list = GetListByUserId(userId);
            if(list != null)
            {
                _listRepository.DeleteFromList(list.Id,productId);
            }
        }

        public List GetListByUserId(string userId)
        {
            return _listRepository.GetListByUserId(userId);
        }

        public void InitializeList(string userId)
        {
            _listRepository.Create(new List()
            {
                UserId = userId
            });
        }
    }
}