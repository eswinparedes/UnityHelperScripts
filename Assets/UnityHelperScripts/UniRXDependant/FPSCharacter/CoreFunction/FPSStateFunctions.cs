using System;
using UnityEngine;
public static class FPSStatefulFunctions
{
    public static Func<Vector2, Quaternion> StandardCameraUpdateMovement_SideEffecting(
        FPSRoot fpsRoot, Vector3 mask)
    {
        TransformIdentityMaintainer maintainer = new TransformIdentityMaintainer();
        return
            input =>
            {
                fpsRoot.FPSCamera.ApplyOffset();

                var localRot = fpsRoot.FPSCamera.RootTransform.localRotation;
                var rot = 
                    FPSCoreMovementSystem.GetRotationFromInputAxis(input, fpsRoot.FPSCamera.FPSCameraSettings, localRot);

                fpsRoot.FPSCamera.RootTransform.localRotation = rot;

                using (maintainer.MaintainIdentityOn(fpsRoot.FPSCamera.RootTransform))
                {
                    var newRot =
                        fpsRoot.FPSCamera.RootTransform.rotation.MaskedRotation(mask);

                    fpsRoot.Character.transform.rotation = newRot;
                }

                return fpsRoot.FPSCamera.RootTransform.rotation;
            };
    }
}
