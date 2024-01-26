using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.RiskEngine.Symbols;
using Vertr.Exchange.Domain.Common.Binary.Commands;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.RiskEngine.Binary;

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
