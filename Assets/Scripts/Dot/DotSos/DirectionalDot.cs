using Effectors;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New directional dot", menuName = "Dots/Directional", order = 0)]
    public class DirectionalDot : DotSo
    {
        [SerializeField] private GameObject areaEffectorPrefab;
        [SerializeField] protected float radius, force, duration;
        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            base.Setup(dot, collider2D);

            dot.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        }

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = dot.transform.right * bouncyness;
            Destroy(dot);
        }

        public override void Destroy(GameObject dot)
        {
            var areaEffector = Instantiate(areaEffectorPrefab, dot.transform.position, Quaternion.Euler(dot.transform.eulerAngles)).GetComponent<AreaEffectorController>();
            
            areaEffector.SetDuration(duration);
            areaEffector.SetRadius(radius);
            areaEffector.SetForce(force);
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