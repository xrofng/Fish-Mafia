using UnityEngine;

public static class RaycastUtil
{
    /// <summary>
    /// Shoots a ray downward and returns the ground position.
    /// </summary>
    public static bool TryGetGroundPosition(Vector3 origin, out Vector3 groundPosition, float distance = 100f, LayerMask layerMask = default)
    {
        Ray ray = new Ray(origin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, layerMask))
        {
            groundPosition = hit.point;
            return true;
        }

        groundPosition = origin;
        return false;
    }
}