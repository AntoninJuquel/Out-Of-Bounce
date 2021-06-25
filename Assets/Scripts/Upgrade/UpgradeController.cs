using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Upgrade
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private UpgradeType upgradeType;
        [SerializeField] private List<UpgradeSo> upgradeSos;

        private void Awake()
        {
            upgradeSos = playerSo.GetUpgrades().FindAll(upgrade => upgrade.Unlocked() && upgrade.type == upgradeType);
            for (var i = 0; i < upgradeSos.Count; i++)
            {
                upgradeSos[i] = Instantiate(upgradeSos[i]);
            }
        }

        private void OnEnable()
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.OnEnableUpgrade(gameObject);
            }
        }

        private void Update()
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.UpdateUpgrade(gameObject);
            }
        }

        private void FixedUpdate()
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.FixedUpdateUpgrade(gameObject);
            }
        }

        private void OnDisable()
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.OnDisableUpgrade(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.OnCollisionEnter2DUpgrade(gameObject, other);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.OnTriggerEnter2DUpgrade(gameObject, other);
            }
        }

        public void OnBounce(GameObject other)
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.OnBounceUpgrade(gameObject, other);
            }
        }

        public void TriggerUpgrades()
        {
            foreach (var upgrade in upgradeSos)
            {
                upgrade.TriggerUpgrade(gameObject);
            }
        }
    }
}