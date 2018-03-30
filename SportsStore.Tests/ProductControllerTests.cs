using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            // Arrange

            // Create mock repository with 5 fake products
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "p1"},
                new Product {ProductId = 2, Name = "p2"},
                new Product {ProductId = 3, Name = "p3"},
                new Product {ProductId = 4, Name = "p4"},
                new Product {ProductId = 5, Name = "p5"}
            }).AsQueryable<Product>());

            // Pass mocked repository to product controller
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            
            // Act
            var result = controller.List(null, 2).ViewData.Model as ProductListViewModel;
            
            // Assert
            var prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("p4", prodArray[0].Name);
            Assert.Equal("p5", prodArray[1].Name);
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "p1"},
                new Product {ProductId = 2, Name = "p2"},
                new Product {ProductId = 3, Name = "p3"},
                new Product {ProductId = 4, Name = "p4"},
                new Product {ProductId = 5, Name = "p5"}
            }).AsQueryable<Product>());

            var controller = new ProductController(mock.Object) {PageSize = 3};

            var result = controller.List(null, 2).ViewData.Model as ProductListViewModel;

            var pageInfo = result.PagingInfo;

            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void CanFilterProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "p1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "p2", Category = "Cat1"},
                new Product {ProductId = 3, Name = "p3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "p4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "p5", Category = "Cat3"}
            }).AsQueryable<Product>());

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = (controller.List("Cat1", 1).ViewData.Model as ProductListViewModel).Products.ToArray();

            Assert.Equal(3, result.Length);
            Assert.True(result[0].Name == "p1" && result[0].Category == "Cat1");
            Assert.True(result[1].Name == "p2" && result[1].Category == "Cat1");
            Assert.True(result[2].Name == "p3" && result[2].Category == "Cat1");
        }
    }
}
