using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public TransformData PlayerRootData { get; }
    public TransformData PlayerViewData { get; }

    public PlayerData(TransformData playerRootData, TransformData playerViewData)
    {
        this.PlayerRootData = playerRootData;
        this.PlayerViewData = playerViewData;
    }
}
