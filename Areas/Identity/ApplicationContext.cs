using BlogProject.Context;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Areas.Identity
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext()
        {
            
        }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-5EJQMOF;Database=BlogApp;Uid=bö;Pwd=123456;Trusted_Connection=True;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            modelBuilder.Entity<Follow>()                                           
                .HasKey(k => new { k.FollowerId, k.FolloweeId });

            modelBuilder.Entity<Follow>()                                            
                .HasOne(u => u.Followee)
                .WithMany(u => u.Follower)
                .HasForeignKey(u => u.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()                                            
                .HasOne(u => u.Follower)
                .WithMany(u => u.Followee)
                .HasForeignKey(u => u.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<BlogProject.Context.Message>()
                .HasOne(u => u.Receiver)
                .WithMany(u => u.Sender)
                .HasForeignKey(u => u.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BlogProject.Context.Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.Receiver)
                .HasForeignKey(u => u.SenderId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Notification>()
               .HasOne(u => u.Actor)
               .WithMany(u => u.Actor)
               .HasForeignKey(u => u.ActorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
               .HasOne(u => u.Notifier)
               .WithMany(u => u.Notifier)
               .HasForeignKey(u => u.NotifierId)
               .OnDelete(DeleteBehavior.Restrict);

        }


        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Context.Message> Messages { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<Notification> Notification { get; set; }
        
    }
}
