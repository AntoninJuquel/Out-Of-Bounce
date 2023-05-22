using Balls;
using Effectors;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New directional dot", menuName = "Dots/Directional", order = 0)]
    public class DirectionalDotItem : DotItem
    {
        [SerializeField] private GameObject areaEffectorPrefab;

        [SerializeField] private float[] radiusPerLevel, forcePerLevel, durationPerLevel;
        private float Radius => radiusPerLevel[Mathf.Clamp(LevelIndex, 0, radiusPerLevel.Length - 1)];
        private float Force => forcePerLevel[Mathf.Clamp(LevelIndex, 0, forcePerLevel.Length - 1)];
        private float Duration => durationPerLevel[Mathf.Clamp(LevelIndex, 0, durationPerLevel.Length - 1)];

        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            base.Setup(dot, collider2D);

            dot.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        }

        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = dot.transform.right * bouncyness;
            dot.Destroy();
        }

        public override void Destroy(DotController dot)
        {
            var dotTransform = dot.transform;

            var areaEffector =
                Instantiate(areaEffectorPrefab, dotTransform.position, Quaternion.Euler(dotTransform.eulerAngles))
                    .GetComponent<AreaEffectorController>();

            areaEffector.SetDuration(Duration);
            areaEffector.SetRadius(Radius);
            areaEffector.SetForce(Force);
            areaEffector.SetColor(color);

            areaEffector.SetOnTriggerEnterAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 0;
            });
            areaEffector.SetOnTriggerExitAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 1;
            });

            base.Destroy(dot);
        }
    }
}