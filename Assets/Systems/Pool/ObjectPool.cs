using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<Pool> pools = new();
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        private void Start()
        {
            StartPool();
        }

        private void StartPool()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (var pool in pools)
            {
                var objectPool = new Queue<GameObject>();

                for (var i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    var controller = obj.AddComponent<ObjectPoolController>();
                    controller.SetObjectPool(this, pool.key);
                    objectPool.Enqueue(obj);
                }

                _poolDictionary.Add(pool.key, objectPool);
            }
        }

        protected GameObject SpawnFromPool(string key, Vector3 position, Quaternion rotation)
        {
            GameObject objectToSpawn;

            if (_poolDictionary == null)
            {
                StartPool();
            }

            if (_poolDictionary == null)
            {
                Debug.LogWarning($"Pool not initialized for {key} in {gameObject.name}");
                return null;
            }

            if (_poolDictionary[key].Count != 0)
            {
                objectToSpawn = _poolDictionary[key].Dequeue();
                objectToSpawn.transform.SetPositionAndRotation(position, rotation);
                objectToSpawn.SetActive(true);
            }
            else
            {
                objectToSpawn = Instantiate(pools.Find(p => p.key == key).prefab, position, rotation);
                var controller = objectToSpawn.AddComponent<ObjectPoolController>();
                controller.SetObjectPool(this, key);
            }

            return objectToSpawn;
        }

        public void ReturnToPool(GameObject go, string key) => _poolDictionary[key].Enqueue(go);
    }

    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size;
    }
}