using System;
namespace BetControlAuthentication
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = String.Empty;
        public string Username { get; internal set; }
    }
}

