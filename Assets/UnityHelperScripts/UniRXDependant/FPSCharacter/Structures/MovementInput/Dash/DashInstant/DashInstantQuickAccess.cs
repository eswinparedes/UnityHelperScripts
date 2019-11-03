using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;

[System.Serializable]
public class DashInstantQuickAccess : DashInstant
{
    public float boostSpeed = 5;
    public int maxBoosts = 1;

    public override float BoostSpeed => boostSpeed;
    public override Option<int> MaxBoosts => maxBoosts < 0 ? NONE : maxBoosts.AsOption();
}
