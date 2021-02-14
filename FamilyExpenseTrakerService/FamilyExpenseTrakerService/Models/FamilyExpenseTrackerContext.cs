using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public class FamilyExpenseTrackerContext : IdentityDbContext
    {
        public FamilyExpenseTrackerContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<FamilyExpense> FamilyExpenses { get; set; }
        public DbSet<FamilyMaster> FamilyMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FamilyMaster>()
                .HasKey(fm => fm.FamilyId);

            modelBuilder.Entity<FamilyExpense>()
                .HasKey(fe => fe.ExpenseId);

            modelBuilder.Entity<FamilyExpense>()
            .HasOne(p => p.FamilyMember)
            .WithMany(b => b.FamilyExpenses);
        }
    }
}
