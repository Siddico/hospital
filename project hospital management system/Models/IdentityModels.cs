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
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // Configure Entity Framework to throw exceptions when there are database errors
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Add DbSet for Patient model
        public DbSet<Patient> Patients { get; set; }

<<<<<<< HEAD
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

=======
>>>>>>> 00a11ec54d2ce5d42722424882e545991dc44544
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
<<<<<<< HEAD

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
=======
>>>>>>> 00a11ec54d2ce5d42722424882e545991dc44544
        }
    }
}