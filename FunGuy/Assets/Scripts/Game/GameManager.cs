using System;
using System.Collections.Generic;
using Event;
using Game.Data;
using Injection;
using Util;

namespace Game
{
    public class GameManager : IEventReceiver<StartRootingEvent>, IEventReceiver<StopRootingEvent>,
        IEventReceiver<TakeDamageEvent>, IGameSingleton
    {
        public readonly Dictionary<string, FungiData> Players = new();

        private GameManager()
        {
        }

        private static readonly Lazy<GameManager> _instance = new(() => new GameManager());
        public static GameManager INSTANCE => _instance.Value;

        public void Init()
        {
            Players.Add(GlobalConst.PlayerOne, new FungiData());
            Players.Add(GlobalConst.PlayerTwo, new FungiData());

            EventBus<StartRootingEvent>.Register(this);
            EventBus<StopRootingEvent>.Register(this);
            EventBus<TakeDamageEvent>.Register(this);
        }

        public void Destroy()
        {
            EventBus<StartRootingEvent>.UnRegister(this);
            EventBus<StopRootingEvent>.UnRegister(this);
            EventBus<TakeDamageEvent>.UnRegister(this);
        }

        public void OnEvent(StartRootingEvent e)
        {
            var pid = e.PID;
            Players[pid] = Players[pid] with
            {
                State = FungiState.Rooting,
            };
        }

        public void OnEvent(StopRootingEvent e)
        {
            var fungi = Players[e.PID];
            Players[e.PID] = fungi with { State = FungiState.Walking };
        }

        public void OnEvent(TakeDamageEvent e)
        {
            var fungi = Players[e.PID];
            if (DateTime.Now.Second - fungi.LastDamageTimeStamp >= GlobalConst.InvulnerabilityTime)
            {
                Players[e.PID] = fungi with
                {
                    Health = fungi.Health - 1,
                    LastDamageTimeStamp = DateTime.Now.Second
                };
            }
        }
    }
}