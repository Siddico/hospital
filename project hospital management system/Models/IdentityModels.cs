using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace project_hospital_management_system.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }

        // Navigation properties
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }

        public ApplicationUser()
        {
            DateCreated = DateTime.Now;
            IsActive = true;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // Database initialization is handled in Global.asax.cs
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Add DbSet for Patient model
        public DbSet<Patient> Patients { get; set; }

        // Add DbSet for Doctor model
        public DbSet<Doctor> Doctors { get; set; }

        // Add DbSet for Appointment model
        public DbSet<Appointment> Appointments { get; set; }

        // Add DbSet for MedicalRecord model
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        // Add DbSet for Medication model
        public DbSet<Medication> Medications { get; set; }

        // Add DbSet for Prescription model
        public DbSet<Prescription> Prescriptions { get; set; }

        // Add DbSet for Invoice model
        public DbSet<Invoice> Invoices { get; set; }

        // Add DbSet for Dashboard statistics
        public DbSet<DashboardStatistic> DashboardStatistics { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Patient entity
            modelBuilder.Entity<Patient>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Gender)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(200);

            // Configure Doctor entity
            modelBuilder.Entity<Doctor>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Specialization)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}