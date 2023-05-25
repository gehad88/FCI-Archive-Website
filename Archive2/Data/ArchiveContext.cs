using System;
using System.Net.Mail;
using Archive2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using modeforview2.Models;

#nullable disable

namespace modelfor_archive.Models
{
    public partial class Archive2Context : DbContext
    {
        public Archive2Context()
        {
        }

        public Archive2Context(DbContextOptions<Archive2Context> options)
            : base(options)
        {
        }

        // Define DbSet properties for each entity in the database
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Messages> Message { get; set; }
        public virtual DbSet<UserAcc> UserAccs { get; set; }
        public virtual DbSet<ListUserMes> ListUserMes { get; set; }
        public virtual DbSet<RemoveUser> RemoveUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure the connection to the database
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-8S0IFLS\SQL2022;Database=Archive;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite primary key for ListUserMes entity
            modelBuilder.Entity<ListUserMes>().HasKey(K => new { K.UserId, K.MessageId });
        }
    }
}
