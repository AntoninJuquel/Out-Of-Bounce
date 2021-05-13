using System.Collections;
using Systems.Event.Scripts.Channels;
using Game;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
        [SerializeField] private float moveSpeed = 7f, zoomSpeed = 7f;
        [SerializeField] private FloatEventChannelSo xAxisChannelSo, yAxisChannelSo;
        private Camera _camera;
        private Transform _transform;
        private Rigidbody2D _target;
        private float _targetZoom;
        private Vector3 _moveVelocity, _position, _targetPos, _targetVelocity;
        private IEnumerator _currentShake;

        private void Awake()
        {
            Instance = this;
            _camera = GetComponent<Camera>();
            _transform = transform;
        }

        private void Update()
        {
            if (!_target) return;
            _position = _transform.position;
            _targetVelocity = _target.velocity;
            _targetPos = _target.position + (Vector2) _targetVelocity.normalized;

            _targetZoom = Mathf.Clamp(_targetVelocity.magnitude, 10f, 20f);

            if (GameManager.GameStatus == GameStatus.GameOver) return;
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * zoomSpeed);
            xAxisChannelSo.RaiseEvent(_position.x);
            yAxisChannelSo.RaiseEvent(_position.y);
        }

        private void LateUpdate()
        {
            if (!_target || Input.GetMouseButton(0)) return;
            _transform.position = Vector3.SmoothDamp(_position, _targetPos + Vector3.back, ref _moveVelocity, 1f / moveSpeed);
        }

        private IEnumerator Shake(float duration, float movementPower, float rotationPower)
        {
            var elapsed = 0f;
            var mvtFadeTime = movementPower / duration;
            var rotFadeTime = rotationPower / duration;
            while (elapsed < duration)
            {
                var xAmount = Random.Range(-1f, 1f) * movementPower;
                var yAmount = Random.Range(-1f, 1f) * movementPower;
                var zAmount = Random.Range(-1f, 1f) * rotationPower;

                _transform.position += new Vector3(xAmount, yAmount);
                _transform.rotation = Quaternion.Euler(0f, 0f, zAmount);

                movementPower = Mathf.MoveTowards(movementPower, 0f, mvtFadeTime * Time.deltaTime);
                rotationPower = Mathf.MoveTowards(rotationPower, 0f, rotFadeTime * Time.deltaTime);

                elapsed += Time.deltaTime;
                yield return null;
            }

            _transform.rotation = Quaternion.identity;
        }

        public void SetTarget(Rigidbody2D target) => _target = target;
        public void SetTarget(GameObject target) => _target = target.GetComponent<Rigidbody2D>();

        public void StartShake()
        {
            if (_currentShake != null) StopCoroutine(_currentShake);
            _currentShake = Shake(.25f, .05f, 2.5f);
            StartCoroutine(_currentShake);
        }

        public void StopShake()
        {
            if (_currentShake != null)
                StopCoroutine(_currentShake);
        }
    }
}