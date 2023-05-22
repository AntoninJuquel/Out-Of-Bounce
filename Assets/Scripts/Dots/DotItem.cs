using System;
using Balls;
using MoreMountains.Feedbacks;
using Save;
using ShopSystem;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New basic dot", menuName = "Dots/Basic", order = 0)]
    public class DotItem : ShopItem, ISave
    {
        [SerializeField] private LayerMask layer;
        [SerializeField] protected Color color;
        [SerializeField] [Range(0, 1)] private float spawnChance = 1;
        [field: SerializeField] public int Points { get; private set; }
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
        [SerializeField] protected GameObject deathFeedback;
        public float SpawnChance => spawnChance;

        public virtual void Setup(GameObject dot, Collider2D collider2D)
        {
            collider2D.isTrigger = false;
            dot.layer = (int)Mathf.Log(layer.value, 2);
        }

        public virtual void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = rigid.velocity.normalized * bouncyness;
            dot.Destroy();
        }

        public virtual void Destroy(DotController dot)
        {
            Instantiate(deathFeedback, dot.transform.position, Quaternion.identity);
        }

        public virtual void DrawGizmos(DotController dot)
        {
        }

        private void OnEnable()
        {
            onItemChange.AddListener(OnItemChange);
        }

        private void OnDisable()
        {
            onItemChange.RemoveListener(OnItemChange);
        }

        private void OnItemChange(ShopItemSave save, string saveName)
        {
            Save();
        }

        public event Action<string, object> OnSave;
        public object DefaultSave => SaveItem();
        public string Name => name;

        public void Load(object loadedObject)
        {
            if (loadedObject is ShopItemSave save)
            {
                LoadItem(save);
            }
        }

        public void Save()
        {
            OnSave?.Invoke(Name, SaveItem());
        }
    }
}