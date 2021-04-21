using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New teleport dot", menuName = "Dots/Teleport", order = 0)]
    public class TeleportDot : DotSo
    {
        [SerializeField] private Vector2 xRange = new Vector2(10, 32), yRange = new Vector2(10, 32);

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            var teleportPosition = dot.transform.position + new Vector3(Random.Range(xRange.x, xRange.y), Random.Range(yRange.x, yRange.y));
            ball.transform.position = teleportPosition;
            InstantiateParticles(teleportPosition);
            base.Bounce(ball, dot, bouncyness);
        }
    }
}