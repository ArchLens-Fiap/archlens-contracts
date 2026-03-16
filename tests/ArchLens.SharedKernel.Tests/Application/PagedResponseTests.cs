using ArchLens.SharedKernel.Application;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Application;

public class PagedResponseTests
{
    [Fact]
    public void TotalPages_ShouldCeilDivision()
    {
        var response = new PagedResponse<int>([1, 2, 3], Page: 1, PageSize: 10, TotalCount: 25);

        response.TotalPages.Should().Be(3);
    }

    [Fact]
    public void TotalPages_ExactDivision_ShouldNotRoundUp()
    {
        var response = new PagedResponse<int>([1, 2], Page: 1, PageSize: 5, TotalCount: 10);

        response.TotalPages.Should().Be(2);
    }

    [Fact]
    public void TotalPages_ZeroTotalCount_ShouldBeZero()
    {
        var response = new PagedResponse<string>([], Page: 1, PageSize: 20, TotalCount: 0);

        response.TotalPages.Should().Be(0);
    }

    [Fact]
    public void HasPrevious_OnFirstPage_ShouldBeFalse()
    {
        var response = new PagedResponse<int>([1], Page: 1, PageSize: 10, TotalCount: 50);

        response.HasPrevious.Should().BeFalse();
    }

    [Fact]
    public void HasPrevious_OnSecondPage_ShouldBeTrue()
    {
        var response = new PagedResponse<int>([1], Page: 2, PageSize: 10, TotalCount: 50);

        response.HasPrevious.Should().BeTrue();
    }

    [Fact]
    public void HasNext_OnLastPage_ShouldBeFalse()
    {
        var response = new PagedResponse<int>([1], Page: 5, PageSize: 10, TotalCount: 50);

        response.HasNext.Should().BeFalse();
    }

    [Fact]
    public void HasNext_NotOnLastPage_ShouldBeTrue()
    {
        var response = new PagedResponse<int>([1], Page: 3, PageSize: 10, TotalCount: 50);

        response.HasNext.Should().BeTrue();
    }

    [Fact]
    public void HasNext_OnlyOnePage_ShouldBeFalse()
    {
        var response = new PagedResponse<int>([1], Page: 1, PageSize: 10, TotalCount: 5);

        response.HasNext.Should().BeFalse();
    }

    [Fact]
    public void Items_ShouldContainProvidedItems()
    {
        var items = new List<string> { "a", "b", "c" };
        var response = new PagedResponse<string>(items, Page: 1, PageSize: 10, TotalCount: 3);

        response.Items.Should().BeEquivalentTo(items);
    }

    [Fact]
    public void Properties_ShouldReflectConstructorValues()
    {
        var response = new PagedResponse<int>([1, 2], Page: 3, PageSize: 15, TotalCount: 100);

        response.Page.Should().Be(3);
        response.PageSize.Should().Be(15);
        response.TotalCount.Should().Be(100);
    }
}
