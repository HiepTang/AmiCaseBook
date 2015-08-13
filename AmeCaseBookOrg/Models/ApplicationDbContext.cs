using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace AmeCaseBookOrg.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Country> Countries { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<File> Files { get; set; }

        public DbSet<DataItem> DataItems { get; set; }

        public DbSet<Comment> Comments { get; set; }


        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<CommunityTopic> CommunityTopics { get; set; }

        public DbSet<CommuityTopicComment> CommuityTopicComments { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(c => c.CanAccessCategories).WithMany(i => i.GrantForUsers)
                .Map(t => t.MapLeftKey("Id")
                    .MapRightKey("Code")
                    .ToTable("MemberPermission"));

            modelBuilder.Entity<Announcement>()
                .HasMany(c => c.AttachmentFiles).WithMany(i => i.AttachedToAnnouncements)
                .Map(t => t.MapLeftKey("ID")
                    .MapRightKey("FileId")
                    .ToTable("AnnouncementAttachment"));

            modelBuilder.Entity<CommunityTopic>()
                .HasMany(c => c.AttachmentFiles).WithMany(i => i.AttachedToCommunityTopics)
                .Map(t => t.MapLeftKey("ID")
                    .MapRightKey("FileId")
                    .ToTable("CommunityTopicAttachment"));

            base.OnModelCreating(modelBuilder);
        }
    }
}