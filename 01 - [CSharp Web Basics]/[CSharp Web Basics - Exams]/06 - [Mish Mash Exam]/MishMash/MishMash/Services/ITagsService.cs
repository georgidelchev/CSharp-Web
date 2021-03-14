namespace MishMash.Services
{
    public interface ITagsService
    {
        int Create(string name);

        int FindTagIdByName(string tagName);

        bool CheckForTagExisting(string name);
    }
}