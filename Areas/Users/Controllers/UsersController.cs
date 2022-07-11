using BlogProject.Areas.Admin.Model;
using BlogProject.Areas.Identity;
using BlogProject.Areas.Identity.Model;
using BlogProject.Areas.Users.Model;
using BlogProject.Context;
using BlogProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlogProject.Areas.Users.Controllers
{

    [Authorize]
    public class UsersController : Controller
    {


        private UserManager<User> _userManager;

        [Obsolete]
        private IHostingEnvironment hostingEnv;


        [Obsolete]
        public UsersController(UserManager<User> userManager, IHostingEnvironment env)
        {
            _userManager = userManager;
            this.hostingEnv = env;
        }



        ApplicationContext context = new ApplicationContext();

        [Area("Users")]
        public async Task<IActionResult> Index()
        {
          
            var LoginUser = await _userManager.GetUserAsync(User);

            var followIds = context.Follows.Where(x => x.IsFollowing == true).Select(i => i.FolloweeId).ToList();

            var usersFollowing = context.Users.Where(x => !followIds.Contains(x.Id) && x.Id != LoginUser.Id).Include(i => i.Follower).Select(a => new UserModel()
            {

                AvatarUrl = a.Avatar,
                FirstName = a.FirstName,
                PersonelInfo = a.Personelİnfo,
                LastName = a.LastName,
                Id = a.Id,

            }).ToList();

            var blogs = context.Blogs.Where(a => a.IsDeleted == false).Include(i => i.User).OrderByDescending(a => a.Date).ToList();
        
            var Categories = context.Categories.Select(i => new BlogProject.Areas.Users.Model.CategoryModel()
            {
                Name = i.Name
            }).Take(7).ToList();

      
            var lastBlogs = context.List.Where(a => a.UserId == LoginUser.Id).OrderByDescending(a => a.Date).Select(a => new SelectModel()
            {

                FirstName = a.Blog.User.FirstName,
                LastName = a.Blog.User.LastName,
                BlogTitle = a.Blog.Title,
                BlogContent = a.Blog.Content,
                Avatar = a.Blog.User.Avatar,
            }).Take(3).ToList();



            var model = new ViewModel()
            {
                Category = Categories,
                Following = usersFollowing,
                Blogs = blogs,
                User = LoginUser,
                LastBlogs = lastBlogs
            };



            return View(model);
        }



        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> Set()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var model = new BlogViewModel()
            {
                User = currentUser
            };
            return View(model);
        }



        [Area("Users")]
        [HttpPost]
        public async Task<IActionResult> Set(BlogViewModel model)
        {

            if (ModelState.IsValid)
            {

                var category = new Category()
                {
                    Name = model.Tag
                };

                 await context.Categories.AddAsync(category);
                 await context.SaveChangesAsync();

                var currentUser = await _userManager.GetUserAsync(User);


                Random rastgele = new Random();
                int rndImamge = rastgele.Next();
                var extension = Path.GetExtension(model.BlgImage.FileName);
                var newİmageName = "Image-" + rndImamge.ToString() + extension;
                var location = Path.Combine("wwwroot/blgImg/", newİmageName);
                var stream = new FileStream(location, FileMode.Create);

                model.BlgImage.CopyTo(stream);


                var blogModel = new Blog()
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryId = category.Id,
                    Description = model.Description,
                    Date = DateTime.Now,
                    Image = "blgImg/" + newİmageName,
                    UserId = currentUser.Id
                };
                context.Blogs.Add(blogModel);
                context.SaveChanges();
                return RedirectToAction("Home", "Users", new { area = "Users" });

            }

            return View(model);

        }



        [Area("Users")]
        [HttpGet]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UserId(string id)
        {
            var FirstUser = await _userManager.GetUserAsync(User);
            var FirstUserId = FirstUser.Id;

            var FollewedUser = await _userManager.FindByIdAsync(id);
            var SecondUserId = FollewedUser.Id;

            if (context.Follows.Any(x => x.FollowerId == FirstUserId && x.FolloweeId == SecondUserId))
            {
                var data = context.Follows.FirstOrDefault(x => x.FollowerId == FirstUserId);
                data.IsFollowing = true;
            }
            else
            {
                var model = new Follow()
                {
                    FollowerId = FirstUserId,
                    FolloweeId = SecondUserId,
                    IsFollowing = true,
                };

                context.Follows.Add(model);
            }

            context.SaveChanges();
            return Ok();
        }

        [Area("Users")]
        [HttpGet]
        [ValidateAntiForgeryToken]

        public IActionResult IsFollowing(string id)
        {
            Follow follows = context.Follows.Where(i => i.FolloweeId == id).FirstOrDefault();
            follows.IsFollowing = false;
            context.SaveChanges();

            return Ok();
        }



        [Area("Users")]
        [HttpGet]

        public async Task<IActionResult> Home()
        {

    
            var AllUsers = new ApplicationContext();
            var LoginUser = await _userManager.GetUserAsync(User);

            var AllFallowedUser = AllUsers.Follows.Where(i => i.FollowerId == LoginUser.Id && i.IsFollowing == true).Include(i => i.Follower).ToList();
            var AllFallowerUser = AllUsers.Follows.Where(i => i.FolloweeId == LoginUser.Id && i.IsFollowing == true).ToList();

            //login olan ve takip edilen kullanıcı dışındaki kullanıcı listesi
            var followIds = context.Follows.Where(x => x.IsFollowing == true).Select(i => i.FolloweeId).ToList();
         
            var users = context.Users.Where(x => !followIds.Contains(x.Id) && x.Id != LoginUser.Id).Include(i => i.Follower).Select(a => new UserModel()
            {

                AvatarUrl = a.Avatar,
                FirstName = a.FirstName,
                PersonelInfo = a.Personelİnfo,
                LastName = a.LastName,
                Id = a.Id,

            }).ToList();

            //takip edilen kullanıcı sayısı*
            var FollowedCount = AllUsers.Follows.Where(i => i.FollowerId == LoginUser.Id && i.IsFollowing == true).Count();
            var FollowerCount = context.Follows.Where(i => i.FolloweeId == LoginUser.Id && i.IsFollowing == true).Count();

            //kullanıcını blogları
            //var UserBlogs = AllUsers.Users.Where(i => i.Id == LoginUser.Id).Include(i => i.Blogs.Where(a => a.IsDeleted == false)).ToList();

            var userBlog = context.Blogs.Where(i => i.UserId == LoginUser.Id && i.IsDeleted == false).Select(a => new SelectBlogModel() {
            
                Id = a.Id,
                BlogContent = a.Content,
                BlogDate = a.Date,
                BlogTitle = a.Title,
                BlogUserFirstName = a.User.FirstName,
                BlogUserLastName = a.User.LastName,
                BlogImage = a.Image,
                BlogUserAvatar = a.User.Avatar

            }).ToList();

            var userLikes = context.Likes.Where(i => i.UserId == LoginUser.Id && i.IsLike == true).Include(a => a.Blog).ThenInclude(s => s.User).Select(e => new ViewModel()
            { 
                LikeId = e.Id,
                UserName = e.Blog.User.UserName,
                FirstName = e.Blog.User.FirstName,
                UserAvatar = e.Blog.User.Avatar,
                BlogsTitle = e.Blog.Title,
                BlogsContent = e.Blog.Content,
                BlogImage = e.Blog.Image,
                BlogUserFirstname = e.Blog.User.FirstName,
                BlogUserLastname = e.Blog.User.LastName,


            }).ToList();

            var myList = context.List.Where(a => a.UserId == LoginUser.Id && a.IsDeleted == false).Include(a => a.Blog).Include(s => s.User).Select(a => new ListModel() {
            
                BlogId = a.Blog.Id,
                Title = a.Blog.Title,
                Image = a.Blog.Image,
                Content = a.Blog.Content,
                UserFirstname = a.Blog.User.FirstName,
                UserLastname = a.Blog.User.LastName,
                UserImage = a.Blog.User.Avatar
            
            }).ToList();

            var lastBlogs = context.Blogs.Where(a => a.IsDeleted == false).OrderByDescending(a => a.Date).Select(a => new SelectModel()
            {
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                BlogTitle = a.Title,
                BlogContent = a.Content,
                PersonelInfo = a.User.Personelİnfo,
                Avatar = a.User.Avatar,
            }).Take(5).ToList();



            var model = new ViewModel()
            {
                Following = users,
                Ublog = userBlog,
                AllFollowedUser = AllFallowedUser,
                AllFollowerUser = AllFallowerUser,
                User = LoginUser,
                FollowedCount = FollowedCount,
                FollowerCount = FollowerCount,
                UserLikes = userLikes,
                myList = myList,
                LastBlogs = lastBlogs
            };


            return View(model);
        }


        [Area("Users")]
        [Route("/kullanıcı/{username}")]
        public async Task<IActionResult> UserProfile(string username)
        {
            var context = new ApplicationContext();
            var user = context.Users.Where(i => i.UserName == username).FirstOrDefault();
            var userId = user.Id;
            


            var Categories = context.Categories.Select(i => new BlogProject.Areas.Users.Model.CategoryModel()
            {
                Name = i.Name
            }).ToList();


            var LoginUser = await _userManager.GetUserAsync(User);

            var users = context.Users.Where(i => i.Id != LoginUser.Id).Take(5).Select(a => new UserModel()
            {

                AvatarUrl = a.Avatar,
                FirstName = a.FirstName,
                PersonelInfo = a.Personelİnfo,
                LastName = a.LastName,
                Id = a.Id,

            }).ToList();

            var AllFallowedUser = context.Follows.Where(i => i.FollowerId == userId && i.IsFollowing == true).Include(i => i.Follower).ToList();

            var AllFallowerUser = context.Follows.Where(i => i.FolloweeId == userId && i.IsFollowing == true).Include(i => i.Followee).ToList();

            var FollowedCount = context.Follows.Where(i => i.FollowerId == userId && i.IsFollowing == true).Count();

            var FollowerCount = context.Follows.Where(i => i.FolloweeId == userId && i.IsFollowing == true).Count();

            var userBlog = context.Blogs.Where(i => i.UserId == LoginUser.Id && i.IsDeleted == false).Select(a => new SelectBlogModel()
            {

                Id = a.Id,
                BlogContent = a.Content,
                BlogDate = a.Date,
                BlogTitle = a.Title,
                BlogUserFirstName = a.User.FirstName,
                BlogUserLastName = a.User.LastName,
                BlogImage = a.Image,
                BlogUserAvatar = a.User.Avatar

            }).ToList();

            var lastBlogs = context.Blogs.Where(a => a.IsDeleted == false).OrderByDescending(a => a.Date).Select(a => new SelectModel()
            {
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                BlogTitle = a.Title,
                BlogContent = a.Content,
                PersonelInfo = a.User.Personelİnfo,
                Avatar = a.User.Avatar,
            }).Take(5).ToList();


            var model = new ViewModel()
            {
                Category = Categories,
                Users = users,
                AllFollowedUser = AllFallowedUser,
                AllFollowerUser = AllFallowerUser,
                User = user,
                LoginUserImage = LoginUser,
                FollowedCount = FollowedCount,
                FollowerCount = FollowerCount,
                UsersBlog = userBlog,
                LastBlogs = lastBlogs
            };



            return View(model);

        }


        //[Area("Users")]
        //public async Task<IActionResult> Followed()
        //{
        //    var Categories = context.Categories.Select(i => new BlogProject.Areas.Users.Model.CategoryModel()
        //    {
        //        Name = i.Name
        //    }).ToList();
        //    var LoginUser = await _userManager.GetUserAsync(User);

        //    var followed = context.Follows.Where(i => i.FollowerId == LoginUser.Id && i.IsFollowing == true).Include(i => i.Follower).ToList();
        //    var follower = context.Follows.Where(i => i.FolloweeId == LoginUser.Id && i.IsFollowing == true).Include(i => i.Followee).ToList();

        //    var followIds = context.Follows.Where(x => x.IsFollowing == true).Select(i => i.FolloweeId).ToList();
        //    var users = context.Users.Where(x => !followIds.Contains(x.Id) && x.Id != LoginUser.Id).Include(i => i.Follower).ToList();

        //    var model = new ViewModel()
        //    {
        //        Followed = followed,
        //        Follower = follower,
        //        Category = Categories,
        //        Following = users,
        //        User = LoginUser
        //    };


        //    return Json("asdas");
        //}


        //[Area("Users")]

        //public async Task<IActionResult> Follower()
        //{
        //    var Categories = context.Categories.Select(i => new BlogProject.Areas.Users.Model.CategoryModel()
        //    {
        //        Name = i.Name
        //    }).ToList();
        //    var LoginUser = await _userManager.GetUserAsync(User);

        //    var followed = context.Follows.Where(i => i.FollowerId == LoginUser.Id && i.IsFollowing == true).Include(i => i.Follower).ToList();
        //    var follower = context.Follows.Where(i => i.FolloweeId == LoginUser.Id && i.IsFollowing == true).Include(i => i.Followee).ToList();

        //    var followIds = context.Follows.Where(x => x.IsFollowing == true).Select(i => i.FolloweeId).ToList();
        //    var users = context.Users.Where(x => !followIds.Contains(x.Id) && x.Id != LoginUser.Id).Include(i => i.Follower).ToList();

        //    var model = new ViewModel()
        //    {
        //        Followed = followed,
        //        Follower = follower,
        //        Category = Categories,
        //        Following = users,
        //        User = LoginUser
        //    };


        //    return View(model);
        //}



        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> like(int id, ViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var blog = await context.Blogs.FindAsync(id);


            if (context.Likes.Any(x => x.UserId == user.Id && x.BlogId == blog.Id))
            {
                var data = context.Likes.FirstOrDefault(x => x.UserId == user.Id && x.BlogId == blog.Id);
                data.IsLike = true;

            }
            else
            {
                var model = new Likes()
                {
                    UserId = user.Id,
                    BlogId = blog.Id,
                    IsLike = true,
                    Date = DateTime.Now
                };

                await context.Likes.AddAsync(model);

            }
            await context.SaveChangesAsync();


            var blogLikeCount = context.Blogs.Where(i => i.Id == id).Include(a => a.Likes).Count();
            var Jsonİnfo = JsonConvert.SerializeObject(blogLikeCount);

            return Json(Jsonİnfo);
        }


        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> unlike(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var blog = context.Blogs.Where(i => i.Id == id).Include(a => a.Likes).FirstOrDefault();
            foreach (var item in blog.Likes)
            {
                item.IsLike = false;
                await context.SaveChangesAsync();
            }

            
            await context.SaveChangesAsync();

            return Ok();
        }



        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> savelist(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var blog = await context.Blogs.FindAsync(id);


            if (context.List.Any(x => x.UserId == user.Id && x.BlogId == blog.Id))
            {
                var data = context.List.FirstOrDefault(x => x.UserId == user.Id && x.BlogId == blog.Id);
                data.IsDeleted = false;

            }
            else
            {
                var model = new List()
                {
                    UserId = user.Id,
                    BlogId = blog.Id,
                    IsDeleted = false,
                    Date = DateTime.Now
                };

                await context.List.AddAsync(model);
            }

            await context.SaveChangesAsync();
            var JsonData = JsonConvert.SerializeObject(id);

            return Json(JsonData);
        }


        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> removelist(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userList = context.Users.Where(i => i.Id == user.Id).Include(q => q.List).FirstOrDefault();

            foreach (var item in userList.List)
            {
                item.IsDeleted = true;
                await context.SaveChangesAsync();
            }

            return Ok();
        }


        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> removefrombookmark(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userList = await context.List.Where(i => i.Id == id).ToListAsync();
            foreach (var item in userList)
            {
                item.IsDeleted = true;
                await context.SaveChangesAsync();
            };

            return Ok();
        }

        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> removefromblog(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userBlog = await context.Blogs.Where(i => i.Id == id).FirstOrDefaultAsync();

            userBlog.IsDeleted = true;
            await context.SaveChangesAsync();

            return Ok();
        }

        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> removefromlikes(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userLikes = await context.Likes.Where(i => i.Id == id).FirstOrDefaultAsync();

            userLikes.IsLike = false;
            await context.SaveChangesAsync();
            return Ok();

        }

        //[Area("Users")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> addcomment(ViewModel modal, int id)
        //{

        //    var user = await _userManager.GetUserAsync(User);


        //    var comment = new Comment()
        //    {
        //        UserId = user.Id,
        //        Message = modal.Comment,
        //        Date = DateTime.Now,
        //        Email = user.Email,
        //        BlogId = id
        //    };
        //    context.Comments.Add(comment);
        //    context.SaveChanges();
        
        //var commentId = context.Comments.Where(i => i.UserId == user.Id).FirstOrDefault();

        //    NotificationCommentDetail notificationCommentDetail = new NotificationCommentDetail()
        //    {
        //        ActorId = user.Id,
        //        Date = DateTime.Now,
        //        Status = true,
        //        CommentId = commentId.Id,
        //        NotificationMessage = "size yorum yaptı."
        //    };
        //    await context.NotificationCommentDetails.AddAsync(notificationCommentDetail);
        //    await context.SaveChangesAsync();

        //    var notificationCommentDetailId = context.NotificationCommentDetails.Where(i => i.ActorId == user.Id).FirstOrDefault();




        //    NotificationComment notificationComment = new NotificationComment()
        //    {
        //        NotifierId = modal.ReceiverId,
        //        Date = DateTime.Now,
        //        NotificationCommentDetailId = notificationCommentDetailId.Id
        //    };
        //    await context.NotificationComments.AddAsync(notificationComment);
        //    await context.SaveChangesAsync();

        //    var JsonModel = new ViewModel()
        //    {
        //        User = user,
        //        comment = comment
        //    };

        //    var JsonInfo = JsonConvert.SerializeObject(JsonModel);

        //    return Json(JsonInfo);
        //}

        
        [Area("Users")]
        [Route("/anasayfa/blog/{title}")]
        public async Task<IActionResult> UserBlogDetails(string title)
        {

            if(title == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            
             
            var userBlogDetails = await context.Blogs.Where(i => i.Title == title).Include(a => a.User).ThenInclude(e => e.Comments).Include(a => a.Likes).Select(o => new UserModel()
            {
                Id = o.User.Id,
                FirstName = o.User.FirstName,
                Content = o.Content,
                Date = o.Date,
                BlogId = o.Id,
                LastName = o.User.LastName,
                Title = o.Title,
                ContentLengthCount = o.Content.Length,
                AvatarUrl = o.User.Avatar,
                Followee = o.User.Followee,
                PersonelInfo = o.User.Personelİnfo,
                WebSite = o.User.WebSite,
                Comments = o.Comments.Select(a => new SelectCommentModel
                {

                    CommentFirstname = a.User.FirstName,
                    CommentLastname = a.User.LastName,
                    CommentMessage = a.Message,

                }).ToList()
            }).FirstOrDefaultAsync();
            

            var tags = context.Categories.Select(i => new BlogProject.Areas.Users.Model.CategoryModel() { Name = i.Name }).ToList();
       
            var LastblogByDate =  context.Blogs.Include(a => a.User).ToList().Select(i => new BlogViewModel()
            {
                Image = i.Image,
                Title = i.Title,
                Firstname = i.User.FirstName,
                LastName = i.User.LastName,
                Date = i.Date,
                UserAvatar = i.User.Avatar,
                PersonalInfo = i.User.Personelİnfo,
                Location = i.User.Location,
                WebSite = i.User.WebSite,

            }).OrderByDescending(a => a.Date).Take(3).ToList();

            var model = new ViewModel()
            {
                Tags = tags,
                User = user,
                BlogDetails = userBlogDetails,
                UserBlogDetail = LastblogByDate
            };

            return View(model);
        }


        [Area("Users")]
        [HttpGet]
        [Route("/kullanici/{username}/hesap-ayarlari")]

        public async Task<IActionResult> Editprofile(string username)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserFirstName = user.FirstName;
            ViewBag.UserName = user.UserName;
            ViewBag.UserAvatar = user.Avatar;
            ViewBag.userId = user.Id;

            if (username == null)
            {
                return NotFound();
            }
            var userProfile = await context.Users.Where(i => i.UserName == username).Select(a => new RegisterEditViewModel() 
            { 
                Id = a.Id,
                FirstName = a.FirstName,
                AvatarUrl = a.Avatar,
                Location = a.Location,
                WebSite = a.WebSite,
                PersonalInfo = a.Personelİnfo

            }).FirstOrDefaultAsync();

            var model = new RegisterEditViewModel()
            {
                Id = userProfile.Id,
                FirstName = userProfile.FirstName,
                AvatarUrl = userProfile.AvatarUrl,
                Location = userProfile.Location,
                WebSite = userProfile.WebSite,
                PersonalInfo = userProfile.PersonalInfo

            };

            return View(model);
        }



        [Area("Users")]
        [HttpPost]
        [Route("/kullanici/{username}/hesap-ayarlari")]
        public async Task<IActionResult> Editprofile(BlogProject.Areas.Users.Model.RegisterEditViewModel model,IFormFile file, string Id)
        {


            if (ModelState.IsValid)
            {
                var user = await context.Users.FindAsync(Id);
                if (user == null)
                {
                    return NotFound();
                }
                
                if(model.AvatarUpload != null)
                {

                    Random rastgele = new Random();
                    int rndImamge = rastgele.Next();
                    var extension = Path.GetExtension(model.AvatarUpload.FileName);
                    var newİmageName = "Image-" + rndImamge.ToString() + extension;
                    var location = Path.Combine("wwwroot/registerupdateimage/", newİmageName);
                    var stream = new FileStream(location, FileMode.Create);

                    model.AvatarUpload.CopyTo(stream);
                    user.Avatar = "registerupdateimage/" + newİmageName;

                }

                user.FirstName = model.FirstName;
                user.Location = model.Location;
                user.WebSite = model.WebSite;
                user.Personelİnfo = model.PersonalInfo;

                await context.SaveChangesAsync();
                context.Entry(user).State = EntityState.Modified;

                return RedirectToAction("Home", "Users", new { area = "Users" });
            }

            return View(model);
        }



        [Area("Users")]
        [Route("/mesajlar")]

        public async Task<IActionResult> Messages()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserFirstName = user.FirstName;
            ViewBag.UserName = user.UserName;
            ViewBag.UserAvatar = user.Avatar;
            ViewBag.userId = user.Id;


            var followed = context.Follows.Where(i => i.FollowerId == user.Id && i.IsFollowing == true).Include(i => i.Follower).Select(a => new FollowModel() {
                Id = a.Follower.Id,
                Avatar = a.Follower.Avatar,
                FirstName = a.Follower.FirstName,
                UserName = a.Follower.UserName
            }).ToList();

            var receiverId = context.Messages.Where(i => i.ReceiverId == user.Id && i.IsDeleted == false).Include(a => a.Sender).Select(a => new UserMessageModel() {
            
                MessageId = a.Id,
                SenderAvatar = a.Sender.Avatar,
                SenderFirstname = a.Sender.FirstName,
                SenderMessageText = a.Text,
                MessageDate = a.Date,
                Subject = a.Subject
            
            }).ToList();

           

            var model = new ViewModel()
            {
                MessagesForReceiver = receiverId,
                FollowModel = followed,
                User = user
            };


            return View(model);
        }


        //[Area("Users")]
        //[HttpPost]
        //public async Task<IActionResult> SendMessageToUsers(ViewModel modal, string id)
        //{
        //    var user = await _userManager.GetUserAsync(User);


        //    var message = new BlogProject.Context.Message()
        //    {
        //        Text = modal.Text,
        //        ReceiverId = id,
        //        SenderId = user.Id,
        //        Date = DateTime.Now,
        //        IsDeleted = false
        //    };
        //    await context.Messages.AddAsync(message);
        //    await context.SaveChangesAsync();

        //    var jsonMessage = JsonConvert.SerializeObject(message);

        //    return Json(jsonMessage);
        //}


        [Area("Users")]
        public async Task<IActionResult> Notifications()
        {

            var user = await _userManager.GetUserAsync(User);

            var Categories = context.Categories.Select(i => new BlogProject.Areas.Users.Model.CategoryModel()
            {
                Name = i.Name
            }).Take(7).ToList();

            var users = context.Users.Where(i => i.Id != user.Id).Select(a => new UserModel()
            {

                AvatarUrl = a.Avatar,
                FirstName = a.FirstName,
                PersonelInfo = a.Personelİnfo,
                LastName = a.LastName,
                Id = a.Id,

            }).Take(5).ToList();

            var useno = context.Notification.Where(a => a.NotifierId == user.Id && a.FollowId != null).Include(a => a.Entity_Type).ThenInclude(a => a.Notifications).ThenInclude(s => s.Follow).ToList();


        

            var model = new ViewModel()
            {
                Category = Categories,
                Users = users,
                User = user,
                Notif = useno
            };


            return View(model);
        }


        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> GetToMessage(int id)
        {
            var userMessage = await context.Messages.Where(i => i.Id == id).Include(a => a.Sender).Select(s => new MessageModel()
            {
                Message = s.Text,
                SenderFirstname = s.Sender.FirstName,
                MessageUserAvatar = s.Sender.Avatar
            }).FirstOrDefaultAsync();

            var model = new ViewModel()
            {
                MessageModel = userMessage
            };

            var jsonModel = JsonConvert.SerializeObject(model);
            return Json(jsonModel);
        }


        [Area("Users")]
        [Route("/mesaj-gönder/{username}")]

        public async Task<IActionResult> messagedetail(string username)
        {

            var user = await _userManager.GetUserAsync(User);
            
            var users = context.Users.Where(i => i.Id != user.Id).Select(a => new UserModel()
            {

                AvatarUrl = a.Avatar,
                FirstName = a.FirstName,
                PersonelInfo = a.Personelİnfo,
                LastName = a.LastName,
                Id = a.Id,

            }).Take(5).ToList();

            var selectedUser = await context.Users.Where(a => a.UserName == username).Select(i => new SelectUserModel()
            {
                Avatar = i.Avatar,
                ReceiverId = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                PersonelInfo = i.Personelİnfo,

            }).FirstOrDefaultAsync();


            var model = new ViewModel()
            {
                Users = users,
                User = user,
                SelectedUser = selectedUser
            };
            return View(model);
        }


        [Area("Users")]
        [HttpPost]
        public async Task<IActionResult> SendMessage(ViewModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            if(ModelState.IsValid)
            {
                var message = new BlogProject.Context.Message()
                {
                    SenderId = user.Id,
                    ReceiverId = model.BlogDetails.Id,
                    Text = model.Message,
                    Date = DateTime.Now,
                    IsDeleted = false
                };
                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();
                return RedirectToAction("Home", "Users", new { area ="Users"});

            }
            return View(model);

        }

        [Area("Users")]
        [HttpPost]
        public async Task<IActionResult> SendAnswer(ViewModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var message = new BlogProject.Context.Message()
                {
                    SenderId = user.Id,
                    ReceiverId = model.Id,
                    Text = model.Message,
                    Date = DateTime.Now,
                    IsDeleted = false
                };
                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();
                return RedirectToAction("Home", "Users", new { area = "Users" });

            }
            return View(model);

        }

        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var deletedMessage = await context.Messages.Where(i => i.Id == id).FirstOrDefaultAsync();
            if(deletedMessage != null)
            {
                deletedMessage.IsDeleted = true;
                context.SaveChanges();
            }
            return Ok();
        }


        [Area("Users")]
        [HttpGet]
        public async Task<IActionResult> GetToMessageById(int id)
        {
            var message = await context.Messages.Where(i => i.Id == id && i.IsDeleted == false).Include(a => a.Sender).Select(o => new SenderUserModel() { 
            
                SenderFirstName = o.Sender.FirstName,
                SenderAvatar = o.Sender.Avatar,
                SenderMessage = o.Text,
                Date = o.Date
            
            }).FirstOrDefaultAsync();
            var jsonMessage = JsonConvert.SerializeObject(message);

            return Json(jsonMessage);

           
        }



        [Area("Users")]
        [HttpPost]
        public async Task<IActionResult> GetToComments(Comment model)
        {
            var user = await _userManager.GetUserAsync(User);
            var comment = new Comment()
            {
                UserId = user.Id,
                Email = user.Email,
                Message = model.Message,
                BlogId = model.BlogId,
                Date = DateTime.Now
            };
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();

            var JsonComment = JsonConvert.SerializeObject(model);

            return Json(JsonComment);

        }


        [Area("Users")]
        public IActionResult a()
        {

            var bilgi = context.Categories.Include(s => s.Blogs).ToList();
            var m = new ViewModel()
            {
                cat = bilgi
            };
            return View(m);
        }
    }

}









