using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Grooveyard.Domain.Entities;

namespace Grooveyard.Infrastructure.Data.Configurations
{
    public class MediaConfiguration
    {
        public void ConfigureSong(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Title).IsRequired();
            builder.Property(s => s.Artist).IsRequired();
            builder.Property(s => s.UserId).IsRequired();

            builder.HasOne(s => s.Track)
                   .WithMany(t => t.Songs)
                   .HasForeignKey(s => s.TrackId);


            builder.HasMany(s => s.Genres)
                   .WithMany(g => g.Songs)
                   .UsingEntity(j => j.ToTable("SongGenre"));
        }

        public void ConfigureMix(EntityTypeBuilder<Mix> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).IsRequired();
            builder.Property(m => m.Artist).IsRequired();
            builder.Property(m => m.UserId).IsRequired();

            builder.HasOne(m => m.Track)
                   .WithMany(t => t.Mixes)
                   .HasForeignKey(m => m.TrackId);


            builder.HasOne(m => m.Tracklist)
                   .WithOne(tl => tl.Mix)
                   .HasForeignKey<Mix>(m => m.TracklistId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Genres)
                   .WithMany(g => g.Mixes)
                   .UsingEntity(j => j.ToTable("MixGenre"));
        }

        public void ConfigureTrack(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.DateCreated).IsRequired();
        }

        public void ConfigureTracklist(EntityTypeBuilder<Tracklist> builder)
        {
            builder.HasKey(tl => tl.Id);

            builder.HasMany(tl => tl.TracklistTracks)
                   .WithOne(tlt => tlt.Tracklist)
                   .HasForeignKey(tlt => tlt.TracklistId);
        }

        public void ConfigureTracklistTrack(EntityTypeBuilder<TracklistTrack> builder)
        {
            builder.HasKey(tlt => new { tlt.TracklistId, tlt.TrackId });

            builder.HasOne(tlt => tlt.Tracklist)
                   .WithMany(tl => tl.TracklistTracks)
                   .HasForeignKey(tlt => tlt.TracklistId);

            builder.HasOne(tlt => tlt.Track)
                   .WithMany()
                   .HasForeignKey(tlt => tlt.TrackId);
        }

        public void ConfigureMusicboxTrack(EntityTypeBuilder<MusicboxTrack> builder)
        {
            builder.HasKey(mbt => new { mbt.MusicboxId, mbt.TrackId });

            builder.HasOne(mbt => mbt.Musicbox)
                   .WithMany(mb => mb.MusicboxTracks)
                   .HasForeignKey(mbt => mbt.MusicboxId);

            builder.HasOne(mbt => mbt.Track)
                   .WithMany(t => t.MusicboxTracks)
                   .HasForeignKey(mbt => mbt.TrackId);
        }

    }
}
