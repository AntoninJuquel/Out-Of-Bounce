using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Behaviours
{
    public class RigidbodyRecorder : MonoBehaviour
    {
        [SerializeField] private float recordTime = 1;
        private Rigidbody2D _rigidbody2D;
        private List<Vector2> _velocityList = new();

        private bool _isRewinding;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_isRewinding)
            {
                Rewind();
            }
            else
            {
                Record();
            }
        }

        private void Record()
        {
            if (_velocityList.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
            {
                _velocityList.RemoveAt(_velocityList.Count - 1);
            }

            _velocityList.Insert(0, _rigidbody2D.velocity);
        }

        private void Rewind()
        {
            if (_velocityList.Count > 0)
            {
                _rigidbody2D.velocity = -_velocityList[0];
                _velocityList.RemoveAt(0);
            }
            else
            {
                _isRewinding = false;
            }
        }

        public void StartRewind()
        {
            _isRewinding = true;
        }
    }
}