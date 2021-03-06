using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;
using Xunit.Abstractions;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests{
    private readonly ITestOutputHelper output;

    public NavigationMenuViewComponentTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Indicates_Selected_Category()
    {
        // Arrange
        string categoryToSelect = "Apples";
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns(
            (new Product[]{
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P2", Category = "Oranges"},
            }).AsQueryable<Product>()
        );
    
        NavigationMenuViewComponent target = new(mock.Object);

        target.ViewComponentContext = new ViewComponentContext{
            ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext{
                RouteData = new RouteData(),
            }
        };

        target.RouteData.Values["category"] = categoryToSelect;

        // Action
        string? result = (string?)(target.Invoke() as ViewViewComponentResult)?.ViewData?["SelectedCategory"];
    
        // Assert
        Assert.Equal(categoryToSelect, result);
    }

    [Fact]
    public void Can_Select_Categories()
    {
        // Arrange
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1",
                Category = "Apples"},
                new Product {ProductID = 2, Name = "P2",
                Category = "Apples"},
                new Product {ProductID = 3, Name = "P3",
                Category = "Plums"},
                new Product {ProductID = 4, Name = "P4",
                Category = "Oranges"},
                }).AsQueryable<Product>());
            

        NavigationMenuViewComponent target = new(mock.Object);
        
        // Action
        string[] results = ((IEnumerable<string>?)
                                (target.Invoke() as ViewViewComponentResult)?.ViewData?.Model
                                    ?? Enumerable.Empty<string>())
                                    .ToArray();
        // Assert
        Assert.True(Enumerable.SequenceEqual(new string[]{
            "Apples", "Oranges", "Plums"
        }, results));
    }
}