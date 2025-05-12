﻿namespace project_hospital_management_system.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Transactions;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<project_hospital_management_system.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true; // Allow data loss during migrations
            ContextKey = "project_hospital_management_system.Models.ApplicationDbContext";
        }

        protected override void Seed(project_hospital_management_system.Models.ApplicationDbContext context)
        {
            // This method will be called after migrating to the latest version.
            // You can use the DbSet<T>.AddOrUpdate() helper extension method
            // to avoid creating duplicate seed data.

            try
            {
                // Use a transaction to ensure data consistency
                using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled))
                {

            // Add sample patients if the Patients table is empty
            if (!context.Patients.Any())
            {
                context.Patients.AddOrUpdate(
                    p => p.Name,
                    new Patient
                    {
                        Name = "John Smith",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1980, 6, 15),
                        Phone = "555-123-4567",
                        Address = "123 Main St, Anytown, USA"
                    },
                    new Patient
                    {
                        Name = "Jane Doe",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 3, 22),
                        Phone = "555-987-6543",
                        Address = "456 Oak Ave, Somewhere, USA"
                    }
                );
            }

            // Add sample doctors if the Doctors table is empty
            if (!context.Doctors.Any())
            {
                context.Doctors.AddOrUpdate(
                    d => d.Name,
                    new Doctor
                    {
                        Name = "Dr. Robert Johnson",
                        Specialization = "Cardiology",
                        Phone = "555-111-2222"
                    },
                    new Doctor
                    {
                        Name = "Dr. Sarah Williams",
                        Specialization = "Neurology",
                        Phone = "555-333-4444"
                    },
                    new Doctor
                    {
                        Name = "Dr. Michael Chen",
                        Specialization = "Pediatrics",
                        Phone = "555-555-6666"
                    }
                );
            }

            // Add sample appointments if the Appointments table is empty
            if (!context.Appointments.Any() && context.Patients.Any() && context.Doctors.Any())
            {
                // Get the first patient and doctor for sample data
                var firstPatient = context.Patients.FirstOrDefault();
                var firstDoctor = context.Doctors.FirstOrDefault();

                if (firstPatient != null && firstDoctor != null)
                {
                    context.Appointments.AddOrUpdate(
                        a => a.AppointmentDate,
                        new Appointment
                        {
                            PatientId = firstPatient.PatientId,
                            DoctorId = firstDoctor.DoctorId,
                            AppointmentDate = DateTime.Now.AddDays(1).Date.AddHours(10), // Tomorrow at 10 AM
                            Notes = "Initial consultation"
                        },
                        new Appointment
                        {
                            PatientId = firstPatient.PatientId,
                            DoctorId = firstDoctor.DoctorId,
                            AppointmentDate = DateTime.Now.AddDays(7).Date.AddHours(14), // Next week at 2 PM
                            Notes = "Follow-up appointment"
                        }
                    );
                }
            }

            // Add sample medical records if the MedicalRecords table is empty
            if (!context.MedicalRecords.Any() && context.Patients.Any() && context.Doctors.Any())
            {
                // Get the first patient and doctor for sample data
                var firstPatient = context.Patients.FirstOrDefault();
                var firstDoctor = context.Doctors.FirstOrDefault();

                if (firstPatient != null && firstDoctor != null)
                {
                    context.MedicalRecords.AddOrUpdate(
                        m => m.RecordDate,
                        new MedicalRecord
                        {
                            PatientId = firstPatient.PatientId,
                            DoctorId = firstDoctor.DoctorId,
                            Diagnosis = "Hypertension (High Blood Pressure)",
                            Treatment = "Prescribed Lisinopril 10mg daily. Recommended lifestyle changes including reduced sodium intake, regular exercise, and weight management.",
                            RecordDate = DateTime.Now.AddDays(-30) // 30 days ago
                        },
                        new MedicalRecord
                        {
                            PatientId = firstPatient.PatientId,
                            DoctorId = firstDoctor.DoctorId,
                            Diagnosis = "Upper Respiratory Infection",
                            Treatment = "Rest, increased fluid intake, and over-the-counter pain relievers for symptom management. Follow up if symptoms worsen or persist beyond 7 days.",
                            RecordDate = DateTime.Now.AddDays(-15) // 15 days ago
                        }
                    );
                }
            }

            // Add sample medications if the Medications table is empty
            if (!context.Medications.Any())
            {
                context.Medications.AddOrUpdate(
                    m => m.Name,
                    new Medication
                    {
                        Name = "Lisinopril",
                        Description = "ACE inhibitor used to treat high blood pressure and heart failure",
                        Dose = "10mg once daily"
                    },
                    new Medication
                    {
                        Name = "Amoxicillin",
                        Description = "Antibiotic used to treat bacterial infections",
                        Dose = "500mg three times daily"
                    },
                    new Medication
                    {
                        Name = "Ibuprofen",
                        Description = "Non-steroidal anti-inflammatory drug (NSAID) used to relieve pain and reduce inflammation",
                        Dose = "400mg every 6-8 hours as needed"
                    },
                    new Medication
                    {
                        Name = "Metformin",
                        Description = "Oral diabetes medicine that helps control blood sugar levels",
                        Dose = "500mg twice daily with meals"
                    },
                    new Medication
                    {
                        Name = "Atorvastatin",
                        Description = "Statin medication used to lower cholesterol and reduce the risk of heart disease",
                        Dose = "20mg once daily at bedtime"
                    }
                );
            }

            // Add sample prescriptions if the Prescriptions table is empty
            if (!context.Prescriptions.Any() && context.Patients.Any() && context.Doctors.Any() && context.Medications.Any())
            {
                // Get the first patient, doctor, and medication for sample data
                var firstPatient = context.Patients.FirstOrDefault();
                var firstDoctor = context.Doctors.FirstOrDefault();
                var lisinopril = context.Medications.FirstOrDefault(m => m.Name == "Lisinopril");
                var amoxicillin = context.Medications.FirstOrDefault(m => m.Name == "Amoxicillin");

                if (firstPatient != null && firstDoctor != null && lisinopril != null && amoxicillin != null)
                {
                    context.Prescriptions.AddOrUpdate(
                        p => new { p.PatientId, p.MedicationId, p.DateIssued },
                        new Prescription
                        {
                            PatientId = firstPatient.PatientId,
                            DoctorId = firstDoctor.DoctorId,
                            MedicationId = lisinopril.MedicationId,
                            DateIssued = DateTime.Now.AddDays(-30),
                            Instructions = "Take one 10mg tablet by mouth once daily in the morning. Take with or without food. Do not discontinue without consulting your doctor."
                        },
                        new Prescription
                        {
                            PatientId = firstPatient.PatientId,
                            DoctorId = firstDoctor.DoctorId,
                            MedicationId = amoxicillin.MedicationId,
                            DateIssued = DateTime.Now.AddDays(-15),
                            Instructions = "Take one 500mg capsule by mouth three times daily (every 8 hours) for 10 days. Take with food to reduce stomach upset. Complete the full course even if symptoms improve."
                        }
                    );
                }
            }

            // Add sample invoices if the Invoices table is empty
            if (!context.Invoices.Any() && context.Patients.Any())
            {
                // Get the first patient for sample data
                var firstPatient = context.Patients.FirstOrDefault();
                var secondPatient = context.Patients.Skip(1).FirstOrDefault() ?? firstPatient;

                if (firstPatient != null)
                {
                    context.Invoices.AddOrUpdate(
                        i => new { i.PatientId, i.DateIssued },
                        new Invoice
                        {
                            PatientId = firstPatient.PatientId,
                            DateIssued = DateTime.Now.AddDays(-60),
                            TotalAmount = 1250.75m,
                            Status = "Paid"
                        },
                        new Invoice
                        {
                            PatientId = firstPatient.PatientId,
                            DateIssued = DateTime.Now.AddDays(-30),
                            TotalAmount = 850.50m,
                            Status = "Paid"
                        },
                        new Invoice
                        {
                            PatientId = secondPatient.PatientId,
                            DateIssued = DateTime.Now.AddDays(-15),
                            TotalAmount = 1500.25m,
                            Status = "Pending"
                        },
                        new Invoice
                        {
                            PatientId = firstPatient.PatientId,
                            DateIssued = DateTime.Now.AddDays(-5),
                            TotalAmount = 375.00m,
                            Status = "Pending"
                        },
                        new Invoice
                        {
                            PatientId = secondPatient.PatientId,
                            DateIssued = DateTime.Now.AddDays(-90),
                            TotalAmount = 2100.80m,
                            Status = "Overdue"
                        }
                    );
                }
            }

                    // Save changes and complete the transaction
                    context.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error in Seed method: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                // Rethrow the exception to prevent database initialization
                throw;
            }
        }
    }
}
