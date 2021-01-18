using System;

namespace RXUI
{
    public static class SingleSelectableOperations
    {
        public static Func<T, T, bool> ClassComparer<T>() where T : class => (_0, _1) => _0 == _1;
    }


}

