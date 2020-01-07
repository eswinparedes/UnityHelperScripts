namespace SUHScripts
{
    public static class BoolTrifectaExtensions 
    {
        public static BoolTrifecta GetUpdateFromInput(this BoolTrifecta lastState, bool input)
        {
            bool thisFrame = input && !lastState.IsTrueThisFrame && !lastState.IsTrueStay;
            bool stay = input && !thisFrame;
            bool falseThisFrame = !input && !lastState.IsFalseThisFrame && (lastState.IsTrueThisFrame | lastState.IsTrueStay);
            return new BoolTrifecta(input, thisFrame, stay, falseThisFrame);
        }

        public static BoolTrifecta AsTrueThisFrame(this BoolTrifecta @this) =>
            new BoolTrifecta(true, true, false, false);

        public static BoolTrifecta AsTrueStay(this BoolTrifecta @this) =>
            new BoolTrifecta(true, false, true, false);

        public static BoolTrifecta AsFalseThisFrame(this BoolTrifecta @this) =>
            new BoolTrifecta(false, false, false, true);
    }
}

