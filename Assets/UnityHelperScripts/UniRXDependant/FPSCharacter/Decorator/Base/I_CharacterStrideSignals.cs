using UniRx;
using System;

public interface I_CharacterStrideSignals
{
    IObservable<Unit> StrideLogged { get; }
    IObservable<float> StrideAlphaUpdate { get; }
    float StrideLength { get; }
}