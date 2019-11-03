using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_TransformShakeSubscriber
{
    void AddShakeEvent(SO_A_NoiseData data);
    void AddShakeEvent(SO_TransfromShakeData data);
}
