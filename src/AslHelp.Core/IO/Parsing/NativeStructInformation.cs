namespace AslHelp.Core.IO.Parsing;

public class NativeStructInformation
{
    private record Inheritance(
        string Major,
        string Minor);

    private record Signature(
        string Name,
        int Offset,
        string Pattern);
}
