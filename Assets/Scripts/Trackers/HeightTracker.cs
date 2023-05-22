using UnityEngine;
using UnityEngine.Events;

namespace Trackers
{
    public class HeightTracker : MonoBehaviour
    {
        [SerializeField] private UnityEvent<float> onHeightChanged, onTopHeightChanged, onEndTracking;
        private Camera _camera;
        private float _height, _topHeight;

        private float Height
        {
            set
            {
                if (value > _topHeight)
                {
                    TopHeight = value;
                }

                _height = value;
                onHeightChanged?.Invoke(_height);
            }
        }

        private float TopHeight
        {
            set
            {
                _topHeight = value;
                onTopHeightChanged?.Invoke(_topHeight);
            }
        }

        private void UpdateHeight()
        {
            if (!_camera)
            {
                return;
            }

            Height = _camera.transform.position.y;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            UpdateHeight();
        }

        public void EndTracking()
        {
            onEndTracking?.Invoke(_topHeight);
        }
    }
}