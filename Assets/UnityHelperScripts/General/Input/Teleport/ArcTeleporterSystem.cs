using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MathHelpers.MathHelper;

public static class ArcTeleporterSystem 
{
    /// <summary>
    /// Returns data that holds a rotation that alignes camera rotation to marker
    /// </summary>
    /// <param name="marker"></param>
    /// <param name="camera"></param>
    /// <param name="root"></param>
    /// <returns></returns>
    public static TransformData GetRotationOffset(Transform marker, Transform camera, Transform root)
    {
        //Figure offset for camera and move 
        Vector3 camForwardRelativeToTarget = OrthoNormalVector(camera.forward, marker.up);
        float angle = AngleOffAroundAxisDegrees(camForwardRelativeToTarget, marker.forward, Vector3.up);

        Quaternion rotation = root.rotation * Quaternion.Euler(Vector3.up * -angle);

        return new TransformData(marker.position, rotation, root.localScale);
    }
}
