using System;
using System.Collections.Generic;
using System.Linq;
using Andreys.Data;
using Andreys.Data.Enumerations;
using Andreys.ViewModels.Products;

namespace Andreys.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AndreysDbContext dbContext;

        public ProductsService(AndreysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(AddProductInputModel input)
        {
            var product = new Product()
            {
                Name = input.Name,
                Category = Enum.Parse<Category>(input.Category),
                Description = input.Description,
                Gender = Enum.Parse<Gender>(input.Gender),
                ImageUrl = input.ImageUrl,
                Price = input.Price,
            };

            this.dbContext.Add(product);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<GetAllProductsViewModel> GetAll()
            => this.dbContext
                .Products
                .Select(p => new GetAllProductsViewModel()
                {
                    Name = p.Name,
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                })
                .ToList();

        public GetProductDetailsViewModel GetDetails(int id)
            => this.dbContext
                .Products
                .Where(p => p.Id == id)
                .Select(p => new GetProductDetailsViewModel()
                {
                    Name = p.Name,
                    Gender = p.Gender.ToString(),
                    Category = p.Category.ToString(),
                    Description = p.Description,
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                })
                .FirstOrDefault();

        public void Delete(int id)
        {
            var product = this.dbContext
                .Products
                .FirstOrDefault(p => p.Id == id);

            this.dbContext.Products.Remove(product);
            this.dbContext.SaveChanges();
        }
    }
}
