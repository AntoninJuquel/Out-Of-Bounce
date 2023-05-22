using System.Linq;
using UnityEngine;

namespace Skins
{
    public class SkinController : MonoBehaviour
    {
        [SerializeField] private SkinItem[] skins;
        private SkinItem[] SelectedSkins => skins.Where(skin => skin.Selected).ToArray();
        private Renderer _renderer;

        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            if (SelectedSkins.Length == 0)
            {
                if (_renderer is ParticleSystemRenderer particleSystemRenderer)
                {
                    particleSystemRenderer.enabled = false;
                }

                return;
            }

            var skin = SelectedSkins[Random.Range(0, SelectedSkins.Length)];
            if (_renderer is SpriteRenderer spriteRenderer)
            {
                spriteRenderer.sprite = skin.Sprite;
            }
            else
            {
                _renderer.material.SetTexture(BaseMap, skin.Sprite.texture);
                _renderer.material.SetTexture(MainTex, skin.Sprite.texture);
            }
        }
    }
}