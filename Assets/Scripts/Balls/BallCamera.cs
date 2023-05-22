using Cinemachine;
using UnityEngine;

namespace Balls
{
    public class BallCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float zoomSpeed = 1f, rollSpeed = 1f, shakeThreshold = 10f, rollThreshold = 10f;
        private Rigidbody2D _rigidbody;
        private CinemachineBasicMultiChannelPerlin _perlin;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            virtualCamera.transform.SetParent(null);
        }

        private void LateUpdate()
        {
            var velocity = _rigidbody.velocity;
            var zoom = Mathf.Clamp(velocity.magnitude, 10f, 20f);
            virtualCamera.m_Lens.OrthographicSize =
                Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoom, Time.deltaTime * zoomSpeed);

            // _perlin.m_FrequencyGain = velocity.magnitude > shakeThreshold ? velocity.magnitude : 0f;

            var vectorFrom = Mathf.Sign(velocity.y) * Vector2.up;
            var angle = Vector2.Angle(vectorFrom, velocity);
            var dutch = Mathf.Sign(velocity.x) * Mathf.Sign(velocity.y) *
                        Mathf.Clamp(angle, -rollThreshold, rollThreshold);
            virtualCamera.m_Lens.Dutch = Mathf.Lerp(virtualCamera.m_Lens.Dutch, dutch, Time.deltaTime * rollSpeed);
        }

        private void OnEnable()
        {
            virtualCamera.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (virtualCamera != null)
            {
                virtualCamera.gameObject.SetActive(false);
            }
        }

        public void SetPriority(int priority)
        {
            virtualCamera.Priority = priority;
        }
    }
}