using System.Collections.Generic;
using Systems.Pool;
using Game;
using ScriptableObjects;
using Skin;
using UnityEngine;
using UnityEngine.EventSystems;
using UserInterface;

namespace Platform
{
    public class PlatformManager : ObjectPool
    {
        public static PlatformManager Instance;
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private float radius = .25f, platformTimer = 5f;
        [SerializeField] private SkinSo defaultSkin;
        private int _platformAmount, _platformCounter = 3;
        private LineRenderer _lineRenderer;
        private Vector2 MousePosition => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        private Camera _mainCamera;
        private List<SkinSo> _platformSkins;

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int Color = Shader.PropertyToID("_Color");

        private void Awake()
        {
            Instance = this;
            _lineRenderer = GetComponent<LineRenderer>();
            _mainCamera = Camera.main;
            _platformCounter = _platformAmount = playerSo.GetPlatformCount();
            UpdatePlatformCounter();
            _platformSkins = playerSo.GetUnlockedSkins()[SkinType.Platforms];
        }

        private void Update()
        {
            if (GameManager.GameStatus == GameStatus.Paused) return;

            if (Input.GetMouseButtonDown(0) && _platformCounter > 0 && !EventSystem.current.IsPointerOverGameObject()) StartLine();
            if (Input.GetMouseButton(0)) UpdateLine();
            if (Input.GetMouseButtonUp(0)) ExitLine();
        }

        private void StartLine()
        {
            var skin = _platformSkins.Count == 0 ? defaultSkin : _platformSkins[Random.Range(0, _platformSkins.Count)];
            _lineRenderer.material.SetTexture(MainTex, skin.GetSprites()[0].texture);
            _lineRenderer.material.SetColor(Color, skin.GetColor() * .5f);
            _lineRenderer.widthMultiplier = radius;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new[] {(Vector3) MousePosition, (Vector3) MousePosition});

            _platformCounter--;
            UpdatePlatformCounter();
        }

        private void UpdateLine()
        {
            _lineRenderer.SetPosition(1, MousePosition);
        }

        private void ExitLine()
        {
            var platform = SpawnFromPool("Platform", Vector2.zero, Quaternion.identity).GetComponent<PlatformController>();
            platform.Activate(_lineRenderer, platformTimer);
            _lineRenderer.positionCount = 0;
        }

        private void UpdatePlatformCounter() => CanvasManager.Instance.SetPlatformText(_platformCounter);

        public void ResupplyPlatforms(int points)
        {
            _platformCounter = points <= 0 ? _platformCounter : _platformAmount;
            UpdatePlatformCounter();
        }
    }
}