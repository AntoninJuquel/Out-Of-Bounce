using System.Linq;
using Cinemachine;
using Pool;
using Skins;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using PowerUp.UpgradeSos;

namespace Platform
{
    public class PlatformManager : ObjectPool
    {
        [SerializeField] private PlatformCountPowerUpItem platformCountPowerUp;
        [SerializeField] private SkinItem[] skins;
        private SkinItem[] SelectedSkins => skins.Where(skin => skin.Selected).ToArray();
        [SerializeField] private float radius = .25f, platformTimer = 5f;

        [SerializeField] private SkinItem defaultSkinItem;

        [SerializeField] private Transform center;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private UnityEvent<int> onPlatformCounterChanged;

        private int _platformAmount, _platformCounter = 3;
        private LineRenderer _lineRenderer;
        private Vector2 MousePosition => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        private Camera _mainCamera;
        private EventSystem _eventSystem;

        private static readonly int MainTexPropertyId = Shader.PropertyToID("_MainTex");
        private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        private bool HasPlatforms => _platformCounter > 0;
        private bool IsLineActive => _lineRenderer.positionCount == 2;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _mainCamera = Camera.main;
            _eventSystem = EventSystem.current;
            _platformCounter = _platformAmount = 3 + platformCountPowerUp.Level;
            onPlatformCounterChanged?.Invoke(_platformCounter);
        }

        private void Update()
        {
            if (InputDown() && HasPlatforms)
            {
                StartLine();
            }

            if (InputDrag() && IsLineActive)
            {
                UpdateLine();
            }

            if (InputUp() && IsLineActive)
            {
                ExitLine();
            }
        }

        private bool InputDown()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Input.GetMouseButtonDown(0) && !_eventSystem.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
                   !_eventSystem.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#else
            return false;
#endif
        }

        private bool InputDrag()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Input.GetMouseButton(0);
#elif UNITY_ANDROID || UNITY_IOS
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
#else
            return false;
#endif
        }

        private bool InputUp()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Input.GetMouseButtonUp(0);
#elif UNITY_ANDROID || UNITY_IOS
               return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#else
            return false;
#endif
        }

        private void StartLine()
        {
            virtualCamera.enabled = true;

            var skin = SelectedSkins.Length == 0
                ? defaultSkinItem
                : SelectedSkins[Random.Range(0, SelectedSkins.Length)];
            _lineRenderer.material.SetTexture(MainTexPropertyId, skin.Sprite.texture);
            _lineRenderer.material.SetColor(ColorPropertyId, Color.white * .5f);
            _lineRenderer.widthMultiplier = radius;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new[] { (Vector3)MousePosition, (Vector3)MousePosition });

            _platformCounter--;
            onPlatformCounterChanged?.Invoke(_platformCounter);
        }

        private void UpdateLine()
        {
            _lineRenderer.SetPosition(1, MousePosition);
            center.position = _lineRenderer.GetPosition(0) +
                              (_lineRenderer.GetPosition(1) - _lineRenderer.GetPosition(0)) / 2f;
        }

        private void ExitLine()
        {
            var platform = SpawnFromPool("Platform", Vector2.zero, Quaternion.identity)
                .GetComponent<PlatformController>();
            platform.Activate(_lineRenderer, platformTimer);
            _lineRenderer.positionCount = 0;

            virtualCamera.enabled = false;
        }

        public void ResupplyPlatforms()
        {
            _platformCounter = _platformAmount;
            onPlatformCounterChanged?.Invoke(_platformCounter);
        }
    }
}