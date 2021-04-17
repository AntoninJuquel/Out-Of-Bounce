using System;
using Systems.Unlock;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New Dot Data", menuName = "Dot", order = 0)]
    public class DotSo : UnlockableSo
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private int points;
        [SerializeField] private DotType dotType;
        [SerializeField] [Range(0, 1)] private float spawnChance = 1;
        [SerializeField] private GameObject destroyParticles;
        public Action<GameObject> Setup { get; private set; }
        public Action<GameObject, GameObject, float> Bounce { get; private set; }
        public void SetActions(Action<GameObject> setup, Action<GameObject, GameObject, float> bounce)
        {
            Setup = setup;
            Bounce = bounce;
        }
        public bool Spawn(float chance) => chance <= spawnChance;
        public Sprite[] GetSprites() => sprites;
        public DotType GetDotType() => dotType;
        public Color GetColor() => color;
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