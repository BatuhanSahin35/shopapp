using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize]
    public class ListController:Controller
    {
        private IListService _listService;
        private UserManager<User> _userManager;
        public ListController(UserManager<User> userManager,IListService listService)
        {
            _userManager = userManager;
            _listService = listService;
        }
        public IActionResult Index()
        {
            var list = _listService.GetListByUserId(_userManager.GetUserId(User));
            
            return View(new ListModel(){
                ListId=list.Id,
                ListItems=list.ListItems.Select(i=>new ListItemModel(){
                    ListItemId=i.Id,
                    ProductId=i.ProductId,
                    Name=i.Product.Name,
                    ImageUrl=i.Product.ImageUrl
                }).ToList()
            });
        }
        [HttpPost]
        public IActionResult AddToList(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _listService.AddToList(userId,productId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteFromList(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _listService.DeleteFromList(userId,productId);
            return RedirectToAction("Index");
        }

    }
}