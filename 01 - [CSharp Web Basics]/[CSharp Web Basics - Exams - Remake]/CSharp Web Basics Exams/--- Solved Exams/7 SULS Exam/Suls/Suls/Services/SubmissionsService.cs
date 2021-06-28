using System;
using Suls.Data;
using System.Linq;

namespace Suls.Services
{
    public class SubmissionsService : ISubmissionsService
    {
        private readonly ApplicationDbContext db;
        private readonly Random random;

        public SubmissionsService(ApplicationDbContext db, Random random)
        {
            this.db = db;
            this.random = random;
        }

        public void Create(string problemId, string code, string userId)
        {
            var problemMaxPoints = this.db
                .Problems
                .Where(p => p.Id == problemId)
                .Select(p => p.Points)
                .FirstOrDefault();

            var submission = new Submission()
            {
                AchievedResults = this.random.Next(0, problemMaxPoints + 1),
                Code = code,
                CreatedOn = DateTime.UtcNow,
                ProblemId = problemId,
                UserId = userId,
            };

            this.db.Submissions.Add(submission);

            this.db.SaveChanges();
        }

        public void Delete(string id)
        {
            var submission = this.db
                .Submissions
                .FirstOrDefault(s => s.Id == id);

            this.db.Submissions.Remove(submission);

            this.db.SaveChanges();
        }
    }
}