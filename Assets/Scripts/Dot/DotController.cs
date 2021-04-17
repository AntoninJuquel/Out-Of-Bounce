using System;
using System.Collections;
using Systems.Event.Scripts.Channels;
using UnityEngine;

namespace Dot
{
    public class DotController : MonoBehaviour, ICollide
    {
        [SerializeField] private ColorEventChannelSo colorEventChannelSo;
        [SerializeField] private IntEventChannelSo intEventChannelSo;
        private DotSo _dotSo;
        private SpriteRenderer _spriteRenderer;
        private GameObject _gameObject;
        private Transform _transform;
        private Collider2D _collider2D;


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            _gameObject = gameObject;
            _transform = transform;
        }

        private IEnumerator AnimateSprite(Sprite[] sprites)
        {
            var spriteIndex = 0;
            var yieldInstruction = new WaitForSeconds(1f / 15f);
            while (gameObject.activeSelf)
            {
                yield return yieldInstruction;
                spriteIndex = (spriteIndex + 1) % sprites.Length;
                _spriteRenderer.sprite = sprites[spriteIndex];
            }

            StopAllCoroutines();
        }

        public void Setup(DotSo dotSo)
        {
            _dotSo = dotSo;

            _collider2D.isTrigger = false;

            _dotSo.Setup?.Invoke(gameObject);

            _spriteRenderer.color = dotSo.GetColor();

            var sprites = dotSo.GetSprites();
            if (sprites.Length == 1)
                _spriteRenderer.sprite = sprites[0];
            else if (sprites.Length > 0)
                StartCoroutine(AnimateSprite(sprites));
        }

        public void Bounce(GameObject ball, float bouncyness)
        {
            intEventChannelSo.RaiseEvent(_dotSo.GetPoints());
            colorEventChannelSo.RaiseEvent(_dotSo.GetColor());
            _dotSo.Bounce?.Invoke(ball, _gameObject, bouncyness);
            Destroy();
        }

        public void Destroy()
        {
            _dotSo.InstantiateParticles(_transform.position);
            gameObject.SetActive(false);
        }
    }
}