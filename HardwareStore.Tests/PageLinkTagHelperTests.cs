using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using HardwareStore.Controllers;
using HardwareStore.Models;
using HardwareStore.Models.ViewModels;
using HardwareStore.Infrastructure;

namespace HardwareStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void CanGeneratePageLinks()
        {
            Mock<IUrlHelper> urlHelper = new();
            urlHelper
                .SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/page1")
                .Returns("Test/page2")
                .Returns("Test/page3");
            Mock<IUrlHelperFactory> urlHelperFactory = new();
            urlHelperFactory
                .Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);

            PageLinkTagHelper helper = new(urlHelperFactory.Object)
            {   
                PageModel = new PaginingInfo()
                {
                    CurrentPage = 2, 
                    TotalItems = 28, 
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };
            TagHelperContext context = new(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "");
            Mock<TagHelperContent> content = new();
            TagHelperOutput output = new("div",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));

            helper.Process(context, output);

            Assert.Equal(output.Content.GetContent(),
                @"<a href=""Test/page1"">1</a>" +
                @"<a href=""Test/page2"">2</a>" +
                @"<a href=""Test/page3"">3</a>");
        }
    }
}
