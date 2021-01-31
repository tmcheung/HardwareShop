using Common.DTOs;
using Common.Models;
using DataLayer;
using DataLayer.EF;
using FluentAssertions;
using HardwareShopAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.HardwareShopTests
{
    public class ProductControllerTests
    {
        [Theory]
        [InlineData(0, 0, 10, 0)]
        [InlineData(10, 0, 10, 10)]
        [InlineData(10, 5, 10, 5)]
        [InlineData(10, 10, 10, 0)]
        [InlineData(0, 10, 10, 0)]
        public void GetProducts_Should_Return_EqualOrLessThan_TakeFrom_Minus_Amount_Products(int initAmount, int takeFrom, int amount, int expected)
        {
            //arrange
            var contextMock = new Mock<IShopContext>();
            var mockSet = new Mock<DbSet<Product>>();
            var repository = new ProductRepository(contextMock.Object);
            var service = new ProductService(repository);
            var controller = new ProductController(service);

            var data = new List<Product>();
            while (data.Count < initAmount)
                data.Add(new Product { Id = Guid.NewGuid() });

            mockSet.As<IQueryable<Product>>()
                .SetupIQueryable(data.AsQueryable());
            contextMock.SetupGet(s => s.Product).Returns(mockSet.Object);

            //act
            var result = controller.GetProducts(takeFrom, amount, null);

            //assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            var resultValue = okObjectResult.Value as IDictionary<Guid, ProductDTO>;
            resultValue.Should().HaveCount(expected);
        }

        [Fact]
        public void CreateProduct_With_Id_Should_Return_BadRequest()
        {
            //arrange
            var contextMock = new Mock<IShopContext>();
            var repository = new ProductRepository(contextMock.Object);
            var service = new ProductService(repository);
            var controller = new ProductController(service);

            var dto = new ProductDTO { Id = Guid.NewGuid() };

            //act
            var result = controller.Create(dto);

            //assert
            var badRequestObjectResult = result.Result as BadRequestResult;
            badRequestObjectResult.Should().NotBeNull();
        }

        [Fact]
        public void CreateProduct_Should_Create_And_Add_New_Product()
        {
            //arrange
            var contextMock = new Mock<IShopContext>();
            var mockSet = new Mock<DbSet<Product>>();
            var repository = new ProductRepository(contextMock.Object);
            var service = new ProductService(repository);
            var controller = new ProductController(service);

            var dto = new ProductDTO { Name = "test", Description = "test", Price = 123 };

            mockSet.Setup(s => s.Add(It.Is<Product>(p => p.Name.Equals("test") && p.Description.Equals("test") && p.Price.Equals(123)))).Verifiable();
            contextMock.SetupGet(s => s.Product).Returns(mockSet.Object);
            contextMock.Setup(s => s.SaveChanges()).Verifiable();

            //act
            var result = controller.Create(dto);

            //assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            var resultValue = okObjectResult.Value as Guid?;
            resultValue.Should().NotBeNull();
            contextMock.Verify();
            mockSet.Verify();
        }

        [Fact]
        public void UodateProduct_Without_Id_Should_Return_BadRequest()
        {
            //arrange
            var contextMock = new Mock<IShopContext>();
            var repository = new ProductRepository(contextMock.Object);
            var service = new ProductService(repository);
            var controller = new ProductController(service);

            var dto = new ProductDTO();

            //act
            var result = controller.Update(dto);

            //assert
            var badRequestObjectResult = result as BadRequestResult;
            badRequestObjectResult.Should().NotBeNull();
        }

        [Fact]
        public void UpdateProduct_Should_Update_Product()
        {
            //arrange
            var contextMock = new Mock<IShopContext>();
            var mockSet = new Mock<DbSet<Product>>();
            var repository = new ProductRepository(contextMock.Object);
            var service = new ProductService(repository);
            var controller = new ProductController(service);

            var id = Guid.NewGuid();
            var spy = new Product { Id = id, Name = "toBeUpdated", Description = "toBeUpdated", Price = 0 };
            var data = new List<Product> 
            { 
                spy,
                new Product { Id = Guid.NewGuid(), Name = "old", Description = "old", Price = 0 } 
            };
            var dto = new ProductDTO { Id = id, Name = "updated", Description = "test", Price = 123 };

            mockSet.As<IQueryable<Product>>()
                .SetupIQueryable(data.AsQueryable());
            contextMock.SetupGet(s => s.Product).Returns(mockSet.Object);
            contextMock.Setup(s => s.SaveChanges()).Verifiable();

            //act
            var result = controller.Update(dto);

            //assert
            var okObjectResult = result as OkResult;
            okObjectResult.Should().NotBeNull();
            contextMock.Verify();
            spy.Id.Should().Be(id);
            spy.Name.Should().Be(dto.Name);
            spy.Description.Should().Be(dto.Description);
            spy.Price.Should().Be(dto.Price);
        }
    }
}
