namespace Suls.Services
{
    public interface ISubmissionsService
    {
        void Create(string userId, string problemId, string code);

        void Delete(string id);
    }
}