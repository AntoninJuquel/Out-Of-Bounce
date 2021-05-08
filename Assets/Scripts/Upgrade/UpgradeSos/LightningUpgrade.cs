using System.Linq;
using Dot;
using UnityEngine;

namespace Upgrade.UpgradeSos
{
    [CreateAssetMenu(fileName = "New lightning upgrade", menuName = "Upgrades/Lightning", order = 0)]
    public class LightningUpgrade : UpgradeSo
    {
        [SerializeField] private LayerMask dotLayer;
        [SerializeField] private GameObject lightningGo;
        [SerializeField] private float radius;
        [SerializeField] private Vector2 noiseStrength;
        private float Chance => (level + 1) * .5f;
        private int LightningCount => level < 2 ? 1 : level;

        private void CastLightningBolt(Vector3 position)
        {
            var cols = Physics2D.OverlapCircleAll(position, radius, dotLayer);
            if (cols.Length == 0) return;
            var dot = cols[0]?.GetComponent<DotController>();

            if (!dot) return;

            var dotPosition = dot.transform.position;
            var direction = dotPosition - position;
            var perpendicular = Vector2.Perpendicular(direction).normalized;
            var distance = Vector2.Distance(position, dotPosition);

            var line = Instantiate(lightningGo).GetComponent<LineRenderer>();
            line.positionCount = Mathf.Max(Mathf.CeilToInt(distance), 2);
            for (var i = 0; i < line.positionCount; i++)
            {
                var percent = i / (float) line.positionCount;
                line.SetPosition(i, (Vector2) (position + percent * direction) + perpendicular * Random.Range(noiseStrength.x, noiseStrength.y));
            }

            dot.Destroy();
            Destroy(line, .5f);
        }

        public override void OnBounceUpgrade(GameObject gameObject, GameObject other)
        {
            for (var i = 0; i < LightningCount; i++)
            {
                if (Random.value > Chance - i * .9f || !other.GetComponent<DotController>()) return;
                CastLightningBolt(gameObject.transform.position);
            }
        }
    }
}