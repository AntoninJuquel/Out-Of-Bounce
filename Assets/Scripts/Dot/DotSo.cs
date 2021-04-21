using Systems.Audio;
using Systems.Unlock;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New basic dot", menuName = "Dots/Basic", order = 0)]
    public class DotSo : UnlockableSo
    {
        [SerializeField] private int points;
        [SerializeField] [Range(0, 1)] private float spawnChance = 1;
        [SerializeField] private GameObject destroyParticles;
        [SerializeField] protected string destroySound = "dot_destroy";

        public virtual void Setup(GameObject dot, Collider2D collider2D)
        {
            collider2D.isTrigger = false;
        }

        public virtual void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = rigid.velocity.normalized * bouncyness;
            AudioManager.Instance.Play(destroySound);
            Destroy(dot);
        }

        public virtual void Destroy(GameObject dot)
        {
            InstantiateParticles(dot.transform.position);
            dot.SetActive(false);
        }

        public bool Spawn(float chance) => chance <= spawnChance;
        public int GetPoints() => points;

        public void InstantiateParticles(Vector3 position)
        {
            var particles = Instantiate(destroyParticles, position, Quaternion.identity);
            var ps = particles.GetComponent<ParticleSystem>();
            var ma = ps.main;
            ma.startColor = color;
        }
    }
}