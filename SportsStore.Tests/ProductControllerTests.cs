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
            IEnumerable<Product> result = controller.Index(2).ViewData.Model as IEnumerable<Product>;
            
            // Assert
            var prodArray = result.ToArray();
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

            var result = controller.Index(2).ViewData.Model as ProductListViewModel;

            var pageInfo = result.PagingInfo;

            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}
