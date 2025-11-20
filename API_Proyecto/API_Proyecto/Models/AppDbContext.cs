using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Proyecto.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<EventTask> EventTasks { get; set; }
    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }

    // ❌ Se elimina TaskDependency como DbSet para evitar duplicación de tablas
    // public virtual DbSet<TaskDependency> TaskDependencies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=SupabaseConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("event_pkey");
            entity.ToTable("event");
            entity.HasIndex(e => e.Title, "event_title_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EndedAt).HasColumnName("ended_at");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("started_at");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");

            entity.HasOne(d => d.Project).WithMany(p => p.Events)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_project_id_fkey");
        });

        modelBuilder.Entity<EventTask>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.TaskId }).HasName("event_tasks_pkey");
            entity.ToTable("event_tasks");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.TaskOrder)
                .HasDefaultValue(0)
                .HasColumnName("task_order");

            entity.HasOne(d => d.Event).WithMany(p => p.EventTasks)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("event_tasks_event_id_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.EventTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("event_tasks_task_id_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");
            entity.ToTable("person");
            entity.HasIndex(e => e.Email, "person_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_pkey");
            entity.ToTable("project");
            entity.HasIndex(e => e.Name, "project_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.EndedAt).HasColumnName("ended_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("started_at");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.PersonId }).HasName("project_members_pkey");
            entity.ToTable("project_members");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'Member'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("role");

            entity.HasOne(d => d.Person).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("project_members_person_id_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("project_members_project_id_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");
            entity.ToTable("task");
            entity.HasIndex(e => e.Title, "task_title_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Priority)
                .HasColumnType("character varying")
                .HasColumnName("priority");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("task_project_id_fkey");

            // Configuración del M:M con task_dependencies
            entity.HasMany(d => d.DependentTasks).WithMany(p => p.PrerequisiteTasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskDependency",
                    r => r.HasOne<Task>().WithMany()
                        .HasForeignKey("DependentTaskId")
                        .HasConstraintName("task_dependencies_dependent_task_id_fkey"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("PrerequisiteTaskId")
                        .HasConstraintName("task_dependencies_prerequisite_task_id_fkey"),
                    j =>
                    {
                        j.HasKey("PrerequisiteTaskId", "DependentTaskId").HasName("task_dependencies_pkey");
                        j.ToTable("task_dependencies");
                        j.IndexerProperty<Guid>("PrerequisiteTaskId").HasColumnName("prerequisite_task_id");
                        j.IndexerProperty<Guid>("DependentTaskId").HasColumnName("dependent_task_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
