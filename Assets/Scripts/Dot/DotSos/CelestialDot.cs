using Packages.LeanTween;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New celestial dot", menuName = "Dots/Celestial", order = 0)]
    public class CelestialDot : DotSo
    {
        [SerializeField] private float radius = 15f, force = 10f, speed = 1f, bounceMult;

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness * bounceMult);
        }

        public override void Destroy(GameObject dot)
        {
            foreach (var col in Physics2D.OverlapCircleAll(dot.transform.position, radius, 1 << dot.layer))
            {
                Vector2 direction = col.transform.position - dot.transform.position;
                LeanTween.cancel(col.gameObject);
                LeanTween
                    .move(col.gameObject, col.transform.position + (Vector3) direction.normalized * force, speed)
                    .setEaseOutCirc();
            }
            base.Destroy(dot);
        }
    }
}