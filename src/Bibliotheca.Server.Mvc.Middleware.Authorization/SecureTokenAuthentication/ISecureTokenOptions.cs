namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public interface ISecureTokenOptions
    {
        string SecureToken { get; set; }
    }
}