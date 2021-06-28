using System;
using System.Linq;
using Andreys.Data;
using Andreys.Data.Enums;
using System.Collections.Generic;
using Andreys.ViewModels.Products;

namespace Andreys.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AndreysDbContext db;

        public ProductsService(AndreysDbContext db)
        {
            this.db = db;
        }

        public void Add(AddProductInputModel model)
        {
            var product = new Product()
            {
                Category = Enum.Parse<Category>(model.Category),
                Description = model.Description,
                Gender = Enum.Parse<Gender>(model.Gender),
                ImageUrl = model.ImageUrl,
                Name = model.Name,
                Price = model.Price
            };

            this.db.Products.Add(product);

            this.db.SaveChanges();
        }

        public IEnumerable<DisplayAllProductsViewModel> DisplayAll()
        {
            var allProducts = this.db
                .Products
                .Select(p => new DisplayAllProductsViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToList();

            return allProducts;
        }

        public DisplayProductDetailViewModel GetDetail(int id)
        {
            var product = this.db
                .Products
                .Where(p => p.Id == id)
                .Select(p => new DisplayProductDetailViewModel()
                {
                    Name = p.Name,
                    Gender = p.Gender,
                    Category = p.Category,
                    Description = p.Description,
                    Id = p.Id,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .FirstOrDefault();

            return product;
        }

        public bool IsProductExisting(int id)
        {
            return this.db
                .Products
                .Any(p => p.Id == id);
        }

        public void Delete(int id)
        {
            var product = this.db
                .Products
                .FirstOrDefault(p => p.Id == id);

            this.db.Remove(product);

            this.db.SaveChanges();
        }
    }
}