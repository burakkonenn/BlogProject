using BlogProject.Areas.Identity;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;

        }
        BlogProject.Areas.Identity.ApplicationContext context = new ApplicationContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("/home/getprofile/{userId}")]

        public async Task<User> GetProfile(string userId)
        {
            return await context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }

        [HttpGet("/home/getcurrentuser")]

        public async Task<ActionResult<User>> GetCurrentUser()
        {
            User currentUser = new User();

            if (User.Identity.IsAuthenticated)
            {
                currentUser.Email = User.FindFirstValue(ClaimTypes.Email);
                currentUser = await context.Users.Where(u => u.Email == currentUser.Email).FirstOrDefaultAsync();

                if (currentUser == null)
                {
                    currentUser = new User();
                    currentUser.Id = context.Users.Max(user => user.Id) + 1;
                    currentUser.Email = User.FindFirstValue(ClaimTypes.Email);
                    //currentUser.Password = Utility.Encrypt(currentUser.Email);

                    context.Users.Add(currentUser);
                    await context.SaveChangesAsync();
                }
            }
            return await Task.FromResult(currentUser);
        }





        [HttpGet("/home/getcontacts")]

        public List<User> GetContacts()
        {
            return context.Users.ToList();
        }



        [HttpGet("/home/getallcontacts")]

        public List<User> GetAllContacts()
        {
            List<User> users = new();
            users.AddRange(Enumerable.Range(0, 20001).Select(x => new User { Id = $"First{x}", FirstName = $"First{x}", LastName = $"Last{x}" }));

            return users;

        }

        [HttpGet("/home/getonlyvisiblecontacts")]

        public List<User> GetOnlyVisibleContacts(int startIndex, int count)
        {
            List<User> users = new();
            users.AddRange(Enumerable.Range(startIndex, count).Select(x => new User { Id = $"First{x}", FirstName = $"First{x}", LastName = $"Last{x}" }));

            return users;
        }

        [HttpGet("/home/getcontactscount")]

        public async Task<int> GetContactsCount()
        {
            return await context.Users.CountAsync();
        }

        [HttpGet("/home/getvisiblecontacts")]

        public async Task<List<User>> GetVisibleContacts(int startIndex, int count)
        {
            return await context.Users.Skip(startIndex).Take(count).ToListAsync();
        }
    }


}
