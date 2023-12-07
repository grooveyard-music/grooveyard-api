using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Grooveyard.Domain.Models.Media;

namespace Grooveyard.Infrastructure.Data.Configurations
{
    public class MediaConfiguration
    {
        public void ConfigureSong(EntityTypeBuilder<Song> builder)
        {
            builder.HasOne(s => s.MusicFile)
                   .WithOne(mf => mf.Song)
                   .HasForeignKey<Song>(s => s.MusicFileId);

            builder.Property(s => s.UserId).IsRequired();

            builder.HasMany(s => s.Genres)
                   .WithMany(g => g.Songs)
                   .UsingEntity(j => j.ToTable("SongGenre"));
            // Additional configurations for Song...
        }

        public void ConfigureMix(EntityTypeBuilder<Mix> builder)
        {
            builder.HasOne(m => m.MusicFile)
                   .WithOne(mf => mf.Mix)
                   .HasForeignKey<Mix>(m => m.MusicFileId);

            builder.Property(m => m.UserId).IsRequired();

            builder.HasOne(m => m.Tracklist)
                   .WithOne(t => t.Mix)
                   .HasForeignKey<Tracklist>(t => t.MixId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Genres)
                   .WithMany(g => g.Mixes)
                   .UsingEntity(j => j.ToTable("MixGenre"));
     
        }

        public void ConfigureTracklistSong(EntityTypeBuilder<TracklistSong> builder)
        {
            builder.HasKey(ts => new { ts.TracklistId, ts.SongId });

            builder.HasOne(ts => ts.Tracklist)
                   .WithMany(t => t.TracklistSongs)
                   .HasForeignKey(ts => ts.TracklistId);

            builder.HasOne(ts => ts.Song)
                   .WithMany(s => s.TracklistSongs)
                   .HasForeignKey(ts => ts.SongId)
                   .OnDelete(DeleteBehavior.Cascade);
        
        }

        public void ConfigureMusicboxTrack(EntityTypeBuilder<MusicboxTrack> builder)
        {
            builder.HasKey(sbs => new { sbs.MusicboxId, sbs.TrackId });

            builder.HasOne(sbs => sbs.Musicbox)
                   .WithMany(sb => sb.MusicboxTracks)
                   .HasForeignKey(sbs => sbs.MusicboxId);

            builder.HasOne(sbs => sbs.Track)
                   .WithMany(t => t.MusicboxTracks)
                   .HasForeignKey(sbs => sbs.TrackId);
         
        }

        public void ConfigureTrack(EntityTypeBuilder<Track> builder)
        {
            builder.HasOne(t => t.Song)
                   .WithOne()
                   .HasForeignKey<Track>(t => t.MediaId)
                   .HasPrincipalKey<Song>(s => s.Id)
                   .IsRequired(false); // Optional relationship with Song

            builder.HasOne(t => t.Mix)
                   .WithOne()
                   .HasForeignKey<Track>(t => t.MediaId)
                   .HasPrincipalKey<Mix>(m => m.Id)
                   .IsRequired(false); // Optional relationship with Mix
        
        }
    }
}
