using BlogProject.Areas.Identity;
using BlogProject.Areas.Identity.Model;
using BlogProject.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlogProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _UserManager;
        private SignInManager<User> _singInManager;
        private IEmailSender _emailSender;

        public AccountController(UserManager<User> UserManager, SignInManager<User> singInManager, IEmailSender emailSender)
        {
            _UserManager = UserManager;
            _singInManager = singInManager;
            _emailSender = emailSender;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Login(string ReturnUrl)
        {
            

            return View(Tuple.Create<LoginModel, RegisterModel>(new LoginModel()
            {
                ExternalLogins = (await _singInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            },
            new RegisterModel()));
        }


        [HttpPost]
        public async Task<IActionResult> Login(/*LoginModel model*/ /*Tuple<LoginModel, RegisterModel> model*/ [Bind(Prefix = "Item1")] LoginModel model, string ReturnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            User user = await _UserManager.FindByEmailAsync(model.Email);
            

            if (user != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _singInManager.PasswordSignInAsync(user, model.Password, true, true);
                if (result.Succeeded)
                {
                    await AddUserIdClaim(user);
                    return Redirect(ReturnUrl != null ? ReturnUrl : Url.Action("Index", "Users"));

                }
                else
                    ModelState.AddModelError("Hatalı Giriş", "Yaptığınız giriş talebi geçersizdir.");
            }
            else
                ModelState.AddModelError("Olmayan Kullanıcı", "Kullanıcı adı veya şifre yanlış.");
            return View();

        }


        private async Task AddUserIdClaim(IdentityUser user)
        {
            var userClaims = await _UserManager.GetClaimsAsync((User)user);
            if (!userClaims.Any(x => x.Type == "UserId"))
            {
                var claim = new Claim("UserId", user.Id);
                await _UserManager.AddClaimAsync((User)user, claim);
            }
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( [Bind(Prefix = "Item2")] RegisterModel model, IFormFile Avatar, string firstname, string lastname, string username, string email, string password, string title)
        {
            
           
                Random rastgele = new Random();
                int rndImamge = rastgele.Next();
                var extension = Path.GetExtension(Avatar.FileName);
                var newİmageName = "Image-" + rndImamge.ToString() + extension;
                var location = Path.Combine("wwwroot/registerimage/", newİmageName);
                var stream = new FileStream(location, FileMode.Create);

                Avatar.CopyTo(stream);
            

            var user = new User()
            {
                FirstName = firstname,
                LastName = lastname,
                UserName = username,
                Email = email,
                Title = title,
                Date = DateTime.Now,
                Avatar = "registerimage/" + newİmageName
            };

            var result = await _UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var code = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                await _emailSender.SendEmailAsync(email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:44338{url}'>tıklayınız.</a>");
                return RedirectToAction("Login", "Account");

            }
            ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
            return View(model);
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData["message"] = "Geçersiz token";
                return View();
            }

            var user = await _UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _UserManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    TempData["message"] = "Hesabınız onaylandı.";
                    return View();

                }

            }

            TempData["message"] = "Hesabınız onaylanmadı.";
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return Redirect("~/");
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                    new { ReturnUrl = returnUrl });

            var properties = _singInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties); 
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Users/Users/Index");

            LoginModel loginViewModel = new LoginModel
            {

                ReturnUrl = returnUrl,
                ExternalLogins = (await _singInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the ex
            // ternal login provider
            var info = await _singInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _singInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _UserManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _UserManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _UserManager.AddLoginAsync(user, info);
                    await _singInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

                return View("Error");
            }
        }
    }
}
