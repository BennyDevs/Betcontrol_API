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
        public string? Bookie { get; set; }
        public string? Sport { get; set; }
        public string? Tipster { get; set; }
        public double Odds { get; set; }
        public double Stake { get; set; }
        public string Status { get; set; } = "NEW";
        public double Result { get; set; }
        public bool Locked { get; set; } = false;
    }
}