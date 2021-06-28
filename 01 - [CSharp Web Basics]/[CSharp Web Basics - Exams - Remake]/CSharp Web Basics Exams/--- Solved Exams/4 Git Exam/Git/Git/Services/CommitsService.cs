using System;
using Git.Data;
using System.Linq;
using Git.ViewModels.Commits;
using System.Collections.Generic;

namespace Git.Services
{
    public class CommitsService : ICommitsService
    {
        private readonly ApplicationDbContext db;

        public CommitsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string description, string repositoryId, string userId)
        {
            var commit = new Commit()
            {
                CreatedOn = DateTime.UtcNow,
                Description = description,
                CreatorId = userId,
                RepositoryId = repositoryId
            };

            this.db.Commits.Add(commit);

            this.db.SaveChanges();
        }

        public IEnumerable<DisplayAllCommitsViewModel> GetAll(string userId)
        {
            var commits = this.db
                .Commits
                .Where(c => c.CreatorId == userId)
                .Select(c => new DisplayAllCommitsViewModel()
                {
                    Id = c.Id,
                    CreatedOn = c.CreatedOn.ToString("dd/MM/yyyy HH:ss"),
                    Description = c.Description,
                    Repository = c.Repository.Name
                })
                .ToList();

            return commits;
        }

        public bool Delete(string id, string userId)
        {
            var commit = this.db
                .Commits
                .FirstOrDefault(c => c.Id == id);

            if (commit.CreatorId != userId)
            {
                return false;
            }

            this.db.Commits.Remove(commit);

            this.db.SaveChanges();

            return true;
        }
    }
}