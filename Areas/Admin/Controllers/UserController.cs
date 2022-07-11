using BlogProject.Areas.Identity;
using BlogProject.Areas.Users.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogProject.Areas.Admin.Controllers
{
    public class UserController : Controller
    {

      
        private UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        ApplicationContext context = new ApplicationContext();


        [Area("Admin")]

        public IActionResult Index()
        {

            ViewBag.UserId = _userManager.GetUserName(User);

            return View();
        }

     


        [HttpGet]
        [Area("Admin")]

        public IActionResult edituser(string id)
        {
            var users = context.Users.Find(id);

            var model = new UserModel()
            {
                FirstName = users.FirstName,
                UserName = users.UserName,
                AvatarUrl = users.Avatar,
            };



            return PartialView("UserEditPartial", model);
        }


        [HttpPost]
        [Area("Admin")]

        public IActionResult edituser(UserModel model, IFormFile file)
        {

            var userEdit = context.Users.Find(model.Id);

            Random rastgele = new Random();
            int rndImamge = rastgele.Next();
            var extension = Path.GetExtension(model.AvatarUpload.FileName);
            var newİmageName = "Image-" + rndImamge.ToString() + extension;
            var location = Path.Combine("wwwroot/registerupdateimage/", newİmageName);
            var stream = new FileStream(location, FileMode.Create);

            model.AvatarUpload.CopyTo(stream);

            userEdit.FirstName = model.FirstName;
            userEdit.UserName = model.UserName;
            userEdit.Personelİnfo = model.PersonelInfo;
            userEdit.Avatar = "registerupdateimage/" + newİmageName;

            context.Users.Add(userEdit);
            context.SaveChanges();
            return PartialView("UserEditPartial", userEdit);
        }







    }
}
