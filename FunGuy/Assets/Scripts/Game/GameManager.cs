using System;
using System.Collections.Generic;
using Event;
using Game.Data;
using Injection;
using UnityEngine.SceneManagement;
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
            Players.TryAdd(GlobalConst.PlayerOne, new FungiData());
            Players.TryAdd(GlobalConst.PlayerTwo, new FungiData());
            EventBus<StartRootingEvent>.Register(this);
            EventBus<StopRootingEvent>.Register(this);
            EventBus<TakeDamageEvent>.Register(this);
        }

        public void Destroy()
        {
            Players.Remove(GlobalConst.PlayerOne);
            Players.Remove(GlobalConst.PlayerTwo);
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
            var timeNow = DateTime.Now.Second;

            if (timeNow - fungi.LastDamageTimeStamp >= GlobalConst.InvulnerabilityTime)
            {
                if (fungi.Health <= 1)
                {
                    // GAME OVER!
                    SceneManager.LoadSceneAsync(0);
                }
                else
                {
                    Players[e.PID] = fungi with
                    {
                        Health = fungi.Health - 1,
                        LastDamageTimeStamp = timeNow
                    };

                    EventBus<HealthLostEvent>.Raise(new HealthLostEvent
                    {
                        PID = e.PID
                    });
                }
            }
        }
    }
}