using ArchLens.SharedKernel.Application;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Application;

public class ErrorTests
{
    [Fact]
    public void None_ShouldHaveEmptyCodeAndDescription()
    {
        Error.None.Code.Should().BeEmpty();
        Error.None.Description.Should().BeEmpty();
    }

    [Fact]
    public void NullValue_ShouldHaveCorrectCodeAndDescription()
    {
        Error.NullValue.Code.Should().Be("Error.NullValue");
        Error.NullValue.Description.Should().Be("A null value was provided.");
    }

    [Fact]
    public void NotFound_ShouldHaveCorrectCodeAndDescription()
    {
        Error.NotFound.Code.Should().Be("Error.NotFound");
        Error.NotFound.Description.Should().Be("The requested resource was not found.");
    }

    [Fact]
    public void Conflict_ShouldHaveCorrectCodeAndDescription()
    {
        Error.Conflict.Code.Should().Be("Error.Conflict");
        Error.Conflict.Description.Should().Be("A conflict occurred.");
    }

    [Fact]
    public void Validation_ShouldHaveCorrectCodeAndDescription()
    {
        Error.Validation.Code.Should().Be("Error.Validation");
        Error.Validation.Description.Should().Be("A validation error occurred.");
    }

    [Fact]
    public void TwoErrors_WithSameCodeAndDescription_ShouldBeEqual()
    {
        var error1 = new Error("Code", "Desc");
        var error2 = new Error("Code", "Desc");

        error1.Should().Be(error2);
        (error1 == error2).Should().BeTrue();
    }

    [Fact]
    public void TwoErrors_WithDifferentCode_ShouldNotBeEqual()
    {
        var error1 = new Error("Code1", "Desc");
        var error2 = new Error("Code2", "Desc");

        error1.Should().NotBe(error2);
        (error1 != error2).Should().BeTrue();
    }

    [Fact]
    public void TwoErrors_WithDifferentDescription_ShouldNotBeEqual()
    {
        var error1 = new Error("Code", "Desc1");
        var error2 = new Error("Code", "Desc2");

        error1.Should().NotBe(error2);
    }

    [Fact]
    public void Error_GetHashCode_SameErrors_ShouldMatch()
    {
        var error1 = new Error("Code", "Desc");
        var error2 = new Error("Code", "Desc");

        error1.GetHashCode().Should().Be(error2.GetHashCode());
    }
}
