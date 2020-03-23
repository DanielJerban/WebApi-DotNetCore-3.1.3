using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(c => c.User).WithMany(c => c.UserRoles).HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.Role).WithMany(c => c.UserRoles).HasForeignKey(c => c.RoleId);
        }
    }
}
