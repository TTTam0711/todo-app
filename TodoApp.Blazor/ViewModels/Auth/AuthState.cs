namespace TodoApp.Blazor.ViewModels.Auth
{
    public sealed class AuthState
    {
        public string? AccessToken { get; private set; }
        public DateTime? AccessTokenExpiresAtUtc { get; private set; }

        public Guid? UserId { get; private set; }
        public string? Email { get; private set; }
        public string? DisplayName { get; private set; }

        public bool IsAuthenticated =>
            !string.IsNullOrWhiteSpace(AccessToken) && UserId.HasValue;

        // 🔥 Notify UI
        public event Action? OnChange;

        public void Set(string accessToken, int expiresIn, Guid userId, string email, string displayName)
        {
            AccessToken = accessToken;
            AccessTokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(expiresIn);

            UserId = userId;
            Email = email;
            DisplayName = displayName;

            NotifyStateChanged();
        }

        public void Clear()
        {
            AccessToken = null;
            AccessTokenExpiresAtUtc = null;

            UserId = null;
            Email = null;
            DisplayName = null;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
