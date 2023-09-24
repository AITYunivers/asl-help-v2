using System.Text.RegularExpressions;

using AslHelp.Common.Extensions;

namespace AslHelp.Common.UnitTests.Extensions;

public class RegexExtensionsTests
{
    [Fact]
    public void Groups_RegexStringInput_ReturnsGroupCollection()
    {
        // Arrange
        Regex regex = new(@"(?<name>\w+)");
        string input = "Hello World";

        // Act
        var result = regex.Groups(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Hello", result[0].Value);
        Assert.Equal("World", result[1].Value);
    }

    [Fact]
    public void Groups_RegexStringInputStartIndex_ReturnsGroupCollection()
    {
        // Arrange
        var regex = new Regex(@"(?<name>\w+)");
        var input = "Hello World";
        var startIndex = 6;

        // Act
        var result = regex.Groups(input, startIndex);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("World", result[0].Value);
        Assert.Equal("", result[1].Value);
    }

    [Fact]
    public void Groups_RegexStringInputStartIndexLength_ReturnsGroupCollection()
    {
        // Arrange
        var regex = new Regex(@"(?<name>\w+)");
        var input = "Hello World";
        var startIndex = 6;
        var length = 5;

        // Act
        var result = regex.Groups(input, startIndex, length);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("World", result[0].Value);
        Assert.Equal("", result[1].Value);
    }
}
