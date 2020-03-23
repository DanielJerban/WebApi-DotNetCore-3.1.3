using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanielApi.Models
{
    public class UserDto : IValidatableObject
    {
        [Required (ErrorMessage = "فیلد نام کاربری الزامی می باشد.")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required (ErrorMessage = "فیلد رمز عبور الزامی می باشد.")]
        [StringLength(500)]
        public string Password { get; set; }

        [Required (ErrorMessage = "فیلد نام و نام خانوادگی الزامی می باشد.")]
        [StringLength(100)]
        public string FullName { get; set; }

        public int Age { get; set; }

        public GenderType Gender { get; set; }

        /// <summary>
        /// Business concept validation 
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // yield return => works kind of like chunk 
            // and if all if conditions are true it will return all three messages 

            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("نام کاربری نمیتواند Test باشد", new[] { nameof(UserName) });
            if (Password.Equals("123456"))
                yield return new ValidationResult("رمز عبور نمیتواند 123456 باشد", new[] { nameof(Password) });
            if (Gender == GenderType.Male && Age > 30)
                yield return new ValidationResult("آقایان بیشتر از 30 سال معتبر نیستند", new[] { nameof(Gender), nameof(Age) });
        }
    }
}
