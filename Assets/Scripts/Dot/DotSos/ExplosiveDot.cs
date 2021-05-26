using System.Linq;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New explosive dot", menuName = "Dots/Explosive", order = 0)]
    public class ExplosiveDot : DotSo
    {
        [SerializeField] private float force = 2f, radius = 15f;
        [SerializeField] protected LayerMask whatIsDot, whatIsBall;

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness * force);
        }

        public override void Destroy(GameObject dot)
        {
            base.Destroy(dot);
            foreach (var col in Physics2D.OverlapCircleAll(dot.transform.position, radius, whatIsDot).Where(c => c.gameObject != dot.gameObject))
            {
                if (!col.gameObject.activeSelf) continue;
                col.GetComponent<DotController>()?.Destroy();
            }
        }
    }
}