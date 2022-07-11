using BlogProject.Areas.Identity;
using BlogProject.EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Buffers;


namespace BlogProject
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            services.AddControllersWithViews();
            services.AddControllers();
            services.AddSignalR();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins();

            }));
            services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

            services.AddDbContext<ApplicationContext>();
           
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
           
            services.Configure<IdentityOptions>(options =>
            {
                // password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                // Lockout                
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
           
            
            services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = new PathString("/account/login");
                x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                x.SlidingExpiration = true;
                x.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "Blog.Cookie",
                    SameSite = SameSiteMode.Strict

                };
            });


            services.AddAuthentication().AddFacebook(x =>
            {
                x.AppId = Configuration["FacebookAppId"];
                x.AppSecret = Configuration["FacebookAppSecret"];
                x.CallbackPath = new PathString("/User/Hata");
            });


            services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = "771294619569-7ptjvph5km3uiiufs413htr82vv77mq7.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });


            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                 new SmtpEmailSender(
                     Configuration["EmailSender:Host"],
                     Configuration.GetValue<int>("EmailSender:Port"),
                     Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                     Configuration["EmailSender:UserName"],
                     Configuration["EmailSender:Password"])
                );

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
          
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                   name: "MyAreas",
                   pattern: "{area:exists}/users/users/a/{name}",
                   defaults: new { controller = "Users", action = "a" });

                endpoints.MapControllerRoute(
                    name: "MyAreas",
                    pattern: "{area:exists}/users/users/messagedetail/{username}",
                    defaults: new { controller = "Users", action = "messagedetail" });

                endpoints.MapControllerRoute(
                     name: "MyAreas",
                     pattern: "{area:exists}/kullanici/hesap-ayarlari/{username}",
                     defaults: new { controller = "Users", action = "Editprofile" });

                endpoints.MapControllerRoute(
                     name: "MyAreas",
                     pattern: "{area:exists}/Users/{title}",
                     defaults: new { controller = "Users", action = "UserBlogDetail" });

                endpoints.MapControllerRoute(
                     name: "MyAreas",
                     pattern: "{area:exists}/{username}",
                     defaults: new { controller = "Users", action = "UserProfile" });

                endpoints.MapControllerRoute(
                      name: "MyArea",
                      pattern: "{area:exists}/{controller=User}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                      name: "MyArea",
                      pattern: "{area:exists}/{controller=Users}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                       name: "MyArea",
                       pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
            
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });



        }
    }
}
