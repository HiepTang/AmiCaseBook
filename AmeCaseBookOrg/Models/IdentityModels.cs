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
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                   
                return LastName + ", " + FirstName;
            }
        }
        
        [Display(Name ="Affiliation")]
        public string Affiliation { get; set; }

        [Display(Name = "Introduction")]
        public string Introduction { get; set; }

        [Display(Name = "Address")]
        [StringLength(1000)]
        public string Address { get; set; }

        [Display(Name = "Link In")]
        public string LinkIn { get; set; }


        public int? FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual File UploadImage { get; set; }

        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual SubCategory Country { get; set; }

        public virtual ICollection<SubMenu> CanAccessCategories { get; set; }
        
        [InverseProperty("CreatedUser")]
        public  virtual ICollection<DataItem> DataItems { get; set; }

        [InverseProperty("Author")]
        public virtual ICollection<Announcement> Announcements { get; set; }
    }

    
}