using System;
using UnityEngine;

namespace Systems.Pool
{
    public class ObjectPoolController : MonoBehaviour
    {
        private ObjectPool _objectPool;

        private void OnDisable()
        {
            if (!_objectPool) return;
            _objectPool.ReturnToPool(gameObject);
        }

        public void SetObjectPool(ObjectPool objectPool) => _objectPool = objectPool;
    }
}