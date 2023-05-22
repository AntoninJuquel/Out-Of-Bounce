using UnityEngine;

namespace Pool
{
    public class ObjectPoolController : MonoBehaviour
    {
        private ObjectPool _objectPool;
        private string _key;

        private void OnDisable()
        {
            if (!_objectPool)
            {
                return;
            }

            _objectPool.ReturnToPool(gameObject, _key);
        }

        public void SetObjectPool(ObjectPool objectPool, string key)
        {
            _objectPool = objectPool;
            _key = key;
        }
    }
}