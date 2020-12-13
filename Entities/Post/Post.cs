using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Post : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

        public Category Category { get; set; }
        public User Author { get; set; }
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Description).IsRequired();
            builder.HasOne(a => a.Category).WithMany(a => a.Posts).HasForeignKey(a => a.CategoryId);
            builder.HasOne(p => p.Author).WithMany(p => p.Posts).HasForeignKey(p => p.AuthorId);
        }
    }
}
