using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    using Functional;
    public static class ArcDataSystem
    {
        public static ArcInput ExtractArcInput(this Transform @this, Vector3 gravity) =>
            new ArcInput(@this.forward, @this.right, @this.position, gravity);

        public static ArcCalcSet CalcVelocity(this ArcSettings @this, ArcInput input) =>
            (Quaternion.AngleAxis(-@this.Angle, input.RightVector) * input.ForwardVector * @this.Strength)
            .MapInto(velocity => new ArcCalcSet(input.Position, velocity));

        public static ArcCalcSet CalcVelocity(this ArcInput @this, ArcSettings input) =>
            CalcVelocity(input, @this);

        public static ArcCalcSet GetVertexPoint(ArcSettings settings, ArcInput input, ArcCalcSet set)
        {
            Vector3 position1 =
                set.position + set.velocity * settings.VertexDelta + 0.5f
                * input.Gravity * settings.VertexDelta * settings.VertexDelta;

            Vector3 newVelocity = set.velocity + input.Gravity * settings.VertexDelta;
            return new ArcCalcSet(position1, newVelocity);
        }

        public static IEnumerable<Vector3> CalculateArcPositions(this ArcInput input, ArcSettings settings, Func<Vector3, Vector3, bool> takeUntil) =>
            CalculateArcPositions(settings, input, takeUntil);

        public static IEnumerable<Vector3> CalculateArcPositions(ArcSettings settings, ArcInput input, Func<Vector3, Vector3, bool> takeUntil, bool isExclusive = false)
        {
            ArcCalcSet sourceSet = ArcDataSystem.CalcVelocity(settings, input);

            yield return sourceSet.position;

            bool shouldBreak = false;

            for (int i = 0; i < settings.MaxVertexCount; i++)
            {
                if (shouldBreak)
                    yield break;

                ArcCalcSet newSet = GetVertexPoint(settings, input, sourceSet);

                shouldBreak = takeUntil(sourceSet.position, newSet.position);

                if (shouldBreak && isExclusive)
                    yield break;

                yield return newSet.position;

                sourceSet = newSet;
            }
        }


    }
}
