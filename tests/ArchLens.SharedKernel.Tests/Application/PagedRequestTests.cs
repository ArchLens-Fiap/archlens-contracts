using ArchLens.SharedKernel.Application;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Application;

public class PagedRequestTests
{
    [Fact]
    public void DefaultValues_ShouldBePage1AndPageSize20()
    {
        var request = new PagedRequest();

        request.Page.Should().Be(1);
        request.PageSize.Should().Be(20);
    }

    [Fact]
    public void CustomValues_ShouldBePreserved()
    {
        var request = new PagedRequest(3, 50);

        request.Page.Should().Be(3);
        request.PageSize.Should().Be(50);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void PageLessThan1_ShouldBeClampedTo1(int page)
    {
        var request = new PagedRequest(page, 20);

        request.Page.Should().Be(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    public void PageSizeLessThan1_ShouldBeClampedTo20(int pageSize)
    {
        var request = new PagedRequest(1, pageSize);

        request.PageSize.Should().Be(20);
    }

    [Theory]
    [InlineData(101)]
    [InlineData(200)]
    [InlineData(1000)]
    public void PageSizeGreaterThan100_ShouldBeClampedTo100(int pageSize)
    {
        var request = new PagedRequest(1, pageSize);

        request.PageSize.Should().Be(100);
    }

    [Fact]
    public void PageSize100_ShouldNotBeClamped()
    {
        var request = new PagedRequest(1, 100);

        request.PageSize.Should().Be(100);
    }

    [Fact]
    public void PageSize1_ShouldNotBeClamped()
    {
        var request = new PagedRequest(1, 1);

        request.PageSize.Should().Be(1);
    }

    [Theory]
    [InlineData(1, 20, 0)]
    [InlineData(2, 20, 20)]
    [InlineData(3, 10, 20)]
    [InlineData(5, 50, 200)]
    public void Skip_ShouldBeCalculatedCorrectly(int page, int pageSize, int expectedSkip)
    {
        var request = new PagedRequest(page, pageSize);

        request.Skip.Should().Be(expectedSkip);
    }
}
