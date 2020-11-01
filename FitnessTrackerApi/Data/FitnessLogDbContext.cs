using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using FitnessTrackerApi.Models;

namespace FitnessTrackerApi.Data
{
    public class FitnessLogDbContext : DbContext
    {
        public FitnessLogDbContext(DbContextOptions<FitnessLogDbContext> options)
            : base(options)
        { }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
