using System;

using AslHelp.Common.Extensions;

namespace AslHelp.Common.UnitTests.Extensions;

public class CharExtensions_ToHexValue
{
    [Theory]
    [InlineData('0', 0x00)]
    [InlineData('1', 0x01)]
    [InlineData('2', 0x02)]
    [InlineData('3', 0x03)]
    [InlineData('4', 0x04)]
    [InlineData('5', 0x05)]
    [InlineData('6', 0x06)]
    [InlineData('7', 0x07)]
    [InlineData('8', 0x08)]
    [InlineData('9', 0x09)]
    [InlineData('a', 0x0A)]
    [InlineData('b', 0x0B)]
    [InlineData('c', 0x0C)]
    [InlineData('d', 0x0D)]
    [InlineData('e', 0x0E)]
    [InlineData('f', 0x0F)]
    [InlineData('A', 0x0A)]
    [InlineData('B', 0x0B)]
    [InlineData('C', 0x0C)]
    [InlineData('D', 0x0D)]
    [InlineData('E', 0x0E)]
    [InlineData('F', 0x0F)]
    public void ToHexValue_Returns_ConvertedValue(char c, byte expected)
    {
        byte actual = c.ToHexValue();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData('.')]
    [InlineData(' ')]
    [InlineData('g')]
    public void ToHexValue_Returns_0xFF(char c)
    {
        byte actual = c.ToHexValue();
        Assert.Equal(0xFF, actual);
    }
}
