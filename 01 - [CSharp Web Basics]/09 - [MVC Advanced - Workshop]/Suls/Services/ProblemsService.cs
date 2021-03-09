using Suls.Data;
using System.Linq;
using Suls.ViewModels.Problems;
using System.Collections.Generic;

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

        public IEnumerable<HomePageProblemViewModel> GetAll()
        {
            return this.db
                .Problems
                .Select(p => new HomePageProblemViewModel()
                {
                    Count = p.Submissions.Count(),
                    Name = p.Name,
                    Id = p.Id
                })
                .ToList();
        }

        public string GetNameById(string id)
        {
            return this.db
                .Problems
                .Where(p => p.Id == id)
                .Select(p => p.Name)
                .FirstOrDefault();
        }

        public ProblemViewModel GetById(string id)
        {
            return this.db
                .Problems
                .Where(p => p.Id == id)
                .Select(p => new ProblemViewModel()
                {
                    Name = p.Name,
                    Submissions = p.Submissions.Select(s => new SubmissionViewModel()
                    {
                        AchievedResult = s.AchievedResult,
                        CreatedOn = s.CreatedOn,
                        MaxPoints = s.Problem.Points,
                        Username = s.User.Username,
                        SubmissionId = s.Id
                    })
                })
                .FirstOrDefault();
        }
    }
}