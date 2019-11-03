using SUHScripts.Functional;

[System.Serializable]
public abstract class DashInstant
{
    public abstract float BoostSpeed { get; }
    public abstract Option<int> MaxBoosts { get; }
}