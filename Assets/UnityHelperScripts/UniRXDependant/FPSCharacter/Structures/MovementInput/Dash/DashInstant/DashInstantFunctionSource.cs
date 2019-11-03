using SUHScripts.Functional;
using System;
using static SUHScripts.Functional.Functional;

[System.Serializable]
public class DashInstantFunctionSource : DashInstant
{
    private Func<float> _boostSpeed;
    private Func<int> _maxBoosts;

    public override float BoostSpeed => _boostSpeed();
    public override Option<int> MaxBoosts => GetMaxBoosts();

    public DashInstantFunctionSource(Func<float> boostSpeed, Func<int> maxBoosts)
    {
        _boostSpeed = boostSpeed;
        _maxBoosts = maxBoosts;
    }

    Option<int> GetMaxBoosts()
    {
        var max = _maxBoosts();
        return max < 0 ? NONE : max.AsOption();
    }
}
