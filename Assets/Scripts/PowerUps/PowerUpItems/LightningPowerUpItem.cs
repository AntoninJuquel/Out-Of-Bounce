using Dot;
using UnityEngine;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New lightning power-up", menuName = "PowerUps/Lightning", order = 0)]
    public class LightningPowerUpItem : PowerUpItem
    {
        [SerializeField] private LayerMask dotLayer;
        [SerializeField] private GameObject lightningGo;
        [SerializeField] private float radius;
        [SerializeField] private Vector2 noiseStrength;
        [SerializeField] private float[] chancePerLevel, extensionChancePerLevel;
        [SerializeField] private int[] lightningCountPerLevel;
        private float Chance => chancePerLevel[Mathf.Clamp(LevelIndex, 0, chancePerLevel.Length - 1)];

        private float ExtensionChance =>
            extensionChancePerLevel[Mathf.Clamp(LevelIndex, 0, extensionChancePerLevel.Length - 1)];

        private int LightningCount =>
            lightningCountPerLevel[Mathf.Clamp(LevelIndex, 0, lightningCountPerLevel.Length - 1)];

        private void CastLightningBolt(Vector3 position)
        {
            while (true)
            {
                var cols = Physics2D.OverlapCircleAll(position, radius, dotLayer);

                if (cols.Length == 0)
                {
                    return;
                }

                var dot = cols[0]?.GetComponent<DotController>();

                if (!dot)
                {
                    return;
                }

                var dotPosition = dot.transform.position;
                var direction = dotPosition - position;
                var perpendicular = Vector2.Perpendicular(direction).normalized;
                var distance = Vector2.Distance(position, dotPosition);

                var line = Instantiate(lightningGo).GetComponent<LineRenderer>();
                line.positionCount = Mathf.Max(Mathf.CeilToInt(distance), 2);
                for (var i = 0; i < line.positionCount; i++)
                {
                    var percent = i / (float)line.positionCount;
                    line.SetPosition(i,
                        (Vector2)(position + percent * direction) +
                        perpendicular * Random.Range(noiseStrength.x, noiseStrength.y));
                }

                dot.Destroy();
                Destroy(line.gameObject, .5f);

                if (Random.value < ExtensionChance)
                {
                    position = dotPosition;
                    line.SetPosition(line.positionCount - 1, position);
                    continue;
                }

                break;
            }
        }

        public override void OnBouncePowerUp(GameObject gameObject, GameObject other)
        {
            if (other.GetComponent<DotController>() == null)
            {
                return;
            }

            for (var i = 0; i < LightningCount; i++)
            {
                if (Random.value > Chance)
                {
                    continue;
                }

                CastLightningBolt(gameObject.transform.position);
            }
        }
    }
}