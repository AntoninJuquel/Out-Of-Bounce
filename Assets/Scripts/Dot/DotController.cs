using System;
using System.Collections;
using Systems.Chunk;
using Systems.Event.Scripts.Channels;
using Controllers;
using Game;
using Score;
using UnityEngine;

namespace Dot
{
    public class DotController : MonoBehaviour, ICollide
    {
        [SerializeField] private ColorEventChannelSo colorEventChannelSo;
        [SerializeField] private IntEventChannelSo intEventChannelSo;
        private DotSo _dotSo;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        private Chunk _chunk;


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
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

        public void Setup(DotSo dotSo, Chunk chunk)
        {
            _dotSo = dotSo;
            _chunk = chunk;
            _dotSo.Setup(gameObject, _collider2D);
            gameObject.tag = dotSo.name == "Coin" ? "Coin" : "Untagged";

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
            ScoreManager.Instance.SpawnPopup(_dotSo.GetPoints(), transform.position);
            colorEventChannelSo.RaiseEvent(_dotSo.GetColor());
            DotManager.Instance.RemoveDot(_chunk, gameObject);
            CameraController.Instance.StartShake();
            _dotSo.Bounce(ball, gameObject, bouncyness);
        }

        public void Destroy()
        {
            intEventChannelSo.RaiseEvent(_dotSo.GetPoints());
            ScoreManager.Instance.SpawnPopup(_dotSo.GetPoints(), transform.position);
            DotManager.Instance.RemoveDot(_chunk, gameObject);
            _dotSo.Destroy(gameObject);
        }

        public bool IsCoin => _dotSo.name == "Coin";
    }
}