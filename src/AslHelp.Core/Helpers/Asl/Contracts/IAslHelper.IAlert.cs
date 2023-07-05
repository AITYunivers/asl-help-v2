namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    public interface IAlert
    {
        IAslHelper AlertRealTime(string? message = null);
        IAslHelper AlertGameTime(string? message = null);
        IAslHelper AlertLoadless(string? message = null);
    }
}
