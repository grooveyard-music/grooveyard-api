using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Grooveyard.Domain.Entities;

namespace Grooveyard.Infrastructure.Data.Configurations
{
    public class SocialConfiguration
    {
        public void ConfigureDiscussion(EntityTypeBuilder<Discussion> builder)
        {
            builder.Property(d => d.UserId).IsRequired();
            builder.HasMany(m => m.Genres)
                   .WithMany(g => g.Discussions)
                   .UsingEntity(j => j.ToTable("DiscussionGenre"));
        }

        public void ConfigurePost(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.UserId).IsRequired();
            builder.HasOne(p => p.Discussion)
                   .WithMany(d => d.Posts)
                   .HasForeignKey(p => p.DiscussionId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(p => p.TrackId).IsRequired(false);
        }

        public void ConfigureComment(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.UserId).IsRequired();
            builder.HasOne(c => c.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(c => c.PostId)
                   .OnDelete(DeleteBehavior.NoAction);
        }

        public void ConfigureLike(EntityTypeBuilder<Like> builder)
        {
            builder.HasOne(l => l.Post)
                   .WithMany(p => p.Likes)
                   .HasForeignKey(l => l.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(l => l.Comment)
                   .WithMany(c => c.Likes)
                   .HasForeignKey(l => l.CommentId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.Property(l => l.UserId).IsRequired();
        }

        public void ConfigureSubscription(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => new { s.UserId, s.DiscussionId });
            builder.Property(s => s.UserId).IsRequired();
            builder.HasOne(s => s.Discussion)
                   .WithMany(d => d.Subscriptions)
                   .HasForeignKey(s => s.DiscussionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
