using ArchLens.SharedKernel.Domain;
using FluentAssertions;

namespace ArchLens.SharedKernel.Tests.Domain;

public class DomainExceptionTests
{
    private sealed class TestDomainException : DomainException
    {
        public TestDomainException(string code, string message)
            : base(code, message) { }

        public TestDomainException(string code, string message, Exception innerException)
            : base(code, message, innerException) { }
    }

    [Fact]
    public void Constructor_ShouldSetCodeAndMessage()
    {
        var exception = new TestDomainException("ERR001", "Something went wrong");

        exception.Code.Should().Be("ERR001");
        exception.Message.Should().Be("Something went wrong");
    }

    [Fact]
    public void Constructor_WithInnerException_ShouldSetAllProperties()
    {
        var inner = new InvalidOperationException("inner error");
        var exception = new TestDomainException("ERR002", "Outer error", inner);

        exception.Code.Should().Be("ERR002");
        exception.Message.Should().Be("Outer error");
        exception.InnerException.Should().Be(inner);
    }

    [Fact]
    public void DomainException_ShouldInheritFromException()
    {
        var exception = new TestDomainException("ERR", "msg");

        exception.Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public void DomainException_ShouldBeCatchableAsException()
    {
        Action act = () => throw new TestDomainException("ERR", "boom");

        act.Should().Throw<DomainException>()
            .Where(e => e.Code == "ERR" && e.Message == "boom");
    }

    [Fact]
    public void DomainException_InnerException_ShouldBeNull_WhenNotProvided()
    {
        var exception = new TestDomainException("ERR", "msg");

        exception.InnerException.Should().BeNull();
    }
}
