namespace SUHScripts
{
    [System.Serializable]
    public struct BoolTrifecta
    {
        public BoolTrifecta(bool isTrueThisFrame, bool isTrueStay, bool isfalseThisFrame)
        {
            IsTrueThisFrame = isTrueThisFrame;
            IsTrueStay = isTrueStay;
            IsFalseThisFrame = isfalseThisFrame;
        }
        public bool IsTrueThisFrame { get; private set; }
        public bool IsTrueStay { get; private set; }
        public bool IsFalseThisFrame { get; private set; }

        public (bool isTrueThisFrame, bool isTrueStay, bool isFalseThisFrame) Deconstruct() =>
            (IsTrueThisFrame, IsTrueStay, IsFalseThisFrame);

        public override string ToString() =>
            $"IsTrueThisFrame: {IsTrueThisFrame}  IsTrueStay: {IsTrueStay}  IsFalseThisFrame{IsFalseThisFrame}";

    }
}
