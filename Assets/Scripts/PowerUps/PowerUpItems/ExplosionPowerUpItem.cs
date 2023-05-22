using Dot;
using UnityEngine;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New explosion power-up", menuName = "PowerUps/Explosion", order = 0)]
    public class ExplosionPowerUpItem : PowerUpItem
    {
        [SerializeField] private GameObject explosionParticles;
        [SerializeField] private float radius;

        public override void OnDisablePowerUp(GameObject gameObject)
        {
            Instantiate(explosionParticles, gameObject.transform.position, Quaternion.identity);
            foreach (var col in Physics2D.OverlapCircleAll(gameObject.transform.position, radius))
            {
                var dot = col.GetComponent<DotController>();
                if (dot)
                {
                    dot.Destroy();
                }
            }
        }
    }
}