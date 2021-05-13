using UnityEngine;

namespace Game
{
    public static class Utilities
    {
        public static Vector2[] ToVector2Array(Vector3[] vector3Array) => System.Array.ConvertAll(vector3Array, (v3) => new Vector2(v3.x, v3.y));
    
        public static float[] FormatTime(float time)
        {
            var minutes = (int)time / 60;
            var seconds = (int)time % 60;
            var ms = time * 1000;
            ms %= 1000;

            return new float[3] { minutes, seconds, ms };
        }
    }

    public interface ICollide
    {
        public void Bounce(GameObject ball, float bouncyness);
    }
}