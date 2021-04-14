using System;

namespace MyRecipes.Web.ViewModels
{
    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage
            => this.PageNumber > 1;

        public bool HasNextPage
            => this.PageNumber < this.PagesCount;

        public int PreviousPageNumber
            => this.PageNumber - 1;

        public int NextPageNumber
            => this.PageNumber + 1;

        public int RecipesCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PagesCount
            => (int)Math.Ceiling((double)this.RecipesCount / this.ItemsPerPage);
    }
}
