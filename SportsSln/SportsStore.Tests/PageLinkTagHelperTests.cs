using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace SportsStore.Tests;

public class PageLinkTagHelperTests{
    private readonly ITestOutputHelper xUnitOutput;

    public PageLinkTagHelperTests(ITestOutputHelper xUnitOutput)
    {
        this.xUnitOutput = xUnitOutput;
    }

    [Fact]
    public void TestName()
    {
        // Given
        var urlHelper = new Mock<IUrlHelper>();
        urlHelper.SetupSequence(x => x.Action(
            It.IsAny<UrlActionContext>()
        )).Returns("Test/Page1").Returns("Test/Page2").Returns("Test/Page3");

        var urlHelperFactory = new Mock<IUrlHelperFactory>();

        urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
                        .Returns(urlHelper.Object);
        
        var viewContext = new Mock<ViewContext>();
        
        PageLinkTagHelper helper = new(urlHelperFactory.Object)
        {
            PageModel = new(){
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            },
            ViewContext = viewContext.Object,
            PageAction = "Test"
        };
        TagHelperContext ctx = new(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(), ""
        );

        var content = new Mock<TagHelperContent>();
        TagHelperOutput output = new TagHelperOutput("div",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult(content.Object));
        
        // Act
        helper.Process(ctx, output);


        xUnitOutput.WriteLine(output.Content.GetContent());
        // Assert
        Assert.Equal(@"<a href=""Test/Page1"">1</a><a href=""Test/Page2"">2</a><a href=""Test/Page3"">3</a>",
                        output.Content.GetContent());
    }
}