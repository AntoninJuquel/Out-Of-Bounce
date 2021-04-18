using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New explosive dot", menuName = "Dots/Explosive", order = 0)]
    public class ExplosiveDot : DotSo
    {
        [SerializeField] private float force = 2f, radius = 15f;

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness * force);
            foreach (var col in Physics2D.OverlapCircleAll(dot.transform.position, radius))
                col.GetComponent<DotController>()?.Destroy();
        }
    }
}