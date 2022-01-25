using System;
using BetControlAuthentication.Models;

namespace BetControlAuthentication
{
	public class BetResponse
	{
        public List<Bet> Bets { get; set; } = new List<Bet>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

