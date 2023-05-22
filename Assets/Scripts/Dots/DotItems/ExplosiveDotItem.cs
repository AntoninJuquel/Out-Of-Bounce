using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dot
{
    [CreateAssetMenu(fileName = "New explosive dot", menuName = "Dots/Explosive", order = 0)]
    public class ExplosiveDotItem : DotItem
    {
        [SerializeField] private float[] forcePerLevel, radiusPerLevel;
        [SerializeField] protected LayerMask whatIsDot, whatIsBall;

        private float Force => forcePerLevel[Mathf.Clamp(LevelIndex, 0, forcePerLevel.Length - 1)];
        private float Radius => radiusPerLevel[Mathf.Clamp(LevelIndex, 0, radiusPerLevel.Length - 1)];

        public override void Destroy(DotController dot)
        {
            var dotPosition = dot.transform.position;

            foreach (var col in Physics2D.OverlapCircleAll(dotPosition, Radius, whatIsBall))
            {
                var direction = col.transform.position - dotPosition;
                col.GetComponent<Rigidbody2D>().velocity += (Vector2)direction.normalized * Force;
            }

            base.Destroy(dot);
            var otherDots = Physics2D.OverlapCircleAll(dotPosition, Radius, whatIsDot)
                .Where(c => c.gameObject != dot.gameObject && dot.gameObject.activeSelf)
                .Select(c => c.GetComponent<DotController>())
                .Where(c => c != null)
                .ToList();

            foreach (var otherDot in otherDots)
            {
                otherDot.Destroy();
            }
        }

        public override void DrawGizmos(DotController dot)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(dot.transform.position, Radius);
        }
    }
}