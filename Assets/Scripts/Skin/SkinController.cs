using ScriptableObjects;
using UnityEngine;

namespace Skin
{
    public class SkinController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private SkinType skinType;
        private Renderer _renderer;
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            var sr = _renderer as SpriteRenderer;
            var skins = playerSo.GetUnlockedSkins()[skinType];
            if (skins.Count == 0)
            {
                if (skinType == SkinType.Particles) _renderer.enabled = false;
                return;
            }
            var skin = skins[Random.Range(0, skins.Count)];
            if (sr)
            {
                sr.sprite = skin.GetSprites()[0];
                sr.color = skin.GetColor();
            }
            else
            {
                _renderer.material.SetTexture(BaseMap, skin.GetSprites()[0].texture);
                _renderer.material.SetTexture("_MainTex", skin.GetSprites()[0].texture);
            }
        }
    }
}