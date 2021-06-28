namespace Suls.Services
{
    public interface ISubmissionsService
    {
        void Create(string problemId, string code, string userId);

        void Delete(string id);
    }
}