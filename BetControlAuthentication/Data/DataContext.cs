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

        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Bookie> Bookies { get; set; }
    }
}