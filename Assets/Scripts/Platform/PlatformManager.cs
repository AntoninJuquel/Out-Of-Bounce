using System.Collections.Generic;
using Systems.Pool;
using UnityEngine;

namespace Platform
{
    public class PlatformManager : ObjectPool
    {
        [SerializeField] private int platformAmount = 3, platformCounter = 3;
        [SerializeField] private float radius = .25f, platformTimer = 5f;
        private PlatformController _currentPlatform;
        private List<Vector3> _mousePositions = new List<Vector3>();
        private Vector2 MousePosition => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            platformCounter = platformAmount;
        }

        private void Update()
        {
            if (GameManager.GameStatus == GameStatus.Paused) return;

            if (Input.GetMouseButtonDown(0) && platformCounter > 0)
            {
                StartLine();
            }

            if (!_currentPlatform) return;

            if (Input.GetMouseButton(0))
            {
                UpdateLine();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _currentPlatform.Activate(platformTimer);
                ExitLine();
            }
        }

        private void DrawLine()
        {
            var lr = _currentPlatform.GetLineRenderer();
            var edgeCol = _currentPlatform.GetEdgeCollider2D();
            lr.positionCount = _mousePositions.Count;
            lr.SetPositions(_mousePositions.ToArray());
            edgeCol.points = Utilities.ToVector2Array(_mousePositions.ToArray());
        }

        private void StartLine()
        {
            platformCounter--;
            // UpdatePlatformCounter();
            _mousePositions.Add(MousePosition);
            _currentPlatform = SpawnFromPool("Platform", Vector2.zero, Quaternion.identity).GetComponent<PlatformController>();
            _currentPlatform.SetActive(true);
            var lr = _currentPlatform.GetLineRenderer();
            var edgeCol = _currentPlatform.GetEdgeCollider2D();
            _mousePositions.Add(MousePosition);
            lr.widthMultiplier = edgeCol.edgeRadius = radius;
            edgeCol.enabled = false;
            DrawLine();
        }

        private void UpdateLine()
        {
            if (_mousePositions.Count < 2)
            {
                _mousePositions.Add(MousePosition);
            }
            else
            {
                _mousePositions[1] = MousePosition;
            }

            DrawLine();
        }

        private void ExitLine()
        {
            _mousePositions = new List<Vector3>();
            _currentPlatform = null;
        }

        // private void UpdatePlatformCounter() => platformsCounterChannel.RaiseEvent(string.Concat(platformCounter, " |"));
        public void ResupplyPlatforms(float points)
        {
            if (points < 0)
                platformCounter = 0;
            else if (points > 0)
                platformCounter = platformAmount;
            // UpdatePlatformCounter();
        }
    }
}