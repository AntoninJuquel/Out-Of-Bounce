using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New teleport dot", menuName = "Dots/Teleport", order = 0)]
    public class TeleportDot : DotSo
    {
        [SerializeField] private Vector2 xRange = new Vector2(10, 32), yRange = new Vector2(10, 32);


        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            collider2D.isTrigger = true;
        }

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            var teleportPosition = dot.transform.position + new Vector3(Random.Range(xRange.x, xRange.y), Random.Range(level >= 2 ? 0 : yRange.x, yRange.y));
            ball.transform.position = teleportPosition;
            InstantiateParticles(teleportPosition);
            Destroy(dot);
        }
    }
}