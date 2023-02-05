using System;
using System.Collections.Generic;
using Event;
using Game;
using Game.Data;
using Injection;
using JetBrains.Annotations;
using Roots.Data;
using UnityEngine;
using Util;
using Joint = Roots.Data.Joint;

namespace Roots
{
    public class RootsNetwork : IEventReceiver<StartRootingEvent>, IGameSingleton
    {
        /**
         * The current joint that is ready to spread!
         */
        [ItemCanBeNull] private readonly Dictionary<string, Joint> _pivot = new();

        private RootsNetwork()
        {
        }

        private static readonly Lazy<RootsNetwork> _instance = new(() => new RootsNetwork());
        public static RootsNetwork INSTANCE => _instance.Value;


        public void Init()
        {
            _pivot.TryAdd(GlobalConst.PlayerOne, null);
            _pivot.TryAdd(GlobalConst.PlayerTwo, null);
            EventBus<StartRootingEvent>.Register(this);
        }

        public void Destroy()
        {
            _pivot.Remove(GlobalConst.PlayerOne);
            _pivot.Remove(GlobalConst.PlayerTwo);
            EventBus<StartRootingEvent>.UnRegister(this);
        }

        public bool IsCloseToHead(RootUnit root, Vector2 position)
        {
            var pivot = _pivot[root.PID];
            if (pivot == null) return false;

            if (root.GroupID != pivot.ID) return false;
            return (Vector2.Distance(pivot.Position, position) <= GlobalConst.CollisionProximity);
        }

        /**
         * 
         */
        public void Connect(string pid, Vector2 direction)
        {
            var pivot = _pivot[pid];
            if (pivot == null || GameManager.INSTANCE.Players[pid].State != FungiState.Rooting)
            {
                /* We cannot connect here! */
                return;
            }

            var updatedDirection = GetUpdatedDirection(pivot.Direction, direction);
            var position = pivot.Position + (updatedDirection * GlobalConst.RootSpawnDistanceMultiplier);
            var angle = GetRotationAngle(pivot.Position, position);

            var unit = new RootUnit(
                Vector3.Lerp(position, pivot.Position, 0.5f),
                Quaternion.Euler(0, 0, angle),
                pid,
                pivot.ID,
                pivot.isBase
            );

            /* update pivot */
            _pivot[pid] = new Joint(position, pivot.ID, updatedDirection, false);

            /* we can spawn now. */
            EventBus<SpawnRootEvent>.Raise(new SpawnRootEvent
            {
                PID = pid,
                Unit = unit
            });
        }

        private static Vector2 DegreeToVector2(Vector2 from, float degree) =>
            (Vector2)(Quaternion.Euler(0, 0, degree) * from);

        private static float GetRotationAngle(Vector2 initialPosition, Vector2 targetPosition)
        {
            var rot = Vector3.Angle(Vector2.up, targetPosition - initialPosition);
            var isToRight = targetPosition.x >= initialPosition.x;
            var signedRot = rot;
            if (isToRight) signedRot = -rot;
            return signedRot;
        }

        private static Vector2 GetUpdatedDirection(Vector2 pivotDirection, Vector2 direction)
        {
            /* change in degrees between the 2 directions. */
            var change = Vector3.Angle(pivotDirection, direction);

            var updatedDirection = direction;
            if (change >= GlobalConst.TurnRateAngle)
            {
                var upper = DegreeToVector2(pivotDirection, GlobalConst.TurnRateAngle);
                var lower = DegreeToVector2(pivotDirection, -GlobalConst.TurnRateAngle);

                /* we use either upper or lower boundary which are exactly turn-rate distance from pivotDirection. */
                updatedDirection = Vector3.Angle(direction, upper) >= Vector3.Angle(direction, lower) ? lower : upper;
            }

            return updatedDirection;
        }

        public void OnEvent(StartRootingEvent e)
        {
            var pivot = _pivot[e.PID];
            var originID = pivot?.ID ?? 0;
            _pivot[e.PID] = new Joint(e.Position, originID + 1, Vector2.up, true);
        }
    }
}