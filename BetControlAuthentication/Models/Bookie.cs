using System;
namespace BetControlAuthentication.Models
{
    public class Bookie
    {
        public int Id { get; set; }        
        public string Name { get; set; } = String.Empty;
        //public virtual ICollection<Bet>? Bets { get; set; }
    }
}