using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            IsActive = true;
        }

        [Required] [StringLength(500)] public string UserName { get; set; }

        [Required] [StringLength(500)] public string PasswordHash { get; set; }

        [Required] [StringLength(500)] public string FullName { get; set; }

        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }

    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male,

        [Display(Name = "زن")]
        Female
    }
}