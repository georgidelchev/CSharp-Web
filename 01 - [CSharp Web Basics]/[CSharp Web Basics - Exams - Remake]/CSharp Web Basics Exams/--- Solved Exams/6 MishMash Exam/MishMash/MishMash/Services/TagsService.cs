using System.Linq;
using MishMash.Data;

namespace MishMash.Services
{
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext db;

        public TagsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int Create(string name)
        {
            var tag = new Tag()
            {
                Name = name
            };

            this.db.Tags.Add(tag);

            this.db.SaveChanges();

            return tag.Id;
        }

        public int FindTagIdByName(string tagName)
        {
            var tag = this.db
                .Tags
                .FirstOrDefault(t => t.Name == tagName);

            return tag.Id;
        }

        public bool CheckForTagExisting(string name)
        {
            return this.db
                .Tags
                .Any(t => t.Name == name);
        }
    }
}