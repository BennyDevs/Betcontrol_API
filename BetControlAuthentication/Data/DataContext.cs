using System;
using BetControlAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace BetControlAuthentication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Bet> Bets => Set<Bet>();
        public DbSet<Sport> Sports => Set<Sport>();
        public DbSet<Bookie> Bookies => Set<Bookie>();
        public DbSet<Tipster> Tipsters => Set<Tipster>();
    }
}