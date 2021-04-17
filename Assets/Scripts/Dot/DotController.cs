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
        private Action<GameObject, GameObject, float> _bounce;
        private GameObject _gameObject;
        private Transform _transform;


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
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

        public void Setup(DotSo dotSo, Action<GameObject> setup, Action<GameObject, GameObject, float> bounce)
        {
            _dotSo = dotSo;

            setup?.Invoke(gameObject);
            _bounce = bounce;

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
            _bounce?.Invoke(ball, _gameObject, bouncyness);
            _dotSo.InstantiateParticles(_transform.position);
            gameObject.SetActive(false);
        }
    }
}