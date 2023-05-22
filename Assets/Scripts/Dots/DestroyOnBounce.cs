using UnityEngine;

namespace Dot
{
    public class DestroyOnBounce : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            DestroyDot(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DestroyDot(other.gameObject);
        }

        private void DestroyDot(GameObject other)
        {
            var dot = other.GetComponent<DotController>();
            if (!dot)
            {
                return;
            }

            dot.Destroy();
            Destroy(gameObject);
        }
    }
}