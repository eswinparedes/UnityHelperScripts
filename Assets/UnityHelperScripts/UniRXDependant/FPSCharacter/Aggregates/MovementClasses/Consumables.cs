using System;

public interface Consumable
{

}

public class ConsumableTime : Consumable
{
    public float TimeLeft { get; }
    public float timeMax { get; }

    public ConsumableTime(float timeLeft, float maxTime)
    {
        this.TimeLeft = TimeLeft;
        this.timeMax = timeMax;
    }
}

public class ConsumableInteger : Consumable
{
    public int UnitsLeft { get; }
    public int UnitsMax { get; }

    public ConsumableInteger(int unitsLeft, int maxUnits)
    {
        this.UnitsMax = UnitsMax;
        this.UnitsLeft = UnitsLeft;
    }
}

public class ConsumableFloat : Consumable
{
    public float AmountLeft { get; }
    public float AmountMax { get; }

    public ConsumableFloat(float amountLeft, float maxAmount)
    {
        this.AmountLeft = amountLeft;
        this.AmountMax = maxAmount;
    }
}

public static class ConsumableExtensions
{
    public static T Match<T>(this Consumable @this,
        Func<ConsumableTime, T> onConsumableTime,
        Func<ConsumableInteger, T> onConsumableInteger,
        Func<ConsumableFloat, T> onConsumableFloat)
    {
        switch (@this)
        {
            case ConsumableTime ct: return onConsumableTime(ct);
            case ConsumableInteger ci: return onConsumableInteger(ci);
            case ConsumableFloat cf: return onConsumableFloat(cf);
            default: throw new Exception("Pattern match not complete");
        }
    }

    public static void For(this Consumable @this,
        Action<ConsumableTime> onConsumableTime = null,
        Action<ConsumableInteger> onConsumableInteger = null,
        Action<ConsumableFloat> onConsumableFloat = null)
    {
        switch (@this)
        {
            case ConsumableTime ct: onConsumableTime?.Invoke(ct); break;
            case ConsumableInteger ci: onConsumableInteger?.Invoke(ci); break;
            case ConsumableFloat cf: onConsumableFloat?.Invoke(cf); break;
            default: throw new Exception("Pattern match not complete");
        }
    }

    public static ConsumableFloat With(this ConsumableFloat @this, float? amountLeft, float? maxAmount) =>
        new ConsumableFloat(amountLeft ?? @this.AmountLeft, maxAmount ?? @this.AmountMax);

    public static ConsumableInteger With(this ConsumableInteger @this, int? unitsLeft, int? UnitsMax) =>
        new ConsumableInteger(unitsLeft ?? @this.UnitsLeft, UnitsMax ?? @this.UnitsMax);

    public static ConsumableTime With(this ConsumableTime @this, float? timeLeft, float? timeMax) =>
        new ConsumableTime(timeLeft ?? @this.TimeLeft, timeMax ?? @this.timeMax);
}
