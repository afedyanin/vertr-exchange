using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Symbols;

namespace Vertr.Exchange.RiskEngine.Binary;

internal static class CommandExtensions
{
    public static CommandResultCode HandleCommand(
        this BatchAddSymbolsCommand command,
        ISymbolSpecificationProvider symbolSpecificationProvider)
    {
        return symbolSpecificationProvider.AddSymbols(command.Symbols);
    }

    public static CommandResultCode HandleCommand(
        this BatchAddAccountsCommand command,
        IUserProfileProvider userProfiles)
    {
        return userProfiles.BatchAdd(command.Users);
    }
}
