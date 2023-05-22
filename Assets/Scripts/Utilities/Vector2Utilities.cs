using UnityEngine;

namespace Utilities
{
    public static class Vector2Utilities
    {
        public static Vector2[] ToVector2Array(Vector3[] vector3Array) =>
            System.Array.ConvertAll(vector3Array, (v3) => new Vector2(v3.x, v3.y));
    }
}