using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface ICategoryService
    {
        Category GetById(int id);

        Category GetByIdWithProducts(int categoryId);

        List<Category> GetAll();

        void Create(Category entity);

        void Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int productId,int categoryId);
    }
}