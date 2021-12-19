using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ScriptBuddy.Models
{
    public partial class ScriptBuddyDBContext : DbContext
    {
        public ScriptBuddyDBContext()
        {
        }

        public ScriptBuddyDBContext(DbContextOptions<ScriptBuddyDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<CharacterSequenceProperty> CharacterSequenceProperties { get; set; }
        public virtual DbSet<HotStringProperty> HotStringProperties { get; set; }
        public virtual DbSet<KeyListenerProperty> KeyListenerProperties { get; set; }
        public virtual DbSet<KeyPressProperty> KeyPressProperties { get; set; }
        public virtual DbSet<MediaKeyProperty> MediaKeyProperties { get; set; }
        public virtual DbSet<MouseClickProperty> MouseClickProperties { get; set; }
        public virtual DbSet<MouseMoveProperty> MouseMoveProperties { get; set; }
        public virtual DbSet<PauseProperty> PauseProperties { get; set; }
        public virtual DbSet<Script> Scripts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=ScriptBuddyDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Action>(entity =>
            {
                entity.ToTable("Action");

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.ScriptId)
                    .HasConstraintName("FK_Action_Script");
            });

            modelBuilder.Entity<CharacterSequenceProperty>(entity =>
            {
                entity.ToTable("CharacterSequenceProperty");

                entity.Property(e => e.CharacterSequence)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.CharacterSequenceProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_CharacterSequenceProperty_ToTable");
            });

            modelBuilder.Entity<HotStringProperty>(entity =>
            {
                entity.ToTable("HotStringProperty");

                entity.Property(e => e.InputString)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OutputString)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.HotStringProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_HotStringProperty_ToTable");
            });

            modelBuilder.Entity<KeyListenerProperty>(entity =>
            {
                entity.ToTable("KeyListenerProperty");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.KeyListenerProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_KeyListenerProperty_ToTable");
            });

            modelBuilder.Entity<KeyPressProperty>(entity =>
            {
                entity.ToTable("KeyPressProperty");

                entity.Property(e => e.KeyPressed)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PressType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.KeyPressProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_KeyPressProperty_Action");
            });

            modelBuilder.Entity<MediaKeyProperty>(entity =>
            {
                entity.ToTable("MediaKeyProperty");

                entity.Property(e => e.MediaKey)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.MediaKeyProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_MediaKeyProperty_Action");
            });

            modelBuilder.Entity<MouseClickProperty>(entity =>
            {
                entity.ToTable("MouseClickProperty");

                entity.Property(e => e.Button)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ClickType)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.MouseClickProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_MouseClickProperty_Action");
            });

            modelBuilder.Entity<MouseMoveProperty>(entity =>
            {
                entity.ToTable("MouseMoveProperty");

                entity.Property(e => e.Xposition).HasColumnName("XPosition");

                entity.Property(e => e.Yposition).HasColumnName("YPosition");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.MouseMoveProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_MouseMoveProperty_Action");
            });

            modelBuilder.Entity<PauseProperty>(entity =>
            {
                entity.ToTable("PauseProperty");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.PauseProperties)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_PauseProperty_Action");
            });

            modelBuilder.Entity<Script>(entity =>
            {
                entity.ToTable("Script");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TimeLastSaved).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
