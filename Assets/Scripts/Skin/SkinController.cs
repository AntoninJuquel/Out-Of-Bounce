using ScriptableObjects;
using UnityEngine;

namespace Skin
{
    public class SkinController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private SkinType skinType;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            var skins = playerSo.GetUnlockedSkins()[skinType];
            if (skins.Count == 0) return;
            var skin = skins[Random.Range(0, skins.Count)];
            _spriteRenderer.sprite = skin.GetSprites()[0];
            _spriteRenderer.color = skin.GetColor();
        }
    }
}