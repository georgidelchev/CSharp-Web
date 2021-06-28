using Andreys.Data.Enums;

namespace Andreys.ViewModels.Products
{
    public class DisplayProductDetailViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public Gender Gender { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}