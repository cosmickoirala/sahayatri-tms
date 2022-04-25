
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TravelManagementSystem.Models;

namespace TravelManagementSystem.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser, AppRole , int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        override
        public DbSet<AppRole> Roles { get; set; }
        override
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Place1> Places { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
