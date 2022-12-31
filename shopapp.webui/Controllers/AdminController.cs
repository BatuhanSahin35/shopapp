using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController: Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController
                            (IProductService productService,
                            ICategoryService categoryService,
                            RoleManager<IdentityRole> roleManager,
                            UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user!=null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailModel(){
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/userlist");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailModel model,string[] selectedroles) {
            if(ModelState.IsValid){
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null){
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                     var result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded){
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedroles = selectedroles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedroles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedroles).ToArray<string>());
                        return Redirect("/admin/userlist");
                    }
                }
                return Redirect("/admin/userlist");
            }
            return View(model);
        }



        public IActionResult UserList(){
            return View(_userManager.Users);
        }



        [HttpGet]
        public async  Task<IActionResult> RoleEdit(string id){
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonmembers = new List<User>();
            foreach (var user in _userManager.Users.ToList())
            {
               var list = await _userManager.IsInRoleAsync(user,role.Name)
                                ?members:nonmembers;
               list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async  Task<IActionResult> RoleEdit(RoleEditModel model){
            if(ModelState.IsValid){
                foreach (var userId in model.IdsToAdd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if(result.Succeeded){
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                        if(result.Succeeded){
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/" + model.RoleId);
        }


        public IActionResult RoleList(){
            return View(_roleManager.Roles);
        }

        [HttpGet]
        public IActionResult RoleCreate(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model){
            if(ModelState.IsValid){
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded){
                    return RedirectToAction("RoleList");
                }
                else{
                    foreach(var item in result.Errors){
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            
            return View();
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Year = model.Year,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };
                
                _productService.Create(entity);


                TempData["message"] =  $"{entity.Name} isimli ürün eklendi.";

                // {"Message":"samsung isimli ürün eklendi.","AlertType":"success"}

                return RedirectToAction("ProductList");
            }
            return View(model);
        }

         [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
             if(ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url            
                };
                
                _categoryService.Create(entity);


                TempData["message"] =  $"{entity.Name} isimli category eklendi.";


                return RedirectToAction("CategoryList");
            }
            return View(model);
            
        }


        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if(entity==null)
            {
                 return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Year = entity.Year,
                ImageUrl= entity.ImageUrl,
                Description = entity.Description,
                SelectedCategories = entity.ProductCategories.Select(i=>i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public IActionResult ProductEdit(ProductModel model,int[] categoryIds)
        {
            var entity = _productService.GetById(model.ProductId);
            if(entity==null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            entity.Url = model.Url;
            entity.Year = model.Year;
            entity.ImageUrl = model.ImageUrl;
            entity.Description = model.Description;

            _productService.Update(entity,categoryIds);


            TempData["message"] =  $"{entity.Name} isimli ürün güncellendi.";

            return RedirectToAction("ProductList");
        }
       
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if(entity==null)
            {
                 return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p=>p.Product).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.CategoryId);
            if(entity==null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            entity.Url = model.Url;

            _categoryService.Update(entity);


            TempData["message"] =  $"{entity.Name} isimli category güncellendi.";

            return RedirectToAction("CategoryList");
        }
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            if(entity!=null)
            {
                _productService.Delete(entity);
            }



            TempData["message"] =  $"{entity.Name} isimli ürün silindi.";

            return RedirectToAction("ProductList");
        }
        
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if(entity!=null)
            {
                _categoryService.Delete(entity);
            }



            TempData["message"] =  $"{entity.Name} isimli category silindi.";

            return RedirectToAction("CategoryList");
        }
    
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId,categoryId);
            return Redirect("/admin/categories/"+categoryId);
        }
    
    }
}