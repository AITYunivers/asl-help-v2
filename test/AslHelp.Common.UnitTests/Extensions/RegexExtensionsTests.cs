using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AslHelp.Common.Extensions;

namespace AslHelp.Common.UnitTests.Extensions;

public class RegexExtensionsTests
{
    [Fact]
    public void Groups_RegexStringInput_ReturnsGroupCollection()
    {
        // Arrange
        Regex regex = new(@"(\w+) (\w+)");
        string input = "Foo Bar Baz";

        // Act
        GroupCollection groups = regex.Groups(input);

        // Assert
        Assert.Equal(3, groups.Count);
        Assert.Equal([input, "Foo", "Bar", "Baz"], groups.Cast<Group>().Select(g => g.Value).ToArray());
    }

    [Fact]
    public void Groups_RegexStringInputStartIndex_ReturnsGroupCollection()
    {
        // Arrange
        Regex regex = new(@"(\w+) (\w+)");
        string input = "Foo Bar Baz";

        int startIndex = 4;

        // Act
        GroupCollection groups = regex.Groups(input, startIndex);

        // Assert
        Assert.Equal(3, groups.Count);
        Assert.Equal(groups.Cast<string>(), [input, "Bar", "Baz"]);
    }

    [Fact]
    public void Groups_RegexStringInputStartIndexLength_ReturnsGroupCollection()
    {
        // Arrange
        Regex regex = new(@"(\w+) (\w+)");
        string input = "Foo Bar Baz";

        int startIndex = 5;
        int length = 5;

        // Act
        GroupCollection groups = regex.Groups(input, startIndex, length);

        // Assert
        Assert.Equal(2, groups.Count);
        Assert.Equal(groups.Cast<string>(), [input, "ar", "Ba"]);
    }
}
