using static SUHScripts.Functional.GenericExtensions;

public static class IndexTravelerExtensions 
{
    public static int CircleLength(int direction, int length) => direction < 0 ? length - 1 : 0;
    public static bool IndexIsOutOfRange(int index, int length) => index < 0 || index >= length;

    public static IndexCycle CyclePingPong(this IndexCycle i, int length)
    {
        int next = i.Index + i.Direction;
        int dir = IndexIsOutOfRange(next, length) ? i.Direction * -1 : i.Direction;
        int index = i.Index + dir;
        return new IndexCycle(index, dir);
    }

    public static IndexCycle CycleCircular(this IndexCycle i, int length)
    {
        int next = i.Index + i.Direction;
        int index = IndexIsOutOfRange(next, length) ? CircleLength(i.Direction, length) : next;
        return new IndexCycle(index, i.Direction);
    }

    public static IndexCycle CycleNext(this IndexCycle i, int length, bool isCircular = true)
    {
        return isCircular ? CycleCircular(i, length) : CyclePingPong(i, length);
    }

    public static IndexCycle WithDirection(this IndexCycle @this, int dir) =>
        new IndexCycle(@this.Index, dir);

    public static IndexCycle WithIndex(this IndexCycle @this, int index) =>
        new IndexCycle(index, @this.Direction);
}
