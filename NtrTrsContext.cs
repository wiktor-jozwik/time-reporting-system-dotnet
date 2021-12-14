using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  

namespace NtrTrs
{
    public class NtrTrsContext : DbContext
    {
        public NtrTrsContext()
        {
        }

        public NtrTrsContext(DbContextOptions<NtrTrsContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public DbSet<AcceptedEntry> AcceptedEntries { get; set; }

        public DbSet<Activity> Activties { get; set; }

        public DbSet<MonthEntry> MonthEntries { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=ntr;Username=postgres;Password=postgres");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         base.OnModelCreating(modelBuilder);
         modelBuilder.Entity<Activity>()
             .Property(b => b.Active)
             .HasDefaultValue(true);

        modelBuilder.Entity<MonthEntry>()
             .Property(b => b.Frozen)
             .HasDefaultValue(false);

        modelBuilder.Entity<User>()
             .Property(b => b.LoggedIn)
             .HasDefaultValue(false);
        }
    }

    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("logged_in")]
        public bool LoggedIn { get; set; }
    }

    [Table("entries")]
    public class Entry
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Please enter date")]  
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter code of project")]  

        [Column("code")]
        public String Code { get; set; }

        [Column("subcode")]
        public string Subcode { get; set; }

        [Column("time")]
        [Required(ErrorMessage = "Please enter time")]  
        public int Time { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }

    [Table("accepted_entries")]
    public class AcceptedEntry
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("time")]
        public int Time { get; set; }
    }

    [Table("activities")]
    public class Activity
    {        
        [Column("id")]
        public int Id { get; set; }
        
        [Column("code")]
        [Required(ErrorMessage = "Please enter code")]  
        public string Code { get; set; }
        public User Manager { get; set; }

        [Column("budget")]
        [Required(ErrorMessage = "Please enter budget")]  
        public int Budget { get; set; }

        [Column("active")]
        public bool Active { get; set; }
        public List<Activity> Subactivities { get; set; }

        public List<Entry> Entries { get; } = new();
    }

    [Table("month_entries")]
    public class MonthEntry
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("frozen")]
        public bool Frozen { get; set; }

        public List<Entry> Entries { get; set; }

        public List<AcceptedEntry> Accepted { get; set; }
    }
}