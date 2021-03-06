using System;
using System.Text.Json.Serialization;

namespace BetControlAuthentication.Models
{
    public class Tipster
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        [JsonIgnore]
        public virtual ICollection<Bet>? Bets { get; set; }
    }
}

