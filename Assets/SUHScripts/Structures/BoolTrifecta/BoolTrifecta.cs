namespace SUHScripts
{
    [System.Serializable]
    public partial struct BoolTrifecta
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

        public static bool operator ==(BoolTrifecta lhs, BoolTrifecta rhs)
        {
            return
            lhs.IsFalseThisFrame == rhs.IsFalseThisFrame &&
            lhs.IsTrueThisFrame == rhs.IsTrueThisFrame &&
            lhs.IsTrueStay == rhs.IsTrueStay;
        }

        public static bool operator != (BoolTrifecta lhs, BoolTrifecta rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is BoolTrifecta bt && (bt == this);
        }

        public override int GetHashCode()
        {
            var hashCode = -419216277;
            hashCode = hashCode * -1521134295 + IsTrueThisFrame.GetHashCode();
            hashCode = hashCode * -1521134295 + IsTrueStay.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFalseThisFrame.GetHashCode();
            return hashCode;
        }

        public static BoolTrifecta TrueThisFrame = new BoolTrifecta(true, false, false);
        public static BoolTrifecta FalseThisFrame = new BoolTrifecta(false, false, true);
        public static BoolTrifecta TrueStay = new BoolTrifecta(false, true, false);
        public static BoolTrifecta False = default;

    }
}
