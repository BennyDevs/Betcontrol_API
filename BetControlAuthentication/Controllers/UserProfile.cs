namespace BetControlAuthentication.Controllers
{
    internal class UserProfile
    {
        public string Email { get; internal set; } = String.Empty;
        public string Username { get; internal set; } = String.Empty;
        public string? Bio { get; internal set; }
    }
}