using System.Linq;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New explosive dot", menuName = "Dots/Explosive", order = 0)]
    public class ExplosiveDot : DotSo
    {
        [SerializeField] private float force = 2f, radius = 15f;
        [SerializeField] protected LayerMask whatIsDot, whatIsBall;

        public override void Destroy(GameObject dot)
        {
            var dotPosition = dot.transform.position;

            foreach (var col in Physics2D.OverlapCircleAll(dotPosition, radius, whatIsBall))
            {
                var direction = col.transform.position - dotPosition;
                col.GetComponent<Rigidbody2D>().velocity += (Vector2) direction.normalized * force;
            }

            base.Destroy(dot);
            foreach (var col in Physics2D.OverlapCircleAll(dotPosition, radius, whatIsDot).Where(c => c.gameObject != dot.gameObject))
            {
                if (!col.gameObject.activeSelf) continue;
                col.GetComponent<DotController>()?.Destroy();
            }
        }
    }
}