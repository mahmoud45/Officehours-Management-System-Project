using officehours_management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace officehours_management.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>(e => e.Property(IR => IR.Id).HasMaxLength(150));

            modelBuilder.Entity<ApplicationUser>(e => e.Property(IU => IU.Id).HasMaxLength(150));
            modelBuilder.Entity<ApplicationUser>(e => e.Property(IU => IU.is_active).HasDefaultValue(true));
            modelBuilder.Entity<ApplicationUser>(e => e.Property(IU => IU.is_staff).HasDefaultValue(false));
            modelBuilder.Entity<ApplicationUser>(e => e.Property(IU => IU.is_superuser).HasDefaultValue(false));
            modelBuilder.Entity<ApplicationUser>(e => e.Property(IU => IU.FullName).HasMaxLength(250));
            modelBuilder.Entity<ApplicationUser>().HasIndex(IU => IU.Email).IsUnique();
            modelBuilder.Entity<ApplicationUser>().HasIndex(IU => IU.UserName).IsUnique();

            modelBuilder.Entity<IdentityRoleClaim<string>>(e => e.Property(IR => IR.RoleId).HasMaxLength(150));

            modelBuilder.Entity<IdentityUserClaim<string>>(e => e.Property(IR => IR.UserId).HasMaxLength(150));

            modelBuilder.Entity<IdentityUserLogin<string>>(e => e.Property(IU => IU.LoginProvider).HasMaxLength(200));
            modelBuilder.Entity<IdentityUserLogin<string>>(e => e.Property(IU => IU.ProviderKey).HasMaxLength(200));
            modelBuilder.Entity<IdentityUserLogin<string>>(e => e.Property(IU => IU.UserId).HasMaxLength(200));

            modelBuilder.Entity<IdentityUserRole<string>>(e => e.Property(IR => IR.UserId).HasMaxLength(150));
            modelBuilder.Entity<IdentityUserRole<string>>(e => e.Property(IR => IR.RoleId).HasMaxLength(150));

            modelBuilder.Entity<IdentityUserToken<string>>(e => e.Property(IR => IR.UserId).HasMaxLength(150));
            modelBuilder.Entity<IdentityUserToken<string>>(e => e.Property(IR => IR.LoginProvider).HasMaxLength(150));
            modelBuilder.Entity<IdentityUserToken<string>>(e => e.Property(IR => IR.Name).HasMaxLength(150));

            modelBuilder.Entity<SubjectStaff>().HasKey(ss => new { ss.SubjectId, ss.StaffId });
            modelBuilder.Entity<SubjectStudent>().HasKey(ss => new { ss.SubjectId, ss.StudentId });
            
            modelBuilder.Entity<SubjectStaff>()
            .HasOne<Subject>( ss => ss.subject)
            .WithMany(s => s.Subjects_Staff)
            .HasForeignKey(ss => ss.SubjectId);

            modelBuilder.Entity<SubjectStaff>()
            .HasOne<ApplicationUser>( ss => ss.Staff)
            .WithMany(s => s.Subjects_Staff)
            .HasForeignKey(ss => ss.StaffId);

            modelBuilder.Entity<SubjectStudent>()
            .HasOne<ApplicationUser>(ss => ss.Student)
            .WithMany(s => s.Subjects_student)
            .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<SubjectStudent>()
            .HasOne<Subject>(ss => ss.subject)
            .WithMany(s => s.Subjects_student)
            .HasForeignKey(ss => ss.SubjectId);

            modelBuilder.Entity<OfficeHour>()
            .HasOne<ApplicationUser>(oh => oh.Staff)
            .WithMany(au => au.officehours)
            .HasForeignKey(oh => oh.StaffId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<OfficeHour> OfficeHours { get; set; }
        public DbSet<SubjectStaff> SubjectStaff { get; set ;}
        public DbSet<SubjectStudent> SubjectStudents { get; set ;}
    }
}