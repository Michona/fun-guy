using System;
using Event;
using Injection;
using UnityEngine;
using Util;

namespace Rotation
{
    public class RotationManager : IGameSingleton
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

        private RotationManager()
        {
        }

        private static readonly Lazy<RotationManager> _instance = new(() => new RotationManager());
        public static RotationManager INSTANCE => _instance.Value;

        public void Rotate()
        {
            CurrentRotation.z += RotationDeltaDeg;
            EventBus<StopRootingEvent>.Raise(new StopRootingEvent
            {
                PID = GlobalConst.PlayerOne,
            });
            EventBus<StopRootingEvent>.Raise(new StopRootingEvent
            {
                PID = GlobalConst.PlayerTwo,
            });
        }

        public void Update()
        {
            var rotGoal = Quaternion.Euler(CurrentRotation);
            _rotation = Quaternion.Lerp(_rotation, rotGoal, RotationSpeed);
        }

        public void Init()
        {
            _rotation = Quaternion.Euler(0, 0, 0);
        }

        public void Destroy()
        {
        }
    }
}