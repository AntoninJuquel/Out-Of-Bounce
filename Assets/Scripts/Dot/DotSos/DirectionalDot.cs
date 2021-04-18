using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New directional dot", menuName = "Dots/Directional", order = 0)]
    public class DirectionalDot : DotSo
    {
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
    }
}