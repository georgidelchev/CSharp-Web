using Suls.Data;
using System.Linq;
using Suls.ViewModels.Problems;
using System.Collections.Generic;
using Suls.ViewModels.Submissions;

namespace Suls.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly ApplicationDbContext db;

        public ProblemsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string name, int points)
        {
            var problem = new Problem()
            {
                Name = name,
                Points = points
            };

            this.db.Problems.Add(problem);

            this.db.SaveChanges();
        }

        public IEnumerable<DisplayAllProblemsViewModel> GetAll()
        {
            var problems = this.db
                .Problems
                .Select(p => new DisplayAllProblemsViewModel()
                {
                    Id = p.Id,
                    Count = p.Submissions.Count,
                    Name = p.Name
                })
                .ToList();

            return problems;
        }

        public DisplayProblemDetailsViewModel GetDetails(string id)
        {
            var problemDetails = this.db
                .Problems
                .Where(p => p.Id == id)
                .Select(p => new DisplayProblemDetailsViewModel()
                {
                    Name = p.Name,
                    Submissions = p.Submissions.Select(s => new SubmissionViewModel()
                    {
                        AchievedResult = s.AchievedResults,
                        CreatedOn = s.CreatedOn,
                        MaxPoints = p.Points,
                        SubmissionId = s.Id,
                        Username = s.User.Username
                    })
                })
                .FirstOrDefault();

            return problemDetails;
        }

        public string GetNameById(string id)
        {
            return this.db
                .Problems
                .Where(p => p.Id == id)
                .Select(p => p.Name)
                .FirstOrDefault();
        }
    }
}