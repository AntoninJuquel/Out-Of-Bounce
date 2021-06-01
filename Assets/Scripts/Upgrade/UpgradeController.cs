using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Upgrade
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private UpgradeType upgradeType;
        private List<UpgradeSo> _upgradeSos;

        private void Awake()
        {
            _upgradeSos = playerSo.GetUpgrades().FindAll(upgrade => upgrade.Unlocked() && upgrade.type == upgradeType);
        }

        private void OnEnable()
        {
            foreach (var upgrade in _upgradeSos)
            {
                upgrade.OnEnableUpgrade(gameObject);
            }
        }

        private void Update()
        {
            foreach (var upgrade in _upgradeSos)
            {
                upgrade.UpdateUpgrade(gameObject);
            }
        }

        private void FixedUpdate()
        {
            foreach (var upgrade in _upgradeSos)
            {
                upgrade.FixedUpdateUpgrade(gameObject);
            }
        }

        private void OnDisable()
        {
            foreach (var upgrade in _upgradeSos)
            {
                upgrade.OnDisableUpgrade(gameObject);
            }
        }

        public void OnBounce(GameObject other)
        {
            foreach (var upgrade in _upgradeSos)
            {
                upgrade.OnBounceUpgrade(gameObject, other);
            }
        }
    }
}