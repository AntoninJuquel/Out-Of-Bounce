using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.SpaceTime
{
    public class TimeBody : MonoBehaviour
    {
        private List<PositionRotation> _positionRotations;
        private Transform _transform;
        private Rigidbody2D _rb;
        private bool _isRewinding;

        private void Awake()
        {
            _positionRotations = new List<PositionRotation>();
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
            if (_positionRotations.Count > Mathf.Round(5f / Time.fixedDeltaTime))
            {
                _positionRotations.RemoveAt(_positionRotations.Count - 1);
            }

            _positionRotations.Insert(0, new PositionRotation(_transform.position, _transform.rotation));
        }

        private void Rewind()
        {
            if (_positionRotations.Count > 0)
            {
                _transform.SetPositionAndRotation(_positionRotations[0].Position, _positionRotations[0].Rotation);
                _positionRotations.RemoveAt(0);
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

    public class PositionRotation
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public PositionRotation(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}