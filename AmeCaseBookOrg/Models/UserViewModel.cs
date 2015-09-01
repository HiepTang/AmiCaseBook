using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AmeCaseBookOrg.Models
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tel")]
        public string PhoneNumber { get; set; }

        public int CountryId { get; set; }

        public File UploadImage { get; set; }

        [Display(Name = "Affiliation")]
        public string Affiliation { get; set; }

        [Display(Name = "Introduction")]
        public string Introduction { get; set; }
        [Display (Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Link In")]
        public string LinkIn { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin
        {
            get; set;
        }
    }
}