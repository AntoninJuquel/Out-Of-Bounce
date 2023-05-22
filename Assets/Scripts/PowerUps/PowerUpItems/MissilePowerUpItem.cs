using Utilities.Behaviours;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New missile power-up", menuName = "PowerUps/Missile", order = 0)]
    public class MissilePowerUpItem : PowerUpItem
    {
        [SerializeField] private LayerMask whatIsDot;
        [SerializeField] private GameObject projectileGo;

        [SerializeField] private float[] missileSpeedPerLevel;

        [SerializeField] private float[] radiusPerLevel,
            rotationSpeedPerLevel,
            deviationAmountPerLevel,
            deviationSpeedPerLevel,
            fireRatePerLevel;

        private float MissileSpeed => missileSpeedPerLevel[Mathf.Clamp(LevelIndex, 0, missileSpeedPerLevel.Length - 1)];
        private float Radius => radiusPerLevel[Mathf.Clamp(LevelIndex, 0, radiusPerLevel.Length - 1)];

        private float RotationSpeed =>
            rotationSpeedPerLevel[Mathf.Clamp(LevelIndex, 0, rotationSpeedPerLevel.Length - 1)];

        private float DeviationAmount =>
            deviationAmountPerLevel[Mathf.Clamp(LevelIndex, 0, deviationAmountPerLevel.Length - 1)];

        private float DeviationSpeed =>
            deviationSpeedPerLevel[Mathf.Clamp(LevelIndex, 0, deviationSpeedPerLevel.Length - 1)];

        private float TimeToShoot => 1f / fireRatePerLevel[Mathf.Clamp(LevelIndex, 0, fireRatePerLevel.Length - 1)];
        private float _timer;

        public override void OnEnablePowerUp(GameObject gameObject)
        {
            _timer = 0f;
        }

        public override void UpdatePowerUp(GameObject gameObject)
        {
            _timer += Time.deltaTime;
            if (_timer < TimeToShoot)
            {
                return;
            }

            Shoot(gameObject.transform.position);
            _timer = 0;
        }

        private void Shoot(Vector3 startPoint)
        {
            var missile = Instantiate(projectileGo, startPoint,
                quaternion.Euler(Vector3.forward * Random.Range(0f, 360f))).GetComponent<AimForLayer>();
            missile.Setup(MissileSpeed, RotationSpeed, Radius, DeviationAmount, DeviationSpeed, whatIsDot);
        }
    }
}