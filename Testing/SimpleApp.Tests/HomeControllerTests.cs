using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleApp.Controllers;
using SimpleApp.Models;
using Xunit;
namespace SimpleApp.Tests;

public class HomeConrollerTests{

    private class FakeDataSource : IDataSource
    {
        public FakeDataSource(Product[] data) => Products = data;

        public IEnumerable<Product> Products { get;set; }
    }

    [Fact]
    public void IndexActionModelIsComplete(){
        // Arrange
        Product[] testData = new Product[]{
            new Product{ Name = "P1", Price = 75.10M },
            new Product{ Name = "P2", Price = 120M },
            new Product{ Name = "3", Price = 110M },
        };
        IDataSource data = new FakeDataSource(testData);
        var controller = new HomeController();
        controller.dataSource = data;
        // Act
        var model = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;
    
        // Assert
        Assert.Equal(data.Products, model, 
            MyComparer.Get<Product>((p1, p2) => p1?.Name == p2?.Name && p1?.Price == p2?.Price));
    }
}