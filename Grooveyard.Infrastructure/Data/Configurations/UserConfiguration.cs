using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Grooveyard.Domain.Models.User;

namespace Grooveyard.Infrastructure.Data.Configurations
{
    public class UserConfiguration
    {
        public void ConfigureUserProfile(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(u => u.User)
                   .WithOne()
                   .HasForeignKey<UserProfile>(u => u.UserId);
            // Additional configurations for UserProfile...
        }

        public void ConfigureRefreshToken(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(rt => rt.User)
                   .WithMany()
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Additional configurations for RefreshToken...
        }

        public void ConfigureFriendship(EntityTypeBuilder<Friendship> builder)
        {
            builder.HasKey(f => new { f.UserId, f.FriendId });
            builder.HasIndex(f => new { f.UserId, f.FriendId }).IsUnique();
            builder.HasOne(f => f.User)
                   .WithMany(u => u.InitiatedFriendships)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(f => f.Friend)
                   .WithMany(u => u.ReceivedFriendships)
                   .HasForeignKey(f => f.FriendId)
                   .OnDelete(DeleteBehavior.NoAction);
            // Additional configurations for Friendship...
        }
    }
}
