using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Role : IdentityRole<int>, IEntity//BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
        }
    }
}
