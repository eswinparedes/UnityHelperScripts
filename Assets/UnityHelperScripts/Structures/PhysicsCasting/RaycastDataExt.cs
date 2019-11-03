using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;

public static class RaycastDataExt
{
    public static Option<T> GetComponentOption<T>(this RaycastData @this)
    {
        if (@this.RaycastHitOption.IsSome)
            return @this.RaycastHitOption.Value.collider.gameObject.GetComponent<T>().AsOption();
        else
            return NONE;
    }
    public static Option<T> QueryComponentOption<T>(this RaycastData @this, bool useGetComponent = true)
    {
        if (@this.RaycastHitOption.IsSome)
            return @this.RaycastHitOption.Value.collider.gameObject.QueryComponentOption<T>(useGetComponent);
        else return NONE;
    }
}
