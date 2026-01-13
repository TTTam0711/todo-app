using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Persistence.Context;

public partial class TodoAppDbContext : DbContext
{
    public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<Subtask> Subtasks { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TodoList> TodoLists { get; set; }

    public virtual DbSet<TodoTask> TodoTasks { get; set; }

    public virtual DbSet<v_OpenTask> v_OpenTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.ActivityId);

            entity.ToTable("ActivityLog");

            entity.HasIndex(e => new { e.ResourceType, e.ResourceId, e.CreatedAt }, "IX_Activity_Res").IsDescending(false, false, true);

            entity.Property(e => e.Action)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ResourceType)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.ActorUser).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.ActorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activity_Actor");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("AppUser");

            entity.HasIndex(e => e.Email, "UQ_AppUser_Email").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(120);
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TimeZone).HasMaxLength(64);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachment");

            entity.HasIndex(e => new { e.TaskId, e.CreatedAt }, "IX_Attachment_Task").IsDescending(false, true);

            entity.Property(e => e.AttachmentId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.FileName).HasMaxLength(260);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StorageUrl).HasMaxLength(1024);

            entity.HasOne(d => d.Task).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_Attachment_Task");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.HasIndex(e => new { e.TaskId, e.CreatedAt }, "IX_Comment_Task").IsDescending(false, true);

            entity.Property(e => e.CommentId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Task).WithMany(p => p.Comments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_Comment_Task");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.HasIndex(e => e.UserId, "IX_Notification_User_Unread").HasFilter("([IsRead]=(0))");

            entity.Property(e => e.NotificationId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ReadAt).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Type)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.ExpiresAt, "IX_RefreshToken_ExpiresAt");

            entity.HasIndex(e => new { e.UserId, e.TokenHash }, "UX_RefreshToken_User_TokenHash").IsUnique();

            entity.Property(e => e.RefreshTokenId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DeviceInfo).HasMaxLength(255);
            entity.Property(e => e.ExpiresAt).HasPrecision(3);
            entity.Property(e => e.ReplacedByTokenHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RevokedAt).HasPrecision(3);
            entity.Property(e => e.TokenHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_User");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.ToTable("Reminder");

            entity.HasIndex(e => e.RemindAt, "IX_Reminder_Due").HasFilter("([SentAt] IS NULL)");

            entity.Property(e => e.ReminderId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Channel)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValue("inapp");
            entity.Property(e => e.RemindAt).HasPrecision(0);
            entity.Property(e => e.SentAt).HasPrecision(0);

            entity.HasOne(d => d.Task).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_Reminder_Task");
        });

        modelBuilder.Entity<Share>(entity =>
        {
            entity.ToTable("Share");

            entity.HasIndex(e => new { e.UserId, e.ResourceType, e.CreatedAt }, "IX_Share_User").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.ResourceType, e.ResourceId, e.UserId }, "UQ_Share_Unique").IsUnique();

            entity.Property(e => e.ShareId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ResourceType)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Shares)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Share_User");
        });

        modelBuilder.Entity<Subtask>(entity =>
        {
            entity.ToTable("Subtask");

            entity.HasIndex(e => new { e.TaskId, e.OrderIndex }, "IX_Subtask_Task");

            entity.Property(e => e.SubtaskId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.OrderIndex)
                .HasDefaultValue(1000m)
                .HasColumnType("decimal(9, 4)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Task).WithMany(p => p.Subtasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_Subtask_Task");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.HasIndex(e => new { e.OwnerUserId, e.Name }, "UQ_Tag_Owner_Name").IsUnique();

            entity.Property(e => e.TagId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Color)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.Tags)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tag_Owner");
        });

        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(e => e.ListId);

            entity.ToTable("TodoList");

            entity.HasIndex(e => e.OwnerUserId, "IX_TodoList_Owner_Active").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.ListId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Color)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.TodoLists)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TodoList_Owner");
        });

        modelBuilder.Entity<TodoTask>(entity =>
        {
            entity.HasKey(e => e.TaskId);

            entity.ToTable("TodoTask");

            entity.HasIndex(e => e.DueAt, "IX_TodoTask_DueAt_Open").HasFilter("(([Status] IN ('todo', 'doing', 'blocked')) AND [IsDeleted]=(0))");

            entity.HasIndex(e => new { e.ListId, e.IsDeleted }, "IX_TodoTask_List_NotDeleted").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.Status, "IX_TodoTask_Status_Active").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.TaskId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CompletedAt).HasPrecision(0);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.DueAt).HasPrecision(0);
            entity.Property(e => e.OrderIndex)
                .HasDefaultValue(1000m)
                .HasColumnType("decimal(9, 4)");
            entity.Property(e => e.Priority).HasDefaultValue((byte)3);
            entity.Property(e => e.RecurrenceRule).HasMaxLength(200);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StartAt).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValue("todo");
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.TodoTaskCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TodoTask_CreatedBy");

            entity.HasOne(d => d.List).WithMany(p => p.TodoTasks)
                .HasForeignKey(d => d.ListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TodoTask_List");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.TodoTaskUpdatedByUsers)
                .HasForeignKey(d => d.UpdatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TodoTask_UpdatedBy");

            entity.HasMany(d => d.Tags).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_TaskTag_Tag"),
                    l => l.HasOne<TodoTask>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("FK_TaskTag_Task"),
                    j =>
                    {
                        j.HasKey("TaskId", "TagId");
                        j.ToTable("TaskTag");
                        j.HasIndex(new[] { "TagId" }, "IX_TaskTag_Tag");
                    });
        });

        modelBuilder.Entity<v_OpenTask>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_OpenTasks");

            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.DueAt).HasPrecision(0);
            entity.Property(e => e.ListName).HasMaxLength(200);
            entity.Property(e => e.OrderIndex).HasColumnType("decimal(9, 4)");
            entity.Property(e => e.StartAt).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
