using FindFun.Server.Domain;
using FluentAssertions;
using Xunit;

namespace FindFund.Server.UnitTest;

public class ImageTests
{
    private class TestImage : Image
    {
        public TestImage(string url) : base(url) { }
    }

    [Fact]
    public void Image_ShouldSetUrl_WhenDerivedConstructed()
    {
        // Act
        var img = new TestImage("https://example.com/test.jpg");

        // Assert
        img.Should().NotBeNull();
        img.Url.Should().Be("https://example.com/test.jpg");
    }
}
