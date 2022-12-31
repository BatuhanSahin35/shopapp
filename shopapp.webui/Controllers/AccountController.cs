using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController:Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IListService _listService;
        public AccountController(IListService listService,UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _listService=listService;
            _userManager=userManager;
            _signInManager=signInManager;
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl=ReturnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid){
                return View(model);
            }
            var user= await _userManager.FindByNameAsync(model.UserName);
            if(user==null){
                ModelState.AddModelError("","Kullanıcı adı oluşturulmamış");
                return View(model);
            }
            var result= await _signInManager.PasswordSignInAsync(user,model.Password,true,false);
            if(result.Succeeded){
                //anasayfaya gitmek için
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("","Kullanıcı adı veya şifre hatalı");
            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName  = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email    
            };           

            var result = await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded)
            {
                // generate token
                // email
                _listService.InitializeList(user.Id);
                return RedirectToAction("Login","Account");
            }

            ModelState.AddModelError("","Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
            return View(model);
        }


         public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            return Redirect("~/");	
         }

         public IActionResult AccessDenied()
         {
            return View();
         }
    }
}