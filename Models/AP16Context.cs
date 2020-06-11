using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskList.Models
{
    public partial class AP16Context : DbContext
    {
        public AP16Context()
        {
        }

        public AP16Context(DbContextOptions<AP16Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskAttachment> TaskAttachment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(local);Database=AP16;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.FileExtension).HasMaxLength(50);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedPerson)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.AssignedPersonId)
                    .HasConstraintName("FK_Task_Person");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Task_Status");
            });

            modelBuilder.Entity<TaskAttachment>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.AttachmentId });

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.TaskAttachment)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskAttachment_Attachment");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskAttachment)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskAttachment_Task");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
