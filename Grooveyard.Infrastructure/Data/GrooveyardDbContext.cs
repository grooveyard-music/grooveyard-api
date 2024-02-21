

using Grooveyard.Domain.Entities;
using Grooveyard.Domain.Events;
using Grooveyard.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Grooveyard.Infrastructure.Data
{
    public class GrooveyardDbContext : IdentityDbContext
    {
        public GrooveyardDbContext(DbContextOptions<GrooveyardDbContext> options)
            : base(options)
        {
        }

        #region User
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        #endregion

        #region Media
        public DbSet<MusicFile> MusicFiles { get; set; }
        public DbSet<Musicbox> MusicBoxes { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<TracklistTrack> TracklistTracks { get; set; }
        public DbSet<MusicboxTrack> MusicboxTracks { get; set; }

        public DbSet<Mix> Mixes { get; set; }
        public DbSet<Tracklist> Tracklists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }
        #endregion

        #region Social
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        #endregion


        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<DomainEvent>();

            var socialConfig = new SocialConfiguration();
            modelBuilder.Entity<Discussion>(socialConfig.ConfigureDiscussion);
            modelBuilder.Entity<Post>(socialConfig.ConfigurePost);
            modelBuilder.Entity<Comment>(socialConfig.ConfigureComment);
            modelBuilder.Entity<Like>(socialConfig.ConfigureLike);
            modelBuilder.Entity<Subscription>(socialConfig.ConfigureSubscription);

            var mediaConfig = new MediaConfiguration();
            modelBuilder.Entity<Song>(mediaConfig.ConfigureSong);
            modelBuilder.Entity<Mix>(mediaConfig.ConfigureMix);
            modelBuilder.Entity<TracklistTrack>(mediaConfig.ConfigureTracklistTrack);
            modelBuilder.Entity<MusicboxTrack>(mediaConfig.ConfigureMusicboxTrack);
            modelBuilder.Entity<Track>(mediaConfig.ConfigureTrack);

            var userConfig = new UserConfiguration();
            modelBuilder.Entity<UserProfile>(userConfig.ConfigureUserProfile);
            modelBuilder.Entity<RefreshToken>(userConfig.ConfigureRefreshToken);
            modelBuilder.Entity<Friendship>(userConfig.ConfigureFriendship);

        }
    }
}
