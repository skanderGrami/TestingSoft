using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestingSoft_Backend.Models;

public partial class TestingSoftContext : DbContext
{
   

    public TestingSoftContext(DbContextOptions<TestingSoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Build> Builds { get; set; }

    public virtual DbSet<ExceptionDb> ExceptionDbs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Scenario> Scenarios { get; set; }

    public virtual DbSet<TestCase> TestCases { get; set; }

    public virtual DbSet<TestReport> TestReports { get; set; }

    public virtual DbSet<TestSuite> TestSuites { get; set; }

    public virtual DbSet<TestType> TestTypes { get; set; }

    public virtual DbSet<TypeValue> TypeValues { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Build>(entity =>
        {
            entity.ToTable("Build");

            entity.Property(e => e.BuildId).HasColumnName("Build_Id");
            entity.Property(e => e.BuildNum).HasColumnName("Build_Num");
            entity.Property(e => e.BuildStatus).HasColumnName("Build_Status");
            entity.Property(e => e.BuildUrl)
                .HasColumnType("date")
                .HasColumnName("Build_Url");
            entity.Property(e => e.TestCaseId).HasColumnName("TestCase_Id");

            entity.HasOne(d => d.TestCase).WithMany(p => p.Builds)
                .HasForeignKey(d => d.TestCaseId)
                .HasConstraintName("FK_Build_TestCase");
        });

        modelBuilder.Entity<ExceptionDb>(entity =>
        {
            entity.ToTable("ExceptionDb");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.Error).HasColumnName("error");
            entity.Property(e => e.Fonction).HasColumnName("fonction");
            entity.Property(e => e.Repository).HasColumnName("repository");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.RoleName).HasColumnName("Role_Name");
        });

        modelBuilder.Entity<Scenario>(entity =>
        {
            entity.ToTable("Scenario");

            entity.Property(e => e.ScenarioId).HasColumnName("Scenario_Id");
            entity.Property(e => e.IdTypeValue).HasColumnName("Id_TypeValue");
            entity.Property(e => e.TagValue).HasColumnName("tagValue");
            entity.Property(e => e.TestCaseId).HasColumnName("TestCase_Id");

            entity.HasOne(d => d.IdTypeValueNavigation).WithMany(p => p.Scenarios)
                .HasForeignKey(d => d.IdTypeValue)
                .HasConstraintName("FK_Scenario_Type_Value");

            entity.HasOne(d => d.TestCase).WithMany(p => p.Scenarios)
                .HasForeignKey(d => d.TestCaseId)
                .HasConstraintName("FK_Scenario_TestCase");
        });

        modelBuilder.Entity<TestCase>(entity =>
        {
            entity.ToTable("TestCase");

            entity.Property(e => e.TestCaseId).HasColumnName("TestCase_Id");
            entity.Property(e => e.CsharpContent).HasColumnName("CSharpContent");
            entity.Property(e => e.TestCaseCreatedDate).HasColumnType("date");
            entity.Property(e => e.TestCaseUpdatedDate).HasColumnType("date");
            entity.Property(e => e.TestSuiteId).HasColumnName("TestSuite_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.VersionTest).HasColumnName("Version_Test");

            entity.HasOne(d => d.TestSuite).WithMany(p => p.TestCases)
                .HasForeignKey(d => d.TestSuiteId)
                .HasConstraintName("FK_TestCase_TestSuite");

            entity.HasOne(d => d.User).WithMany(p => p.TestCases)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TestCase_User");
        });

        modelBuilder.Entity<TestReport>(entity =>
        {
            entity.ToTable("TestReport");

            entity.Property(e => e.TestReportId).HasColumnName("TestReport_Id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_date");
            entity.Property(e => e.FilePathC)
                .IsUnicode(false)
                .HasColumnName("FilePath_C#");
            entity.Property(e => e.FilePathJson)
                .IsUnicode(false)
                .HasColumnName("FilePath_Json");
            entity.Property(e => e.FilePathVideo)
                .IsUnicode(false)
                .HasColumnName("FilePath_Video");
            entity.Property(e => e.TestCaseId).HasColumnName("TestCase_Id");
            entity.Property(e => e.TestSuiteId).HasColumnName("TestSuite_Id");

            entity.HasOne(d => d.TestCase).WithMany(p => p.TestReports)
                .HasForeignKey(d => d.TestCaseId)
                .HasConstraintName("FK_TestReport_TestCase");
        });

        modelBuilder.Entity<TestSuite>(entity =>
        {
            entity.ToTable("TestSuite");

            entity.Property(e => e.TestSuiteId).HasColumnName("TestSuite_Id");
            entity.Property(e => e.TestId).HasColumnName("Test_Id");
            entity.Property(e => e.TestSuiteCreatedDate).HasColumnType("date");
            entity.Property(e => e.TestSuiteUpdatedDate).HasColumnType("date");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Test).WithMany(p => p.TestSuites)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK_TestSuite_TestType");

            entity.HasOne(d => d.User).WithMany(p => p.TestSuites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TestSuite_User");
        });

        modelBuilder.Entity<TestType>(entity =>
        {
            entity.HasKey(e => e.TestId);

            entity.ToTable("TestType");

            entity.Property(e => e.TestId).HasColumnName("Test_Id");
            entity.Property(e => e.TestDescription).HasColumnName("Test_Description");
            entity.Property(e => e.TestName).HasColumnName("Test_Name");
        });

        modelBuilder.Entity<TypeValue>(entity =>
        {
            entity.ToTable("Type_Value");

            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.FirstName).HasColumnName("First_Name");
            entity.Property(e => e.LastName).HasColumnName("Last_Name");
            entity.Property(e => e.Password).IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("User_Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role_Role");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Role_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
