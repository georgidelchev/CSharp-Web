namespace SUS.HTTP.Enums
{
    public enum HttpStatusCode
    {
        Ok = 200,
        MovedPermanently = 301,
        Found = 302,
        TemporaryRedirect = 307,
        Forbidden = 403,
        NotFound = 404,
        ServerError = 500
    }
}