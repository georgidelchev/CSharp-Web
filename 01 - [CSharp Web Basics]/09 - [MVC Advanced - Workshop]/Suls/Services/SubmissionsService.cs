using System;
using System.Linq;
using Suls.Data;

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

        public void Create(string userId, string problemId, string code)
        {
            var problemMaxPoints = this.db
                .Problems
                .Where(p => p.Id == problemId)
                .Select(p => p.Points)
                .FirstOrDefault();

            var submission = new Submission()
            {
                ProblemId = problemId,
                CreatedOn = DateTime.UtcNow,
                Code = code,
                AchievedResult = this.random.Next(0, problemMaxPoints + 1),
                UserId = userId
            };

            this.db.Submissions.Add(submission);

            this.db.SaveChanges();
        }

        public void Delete(string id)
        {
            var submissionToDelete = this.db
                .Submissions
                .FirstOrDefault(s => s.Id == id);

            this.db
                .Submissions
                .Remove(submissionToDelete);

            this.db.SaveChanges();
        }
    }
}