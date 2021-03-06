﻿
namespace SUHScripts
{
    using Functional;
    using static Functional.Functional;
    public static class RaycastDataExt
    {
        public static Option<T> GetComponentOption<T>(this RaycastData @this)
        {
            if (@this.RaycastHitOption.IsSome)
                return @this.RaycastHitOption.Value.collider.gameObject.GetComponent<T>().AsOption_SAFE();
            else
                return None.Default;
        }
        public static Option<T> QueryComponentOption<T>(this RaycastData @this, bool useGetComponent = true)
        {
            if (@this.RaycastHitOption.IsSome)
                return @this.RaycastHitOption.Value.collider.gameObject.QueryComponentOption<T>(useGetComponent);
            else return None.Default;
        }
    }
}

