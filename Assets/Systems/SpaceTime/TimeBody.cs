using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.SpaceTime
{
    public class TimeBody : MonoBehaviour
    {
        [SerializeField] private List<PositionRotation> positionRotations = new List<PositionRotation>();
        private Transform _transform;
        private Rigidbody2D _rb;
        private bool _isRewinding;

        private void Awake()
        {
            _transform = transform;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isRewinding) Rewind();
        }

        private void FixedUpdate()
        {
            
            if (!_isRewinding) Record();
        }

        private void Record()
        {
            if (positionRotations.Count > Mathf.Round(5f / Time.fixedDeltaTime))
            {
                positionRotations.RemoveAt(positionRotations.Count - 1);
            }

            positionRotations.Insert(0, new PositionRotation(_transform.position, _transform.rotation));
        }

        private void Rewind()
        {
            if (positionRotations.Count > 0)
            {
                _transform.SetPositionAndRotation(positionRotations[0].Position, positionRotations[0].Rotation);
                positionRotations.RemoveAt(0);
            }
            else
            {
                StopRewind();
            }
        }

        public void StartRewind()
        {
            _isRewinding = true;
            if (_rb) _rb.isKinematic = true;
        }

        public void StopRewind()
        {
            _isRewinding = false;
            if (_rb) _rb.isKinematic = false;
        }
    }

    [Serializable]
    public class PositionRotation
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public PositionRotation(Vector3 position, Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }
    }
}