using System;
using UnityEngine;

namespace Rotation
{
    public class RotationState
    {
        public Vector3 CurrentRotation = new(0, 0, 0);

        private const float RotationDeltaDeg = -90;
        private const float RotationSpeed = .01f;

        private bool _isRotationInitialized;
        private Quaternion _rotation;

        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                // not thread safe
                if (_isRotationInitialized)
                    return;
                _isRotationInitialized = true;
                _rotation = value;
            }
        }

        private RotationState()
        {
            _rotation = Quaternion.Euler(0, 0, 0);
        }

        private static readonly Lazy<RotationState> InstanceLazy = new(() => new RotationState());
        public static RotationState Instance => InstanceLazy.Value;

        public void Rotate() => CurrentRotation.z += RotationDeltaDeg;

        public void Update()
        {
            var rotGoal = Quaternion.Euler(CurrentRotation);
            _rotation = Quaternion.Lerp(_rotation, rotGoal, RotationSpeed);
        }
    }
}