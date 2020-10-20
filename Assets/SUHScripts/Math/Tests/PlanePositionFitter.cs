using SUHScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePositionFitter : MonoBehaviour
{
    public LayerMask raycastLayers; // The ground layers
    public float scale = 1f; // For resizing the values when the mech is resized
    public Transform body; // The body transform, the root of the legs
    public Transform[] legs; // All the legs of this spider
    public float legRotationWeight = 1f; // The weight of rotating the body to each leg
    public float rootPositionSpeed = 5f; // The speed of positioning the root
    public float rootRotationSpeed = 30f; // The slerp speed of rotating the root to leg heights
    public float breatheSpeed = 2f; // Speed of the breathing cycle
    public float breatheMagnitude = 0.2f; // Magnitude of breathing
    public float height = 3.5f; // Height from ground
    public float minHeight = 2f; // The minimum height from ground
    public float raycastHeight = 10f; // The height of ray origin
    public float raycastDistance = 5f; // The distance of rays (total ray length = raycastHeight + raycastDistance)

    private Vector3 lastPosition;
    private Vector3 defaultBodyLocalPosition;
    private float sine;
    private RaycastHit rootHit;

    void Update()
    {
        // Find the normal of the plane defined by leg positions
        Vector3 legsPlaneNormal = GetPlaneNormal(body, legs, height, legRotationWeight, scale);

        // Rotating the root
        Quaternion fromTo = Quaternion.FromToRotation(body.up, legsPlaneNormal);
        body.rotation = Quaternion.Slerp(body.rotation, fromTo * body.rotation, Time.deltaTime * rootRotationSpeed);

        // Positioning the root
        Vector3 legCentroid = Centroid(legs);
        Vector3 heightOffset = Vector3.Project((legCentroid + body.up * height * scale) - body.position, body.up);
        body.position += heightOffset * Time.deltaTime * (rootPositionSpeed * scale);

        if (Physics.Raycast(body.position + body.up * raycastHeight * scale, -body.up, out rootHit, (raycastHeight * scale) + (raycastDistance * scale), raycastLayers))
        {
            rootHit.distance -= (raycastHeight * scale) + (minHeight * scale);

            if (rootHit.distance < 0f)
            {
                Vector3 targetPosition = body.position - body.up * rootHit.distance;
                body.position = Vector3.Lerp(body.position, targetPosition, Time.deltaTime * rootPositionSpeed * scale);
            }
        }

        return;
        // Update Breathing
        sine += Time.deltaTime * breatheSpeed;
        if (sine >= Mathf.PI * 2f) sine -= Mathf.PI * 2f;
        float br = Mathf.Sin(sine) * breatheMagnitude * scale;

        // Apply breathing
        Vector3 breatheOffset = transform.up * br;
        body.transform.position = transform.position + breatheOffset;
    }

    // Calculate the normal of the plane defined by leg positions, so we know how to rotate the body
    public static Vector3 Centroid(params Transform[] positions)
    {
        return Centroid(positions.Positions());
    }

    public static Vector3 Centroid(params Vector3[] positions)
    {
        Vector3 position = Vector3.zero;

        float footWeight = 1f / (float)positions.Length;

        // Go through all the legs, rotate the normal by it's offset
        for (int i = 0; i < positions.Length; i++)
        {
            position += positions[i] * footWeight;
        }

        return position;
    }

    // Calculate the normal of the plane defined by leg positions, so we know how to rotate the body
    public static Vector3 GetPlaneNormal(Transform source, Transform[] offsets, float height, float offsetRotationWeight, float scale)
    {
        Vector3 normal = source.up;

        if (offsetRotationWeight <= 0f) return normal;

        float iWeight = 1f / Mathf.Lerp(offsets.Length, 1f, offsetRotationWeight);

        // Go through all the transforms, rotate the normal by it's offset
        for (int i = 0; i < offsets.Length; i++)
        {
            // Direction from source to i
            Vector3 iDir = offsets[i].position - (source.position - source.up * height * scale);

            // Find the tangent to source.up
            Vector3 iNormal = source.up;
            Vector3 iTangent = iDir;
            Vector3.OrthoNormalize(ref iNormal, ref iTangent);

            // Find the rotation offset from the tangent to the direction
            Quaternion fromTo = Quaternion.FromToRotation(iTangent, iDir);
            fromTo = Quaternion.Lerp(Quaternion.identity, fromTo, iWeight);

            // Rotate the normal
            normal = fromTo * normal;
        }

        return normal;
    }
}
