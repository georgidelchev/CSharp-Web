using System;
using Git.Data;
using System.Linq;
using System.Collections.Generic;
using Git.ViewModels.Repositories;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext db;

        public RepositoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string name, string type, string userId)
        {
            var isPublic = type == "Public";

            var repository = new Repository()
            {
                CreatedOn = DateTime.UtcNow,
                IsPublic = isPublic,
                Name = name,
                OwnerId = userId
            };

            this.db.Repositories.Add(repository);

            this.db.SaveChanges();
        }

        public string GetRepositoryName(string id)
        {
            return this.db
                .Repositories
                .Where(r => r.Id == id)
                .Select(r => r.Name)
                .FirstOrDefault();
        }

        public IEnumerable<DisplayAllRepositoriesViewModel> GetAll()
        {
            var repositories = this.db
                .Repositories
                .Where(r => r.IsPublic)
                .Select(r => new DisplayAllRepositoriesViewModel()
                {
                    Id = r.Id,
                    Owner = r.Owner.Username,
                    CommitsCount = r.Commits.Count,
                    CreatedOn = r.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    Name = r.Name
                })
                .ToList();

            return repositories;
        }
    }
}