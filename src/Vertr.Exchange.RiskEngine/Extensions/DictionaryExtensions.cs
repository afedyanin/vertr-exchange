namespace Vertr.Exchange.RiskEngine.Extensions;

internal static class DictionaryExtensions
{
    public static decimal AddToValue(this IDictionary<int, decimal> dict, int key, decimal toBeAdded)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, toBeAdded);
            return toBeAdded;
        }
        else
        {
            dict[key] += toBeAdded;
            return dict[key];
        }
    }
}
