using System.Linq;
using System.Reflection;

using AslHelp.Common.Extensions;

namespace AslHelp.Common.UnitTests.Extensions;

public class ReflectionExtensionsTests
{
    private const string Expected = "Hello, world!";

    [Fact]
    public void GetFieldValue_ReturnsValueIfFieldExists()
    {
        // Arrange
        TestClass obj = new(Expected);

        // Act
        string? actual = obj.GetFieldValue<string>(nameof(TestClass.Field));

        // Assert
        Assert.Equal(Expected, actual);
    }

    [Fact]
    public void GetFieldValue_ReturnsNullIfFieldDoesNotExist()
    {
        // Arrange
        TestClass obj = new();

        // Act
        string? actual = obj.GetFieldValue<string>(nameof(TestClass.Field)[1..]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void SetFieldValue_SetsValue()
    {
        // Arrange
        TestClass obj = new();

        // Act
        obj.SetFieldValue(nameof(TestClass.Field), Expected);

        // Assert
        Assert.Equal(Expected, obj.Field);
    }

    [Fact]
    public void GetPropertyValue_ReturnsValueIfPropertyExists()
    {
        // Arrange
        TestClass obj = new(Expected);

        // Act
        string? actual = obj.GetPropertyValue<string>(nameof(TestClass.Property));

        // Assert
        Assert.Equal(Expected, actual);
    }

    [Fact]
    public void GetPropertyValue_ReturnsNullIfPropertyDoesNotExist()
    {
        // Arrange
        TestClass obj = new();

        // Act
        string? actual = obj.GetPropertyValue<string>(nameof(TestClass.Property)[1..]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void SetPropertyValue_SetsValue()
    {
        // Arrange
        TestClass obj = new();

        // Act
        obj.SetPropertyValue(nameof(TestClass.Property), Expected);

        // Assert
        Assert.Equal(Expected, obj.Property);
    }

    [Fact]
    public void GetMethod_ReturnsMethodIfMethodExists()
    {
        // Arrange
        TestClass obj = new();

        // Act
        MethodInfo? actual = obj.GetMethod(nameof(TestClass.Method));

        // Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public void GetMethod_ReturnsNullIfMethodDoesNotExist()
    {
        // Arrange
        TestClass obj = new();

        // Act
        MethodInfo? actual = obj.GetMethod(nameof(TestClass.Method)[1..]);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void IsType_ReturnsTrueIfTypeIsType()
    {
        // Arrange
        TestClass obj = new();

        // Act
        bool actual = obj.IsType<TestClass>();

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsType_ReturnsFalseIfTypeIsNotType()
    {
        // Arrange
        TestClass obj = new();

        // Act
        bool actual = obj.IsType<string>();

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void CallingAssembly_ReturnsCallingAssembly()
    {
        // Arrange
        Assembly expected = typeof(ReflectionExtensionsTests).Assembly;

        // Act
        Assembly actual = ReflectionExtensions.AssemblyTrace.Skip(1).First();

        // Assert
        Assert.Equal(expected, actual);
    }

    private class TestClass(string? value = default)
    {
        public string? Field = value;
        public string? Property { get; set; } = value;
        public void Method() { }
    }
}
