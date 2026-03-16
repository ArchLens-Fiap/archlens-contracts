using ArchLens.SharedKernel.Application;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Application;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        var error = new Error("Test.Error", "Something went wrong");

        var result = Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Success_WithErrorNone_ShouldNotThrow()
    {
        var act = () => Result.Success();
        act.Should().NotThrow();
    }

    [Fact]
    public void Failure_WithErrorNone_ShouldThrow()
    {
        var act = () => Result.Failure(Error.None);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Failure result must have an error.");
    }

    [Fact]
    public void GenericSuccess_ShouldContainValue()
    {
        var result = Result.Success(42);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void GenericFailure_ShouldNotAllowValueAccess()
    {
        var error = new Error("Test.Error", "Failed");
        var result = Result.Failure<int>(error);

        result.IsFailure.Should().BeTrue();
        var act = () => result.Value;
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot access value of a failed result.");
    }

    [Fact]
    public void GenericSuccess_WithStringValue_ShouldContainValue()
    {
        var result = Result.Success("hello");

        result.Value.Should().Be("hello");
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldCreateSuccessResult()
    {
        Result<string> result = "implicit value";

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("implicit value");
    }

    [Fact]
    public void ImplicitConversion_FromInt_ShouldCreateSuccessResult()
    {
        Result<int> result = 99;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(99);
    }

    [Fact]
    public void GenericFailure_ShouldHaveCorrectError()
    {
        var error = new Error("Custom.Code", "Custom description");
        var result = Result.Failure<string>(error);

        result.Error.Should().Be(error);
        result.Error.Code.Should().Be("Custom.Code");
        result.Error.Description.Should().Be("Custom description");
    }
}
