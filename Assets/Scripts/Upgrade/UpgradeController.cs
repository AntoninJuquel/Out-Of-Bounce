using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Upgrade
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField] private PlayerSo _playerSo;
        private List<UpgradeSo> _upgradeSos;

        private void Awake()
        {
            _upgradeSos = _playerSo.GetUpgrades().FindAll(upgrade => upgrade.Unlocked());
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

        private void OnDisable()
        {
            foreach (var upgrade in _upgradeSos)
            {
                upgrade.OnDisableUpgrade(gameObject);
            }
        }

        public void SetUpgrades(List<UpgradeSo> upgrades) => _upgradeSos = upgrades;
    }
}