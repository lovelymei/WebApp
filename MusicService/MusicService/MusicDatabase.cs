using System;
using Microsoft.EntityFrameworkCore;
using MusicService.Models;

#nullable disable

namespace MusicService
{
    public partial class MusicDatabase : DbContext
    {
        public MusicDatabase()
        {

        }
        public MusicDatabase(DbContextOptions<MusicDatabase> options)
            : base(options)
        {
            //при изменении  бд 
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public virtual DbSet<Performer> Performers { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<Listener> Listeners { get; set; }
        public virtual DbSet<Album> Albums { get; set; }


        internal object GetCollection<T>()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=WIN-C8HRCMG8G6A\\SQLEXPRESS;Database=MusicDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");


            modelBuilder.Entity<Performer>(entity =>
            {
                entity.ToTable("Performer");

                entity.Property(e => e.BirthDate);
         
            });

            modelBuilder.Entity<Performer>().HasKey(e => e.AccountId).HasName("PK_Performer_AccountId");

            modelBuilder.Entity<Listener>(entity =>
            {
                entity.ToTable("Listener");

                entity.Property(e => e.BirthDate);
             
            });

            modelBuilder.Entity<Listener>().HasKey(e=>e.AccountId).HasName("PK_Listener_AccountId");


            modelBuilder.Entity<Song>(entity =>
            {
                entity.ToTable("Song");

                entity.HasOne(e => e.Album)
                   .WithMany(e => e.Songs)
                   .HasForeignKey(e => e.AlbumId)
                   .HasConstraintName("Album/Songs");

                entity.HasOne(e => e.Performer)
                   .WithMany(e => e.Songs)
                   .HasForeignKey(e => e.AccountId)
                   .HasConstraintName("Performer/Songs");

                entity.HasMany(d => d.Listeners)
                    .WithMany(d => d.Songs)
                    .UsingEntity(j => j.ToTable("ListenerSongLibrary"));

            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("Album");

                entity.HasOne(e => e.Performer)
                    .WithMany(e => e.Albums)
                    .HasForeignKey(e => e.AccountId)
                    .HasConstraintName("Performer/Albums");

                entity.HasMany(d => d.Listeners)
                   .WithMany(d => d.Albums)
                   .UsingEntity(j => j.ToTable("ListenerAlbumLibrary"));
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
