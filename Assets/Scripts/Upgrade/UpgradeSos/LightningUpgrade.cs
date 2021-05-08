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
        [SerializeField] private float radius, chance;
        [SerializeField] private Vector2 noiseStrength;

        public override void OnBounceUpgrade(GameObject gameObject, GameObject other)
        {
            if(!other.GetComponent<DotController>()) return;
            var cols = Physics2D.OverlapCircleAll(gameObject.transform.position, radius, dotLayer);
            var col = cols.FirstOrDefault(collider2D => collider2D.gameObject != gameObject);
            if (!col) return;
            var distance = Vector2.Distance(gameObject.transform.position, col.transform.position);
            var line = Instantiate(lightningGo).GetComponent<LineRenderer>();
            line.positionCount = Mathf.Max(Mathf.CeilToInt(distance), 2);
            var vector = col.transform.position - gameObject.transform.position;
            var perpendicular = Vector2.Perpendicular(vector).normalized;
            for (var i = 0; i < line.positionCount; i++)
            {
                var percent = i / (float) line.positionCount;
                line.SetPosition(i, (Vector2) (gameObject.transform.position + percent * vector) + perpendicular * Random.Range(noiseStrength.x, noiseStrength.y));
            }
            col.GetComponent<DotController>().Destroy();
            Destroy(line, 1f);
        }
    }
}