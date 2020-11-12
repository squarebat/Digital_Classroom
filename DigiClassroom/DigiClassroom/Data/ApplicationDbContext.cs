﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DigiClassroom.Models;

namespace DigiClassroom.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity <BlackBoard>()
                .Property(bb => bb.content).HasColumnType("CLOB");
            builder.Entity<ClassroomUser>().HasKey(cu => new { cu.ClassroomId, cu.AppUserId });
            builder.Entity<ClassroomUser>()
                .HasOne<Classroom>(cu => cu.Classroom)
                .WithMany(cu => cu.ClassroomUsers)
                .HasForeignKey(cu => cu.ClassroomId);
            builder.Entity<ClassroomUser>()
                .HasOne<AppUser>(au => au.AppUser)
                .WithMany(cu => cu.ClassroomUsers)
                .HasForeignKey(cu => cu.AppUserId);
            builder.Entity<BlackBoard>()
                .HasOne<ClassroomUser>(cu => cu.ClassroomUser)
                .WithMany(bb => bb.BlackBoards)
                .HasForeignKey(bb => new { bb.ClassroomId, bb.AppUserId });
        }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<ClassroomUser> ClassroomUsers { get; set; }
        public DbSet<BlackBoard> BlackBoards { get; set; }
    }
}