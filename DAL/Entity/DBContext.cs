using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.Entity
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Days> Days { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<DoctorSee> DoctorSee { get; set; }
        public virtual DbSet<DoProc> DoProc { get; set; }
        public virtual DbSet<MakingProcedure> MakingProcedure { get; set; }
        public virtual DbSet<Nurce> Nurce { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Procedure> Procedure { get; set; }
        public virtual DbSet<RecordInPatientFile> RecordInPatientFile { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<ScheduleProcedure> ScheduleProcedure { get; set; }
        public virtual DbSet<Shifts> Shifts { get; set; }
        public virtual DbSet<Specialization> Specialization { get; set; }
        public virtual DbSet<Streets> Streets { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TypeofProc> TypeofProc { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserTypes> UserTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Doctor)
                .WithRequired(e => e.Categories)
                .HasForeignKey(e => e.CategoryID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Days>()
                .Property(e => e.DayOfWeek)
                .IsUnicode(false);

            modelBuilder.Entity<Days>()
                .HasMany(e => e.Schedule)
                .WithRequired(e => e.Days)
                .HasForeignKey(e => e.dayofWeek)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Days>()
                .HasMany(e => e.ScheduleProcedure)
                .WithRequired(e => e.Days)
                .HasForeignKey(e => e.DayID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.DoctorSee)
                .WithRequired(e => e.Doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctor>()
                .HasMany(e => e.Schedule)
                .WithRequired(e => e.Doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DoProc>()
                .HasMany(e => e.Procedure)
                .WithRequired(e => e.DoProc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Nurce>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Nurce>()
                .HasMany(e => e.MakingProcedure)
                .WithRequired(e => e.Nurce)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Nurce>()
                .HasMany(e => e.RecordInPatientFile)
                .WithOptional(e => e.Nurce)
                .HasForeignKey(e => e.NurseID);

            modelBuilder.Entity<Patient>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Male)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.PlaceOfWork)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Document)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.DoctorSee)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.DoProc)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.RecordInPatientFile)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RecordInPatientFile>()
                .Property(e => e.Result)
                .IsUnicode(false);

            modelBuilder.Entity<RecordInPatientFile>()
                .Property(e => e.Diagnosis)
                .IsUnicode(false);

            modelBuilder.Entity<RecordInPatientFile>()
                .HasMany(e => e.DoProc)
                .WithRequired(e => e.RecordInPatientFile)
                .HasForeignKey(e => e.RecordID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ScheduleProcedure>()
                .HasMany(e => e.Procedure)
                .WithRequired(e => e.ScheduleProcedure)
                .HasForeignKey(e => e.ScheduleID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shifts>()
                .HasMany(e => e.Schedule)
                .WithRequired(e => e.Shifts)
                .HasForeignKey(e => e.ShiftID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specialization>()
                .Property(e => e.SpecializationName)
                .IsUnicode(false);

            modelBuilder.Entity<Specialization>()
                .HasMany(e => e.Doctor)
                .WithRequired(e => e.Specialization)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Streets>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<Streets>()
                .HasMany(e => e.Patient)
                .WithRequired(e => e.Streets)
                .HasForeignKey(e => e.StreetID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeofProc>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<TypeofProc>()
                .HasMany(e => e.DoProc)
                .WithRequired(e => e.TypeofProc)
                .HasForeignKey(e => e.ProcID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeofProc>()
                .HasMany(e => e.MakingProcedure)
                .WithRequired(e => e.TypeofProc)
                .HasForeignKey(e => e.ProcedureID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeofProc>()
                .HasMany(e => e.ScheduleProcedure)
                .WithRequired(e => e.TypeofProc)
                .HasForeignKey(e => e.ProcedureID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserTypes>()
                .Property(e => e.UserType)
                .IsUnicode(false);

            modelBuilder.Entity<UserTypes>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserTypes)
                .HasForeignKey(e => e.UserType)
                .WillCascadeOnDelete(false);
        }
    }
}
