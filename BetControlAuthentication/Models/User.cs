namespace BetControlAuthentication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? Username { get; set; }
        public string? Bio { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public ICollection<Bet>? Bets { get; set; }
    }
}

