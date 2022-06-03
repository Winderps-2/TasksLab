using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TasksLab.Models
{
    public partial class TasksLabContext : DbContext
    {
        public TasksLabContext()
        {
        }

        public TasksLabContext(DbContextOptions<TasksLabContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TaskStatus> TaskStatus { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
                var connectionString = new StreamReader(File.Open("secret.txt", FileMode.Open)).ReadToEnd();
                optionsBuilder.UseMySQL(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PRIMARY");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.StatusColorHex)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsFixedLength()
                    .HasDefaultValueSql("'''#FFFFFF'''");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.TaskStatus)
                    .HasName("Tasks_FK");

                entity.Property(e => e.TaskId)
                    .HasColumnName("TaskID")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.TaskDescription)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.TaskStatus).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.TaskStatusNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Tasks_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
