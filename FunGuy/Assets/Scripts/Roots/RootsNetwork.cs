using System;
using System.Collections.Generic;
using Event;
using Game;
using Game.Data;
using Injection;
using JetBrains.Annotations;
using Roots.Data;
using UnityEngine;
using Joint = Roots.Data.Joint;

namespace Roots
{
// TODO: Rename!
    public class RootsNetwork: IEventReceiver<StartRootingEvent>, IEventReceiver<StopRootingEvent>, IGameSingleton
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
            _pivot.Add("0", null);
            _pivot.Add("1", null);
            
            EventBus<StartRootingEvent>.Register(this);
        }

        public void Destroy()
        {
            EventBus<StartRootingEvent>.UnRegister(this);
        }

        /**
         * 
         */
        [CanBeNull]
        public RootUnit Connect(string pid, Vector2 direction)
        {
            var pivot = _pivot[pid];
            if (pivot == null || GameManager.INSTANCE.GetFungi(pid).State != FungiState.Rooting)
            {
                Debug.Log("CANNOT CONNECT");
                /* We cannot connect here! */
                return null;
            }

            var id = pivot.ID + 1;
            var position = pivot.Position + (direction * 0.7f);
            var rot = Vector3.Angle(Vector3.up, position - pivot.Position);

            var isToRight = position.x >= pivot.Position.x;
            var signedRot = rot;
            if (isToRight) signedRot = -rot;

            var unit = new RootUnit(
                Vector3.Lerp(position, pivot.Position, 0.5f),
                Quaternion.Euler(0, 0, signedRot),
                pid
            );

            /* update pivot */
            _pivot[pid] = new Joint(position, id);

            return unit;
        }

        public void OnEvent(StartRootingEvent e)
        {
            _pivot[e.PID] = new Joint(e.Position, 0);
        }

        public void OnEvent(StopRootingEvent e)
        {
            _pivot[e.PID] = null;
        }
    }}