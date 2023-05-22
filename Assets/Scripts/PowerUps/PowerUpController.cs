using System;
using UnityEngine;

namespace PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField] private PowerUpItem[] powerUps;
        private PowerUpItem[] _selectedPowerUps;

        private void Awake()
        {
            _selectedPowerUps = Array.FindAll(powerUps,
                powerUp => powerUp.Purchased && powerUp.Selected);
            for (var i = 0; i < _selectedPowerUps.Length; i++)
            {
                _selectedPowerUps[i] = Instantiate(_selectedPowerUps[i]);
            }
        }

        private void OnEnable()
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.OnEnablePowerUp(gameObject);
            }
        }

        private void Update()
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.UpdatePowerUp(gameObject);
            }
        }

        private void FixedUpdate()
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.FixedUpdatePowerUp(gameObject);
            }
        }

        private void OnDisable()
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.OnDisablePowerUp(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.OnCollisionEnter2DPowerUp(gameObject, other);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.OnTriggerEnter2DPowerUp(gameObject, other);
            }
        }

        public void OnBounce(GameObject other)
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.OnBouncePowerUp(gameObject, other);
            }
        }

        public void TriggerUpgrades()
        {
            foreach (var powerUp in _selectedPowerUps)
            {
                powerUp.TriggerPowerUp(gameObject);
            }
        }
    }
}