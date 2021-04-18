using System.Linq;
using System.Threading.Tasks;

using MyRecipes.Data.Common.Repositories;
using MyRecipes.Data.Models;

namespace MyRecipes.Services.Data
{
    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task SetVoteAsync(int recipeId, string userId, byte value)
        {
            if (this.votesRepository
                .AllAsNoTracking()
                .Any(v => v.RecipeId == recipeId && v.UserId == userId))
            {
                return;
            }

            var vote = new Vote()
            {
                UserId = userId,
                RecipeId = recipeId,
                Value = value,
            };

            await this.votesRepository.AddAsync(vote);
            await this.votesRepository.SaveChangesAsync();
        }

        public double GetAverageVotes(int recipeId)
        {
            var averageVotes = this.votesRepository
                .All()
                .Where(v => v.RecipeId == recipeId)
                .Average(v => v.Value);

            return averageVotes;
        }
    }
}
