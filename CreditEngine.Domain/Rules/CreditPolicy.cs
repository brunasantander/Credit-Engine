namespace CreditEngine.Domain.Rules;

public static class CreditPolicy
{
    public static bool IsEligible(decimal annualRevenue, decimal ebitda, decimal leverage)
    {
        if (annualRevenue < 5_000_000) return false;
        if (ebitda <= 0) return false;
        if (leverage > 3) return false;

        return true;
    }
}