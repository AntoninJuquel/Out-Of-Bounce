using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dot
{
    public class DotRenderer : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private IEnumerator AnimateSprite(IReadOnlyList<Sprite> sprites)
        {
            var spriteIndex = 0;
            var yieldInstruction = new WaitForSeconds(1f / 15f);
            while (gameObject.activeSelf)
            {
                yield return yieldInstruction;
                spriteIndex = (spriteIndex + 1) % sprites.Count;
                _spriteRenderer.sprite = sprites[spriteIndex];
            }

            StopAllCoroutines();
        }

        public void Setup(DotItem dotItem)
        {
            var sprites = dotItem.Sprites;
            switch (sprites.Length)
            {
                case 0:
                    _spriteRenderer.sprite = dotItem.Icon;
                    break;
                case 1:
                    _spriteRenderer.sprite = sprites[0];
                    break;
                case > 0:
                    StartCoroutine(AnimateSprite(sprites));
                    break;
            }
        }
    }
}