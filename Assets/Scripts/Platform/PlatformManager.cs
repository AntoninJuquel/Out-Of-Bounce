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
        private PlatformController _currentPlatform;
        private List<Vector3> _mousePositions = new List<Vector3>();
        private Vector2 mousePosition => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        private Camera _mainCamera;
        private List<SkinSo> _platformSkins;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int Color = Shader.PropertyToID("_Color");

        private void Awake()
        {
            Instance = this;
            _mainCamera = Camera.main;
            _platformCounter = _platformAmount = playerSo.GetPlatformCount();
            UpdatePlatformCounter();
            _platformSkins = playerSo.GetUnlockedSkins()[SkinType.Platforms];
        }

        private void Update()
        {
            if (GameManager.GameStatus == GameStatus.Paused) return;

            if (Input.GetMouseButtonDown(0) && _platformCounter > 0 && !EventSystem.current.IsPointerOverGameObject())
            {
                StartLine();
            }

            if (!_currentPlatform) return;

            if (Input.GetMouseButton(0)) UpdateLine();
            else if (Input.GetMouseButtonUp(0)) ExitLine();
        }

        private void DrawLine()
        {
            var lr = _currentPlatform.GetLineRenderer();
            var edgeCol = _currentPlatform.GetEdgeCollider2D();
            lr.positionCount = _mousePositions.Count;
            lr.SetPositions(_mousePositions.ToArray());
            edgeCol.points = Game.Utilities.ToVector2Array(_mousePositions.ToArray());
        }

        private void StartLine()
        {
            _platformCounter--;
            UpdatePlatformCounter();
            _mousePositions.Add(mousePosition);
            _currentPlatform = SpawnFromPool("Platform", Vector2.zero, Quaternion.identity).GetComponent<PlatformController>();
            _currentPlatform.SetActive(true);
            var lr = _currentPlatform.GetLineRenderer();
            var edgeCol = _currentPlatform.GetEdgeCollider2D();
            var skin = _platformSkins.Count == 0 ? defaultSkin : _platformSkins[Random.Range(0, _platformSkins.Count)];
            lr.material.SetTexture(MainTex, skin.GetSprites()[0].texture);
            lr.material.SetColor(Color, skin.GetColor() * .5f);
            _mousePositions.Add(mousePosition);
            lr.widthMultiplier = edgeCol.edgeRadius = radius;
            edgeCol.enabled = false;
            DrawLine();
        }

        private void UpdateLine()
        {
            if (_mousePositions.Count < 2) _mousePositions.Add(mousePosition);
            else _mousePositions[1] = mousePosition;
            DrawLine();
        }

        private void ExitLine()
        {
            _currentPlatform.Activate(platformTimer);
            _mousePositions = new List<Vector3>();
            _currentPlatform = null;
        }

        private void UpdatePlatformCounter() => CanvasManager.Instance.SetPlatformText(_platformCounter);

        public void ResupplyPlatforms(int points)
        {
            _platformCounter = points <= 0 ? _platformCounter : _platformAmount;
            UpdatePlatformCounter();
        }
    }
}