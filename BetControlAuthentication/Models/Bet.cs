using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BetControlAuthentication.Models
{
    public class Bet
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-mm-dd:hh-mm}")]
        public DateTime EventTime { get; set; } = DateTime.Now;
        public string Event { get; set; } = String.Empty;
        public string Selection { get; set; } = String.Empty;
        [ForeignKey("Bookie")]
        public int BookieId { get; set; }
        public Bookie? Bookie { get; set; }
        public Sport? Sport { get; set; }
        public Tipster? Tipster { get; set; }
        public double Odds { get; set; }
        public double Stake { get; set; }
        public string Status { get; set; } = "NEW";
        public double Result { get; set; }
        public bool Locked { get; set; } = false;
    }
}