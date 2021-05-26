using System;
using System.Collections.Generic;
using UnityEngine;

namespace Effectors
{
    public abstract class EffectorController : MonoBehaviour
    {
        [SerializeField] protected ParticleSystem ps;
        private Action<Collider2D> _onTriggerEnterAction;
        private Action<Collider2D> _onTriggerExitAction;
        private List<Collider2D> _colliders = new List<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onTriggerEnterAction?.Invoke(other);
            _colliders.Add(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _onTriggerEnterAction?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _onTriggerExitAction?.Invoke(other);
            _colliders.Remove(other);
        }

        private void OnDestroy()
        {
            foreach (var col in _colliders)
            {
                _onTriggerExitAction?.Invoke(col);
            }
        }

        public void SetOnTriggerEnterAction(Action<Collider2D> action) => _onTriggerEnterAction = action;
        public void SetOnTriggerExitAction(Action<Collider2D> action) => _onTriggerExitAction = action;

        public abstract void SetRadius(float radius);
        public abstract void SetForce(float force);
        public abstract void SetColor(Color color);

        public void SetDuration(float time)
        {
            var main = ps.main;
            main.duration = time;
            ps.Play();
        }

        public void OnParticleSystemStopped()
        {
            Destroy(gameObject);
        }
    }
}