namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public interface IUserTokenConfiguration
    {
        string GetAuthorizationUrl();
    }
}