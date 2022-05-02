namespace BetControlAuthentication.Models
{
    public class BetResponse
    {
        public List<Bet> Bets { get; set; } = new List<Bet>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int AmountOfBets { get; set; }
        public double UserResult { get; internal set; }
        public double ROI { get; internal set; }
        public IEnumerable<string>? UserBookies { get; internal set; }
        public IEnumerable<string>? UserTipsters { get; internal set; }
        public IEnumerable<string>? UserSports { get; internal set; }
        public string? CommonUserBookie { get; internal set; }
        public string? CommonUserSport { get; internal set; }
        public string? CommonUserTipster { get; internal set; }
    }
}

