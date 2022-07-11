using BlogProject.Areas.Identity;
using BlogProject.Context;
using BlogProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace BlogProject.Areas.Users.Model
{


    [Authorize]
    public class ViewModel
    {

        public List<Notification> Notif { get; set; }
        public List<Category> cat { get; set; }

        public List<RegisterEditViewModel> UserListForMessage { get; set; }
        public RegisterEditViewModel EditProfile { get; set; }

        public List<CategoryModel> Category { get; set; }
        public ICollection<UserModel> Users { get; set; }
        public UserModel BlogDetails { get; set; }
        public List<ListModel> myList { get; set; }
        public ICollection<SelectModel> LastBlogs { get; set; }
        public ICollection<ViewModel> Categories { get; set; }
        public List<UserMessageModel> MessagesForReceiver { get; set; }
        public UserModel RegisterEditUser { get; set; }
        public ICollection<CategoryModel> Tags { get; set; }
        public ICollection<Follow> AllFollowedUser { get; set; }
        public ICollection<Follow> AllFollowerUser { get; set; }
        public ICollection<SelectBlogModel> UsersBlog { get; set; }
        public ICollection<SelectBlogModel> Ublog { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<System.Linq.IGrouping<string, Blog>> Blog { get; set; }
        public ICollection<ViewModel> UserLikes { get; set; }
        public ICollection<Likes> BlogLikes { get; set; }
        public ICollection<User> UserBlog { get; set; }
        public ICollection<BlogViewModel> UserBlogDetail { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public List<IGrouping<ICollection<BlogProject.Context.Message>, User>> Messages { get; set; }
        public ICollection<NotificationModel> UserNotification { get; set; }
        public MessageModel MessageModel { get; set; }
        public List<FollowModel> FollowModel { get; set; }
        //public List<UserModel> SenderUser { get; set; }
        public List<BlogProject.Context.Message> MessageSent { get; set; }
        public User User { get; set; }
        public User UserMessages { get; set; }
        public SelectUserModel SelectedUser { get; set; }
        public Comment comment { get; set; }
        public List<UserModel> SenderUser { get; set; }

        public int CountLike { get; set; }
        public int BlogCount { get; set; }
        ///
        public string UserInfo { get; set; }
        public IFormFile Image { get; set; }
        public int FollowedCount { get; set; }
        public int FollowerCount { get; set; }
        public string Name { get; set; }
        public User LoginUserImage { get; set; }

        //userupdate
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string PersonelInfo { get; set; }
        public string Location { get; set; }
        public string WebSite { get; set; }
        public string ReceiverId { get; set; }

        [DataType(DataType.Upload)]
        public string AvatarUrl { get; set; }
        
        [DataType(DataType.Upload)]
        public IFormFile AvatarUpload { get; set; }
        
        ///
        public List<UserModel> UsersModel { get; set; }
        public List<SelectModel> BlogSelectModel { get; set; }
        public List<UserModel> Following { get; set; }
        public List<User> IsFollowing { get; set; }
        public List<User> Follow { get; set; }
        public List<Follow> Followed { get; set; }
        public List<Follow> Follower { get; set; }

        ///
        public int BlogId { get; set; }
        public int LikeId { get; set; }
        public string BlogsTitle { get; set; }
        public string BlogsContent { get; set; }
        public string BlogImage { get; set; }
        public string UserAvatar { get; set; }
        public string BlogUserFirstname { get; set; }
        public string BlogUserLastname { get; set; }
        ///
        public IEnumerable SenderAvatar { get; set; }
        public string SenderId { get; set; }
        public string SenderFirstName { get; set; }
        public IEnumerable SenderUserName { get; set; }
        public string SenderMessage { get; set; }

        //[Required]
        public string Comment { get; set; }
        public string Text { get; set; }

        //
        public string Subject { get; set; }
        
        [Required(ErrorMessage ="Eksik alanları kontrol edin")]
        public string Message { get; set; }
        public string SenderMessageId { get; set; }
    }



}
