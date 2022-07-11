using BlogProject.Areas.Admin.Model;
using BlogProject.Areas.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationContext context = new ApplicationContext();

        [Area("Admin")]
        public IActionResult Index()
        {
            var model = context.Categories.Select(i => new CategoryModel
            {
                Id = i.Id,
                Name = i.Name
            });

            return View(model);
        }
   
    
    
    
    
    }
}
