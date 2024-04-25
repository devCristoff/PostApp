using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PostApp.Core.Application.Helpers;
using PostApp.Core.Application.ViewModels.Users;
using PostApp.Core.Domain.Common;
using PostApp.Core.Domain.Entities;

namespace PostApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAssesor) : base(options) { }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Fluent API
            #region Tables
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Friend>().ToTable("Friends");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Post>().HasKey(post => post.Id);
            modelBuilder.Entity<Comment>().HasKey(comment => comment.Id);
            modelBuilder.Entity<Friend>().HasKey(friend => new { friend.UserId, friend.FriendId }); // Composite Leys
            #endregion

            #region Relationships
            modelBuilder.Entity<Post>()
                .HasMany(post => post.Comments)
                .WithOne(comment => comment.Post)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region "Property configuration"

            #region Post
            modelBuilder.Entity<Post>()
                .Property(post => post.Message)
                .IsRequired()
                .HasMaxLength(500);
            #endregion

            #region Comment
            modelBuilder.Entity<Comment>()
                .Property(comment => comment.Content)
                .IsRequired()
                .HasMaxLength(100);
            #endregion

            #endregion
        }
    }
}
