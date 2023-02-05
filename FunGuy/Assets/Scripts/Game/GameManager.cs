using System;
using System.Collections.Generic;
using Event;
using Game.Data;
using Injection;

namespace Game
{
    public class GameManager : IEventReceiver<StartRootingEvent>, IEventReceiver<StopRootingEvent>, IGameSingleton
    {
        public readonly Dictionary<string, FungiData> Players = new();

        private GameManager()
        {
        }

        private static readonly Lazy<GameManager> _instance = new(() => new GameManager());
        public static GameManager INSTANCE => _instance.Value;

        public void Init()
        {
            // TODO: Constants!
            Players.Add("0", new FungiData(FungiState.Walking, false, false));
            Players.Add("1", new FungiData(FungiState.Walking, false, false));

            EventBus<StartRootingEvent>.Register(this);
            EventBus<StopRootingEvent>.Register(this);
        }

        public void Destroy()
        {
            EventBus<StartRootingEvent>.UnRegister(this);
            EventBus<StopRootingEvent>.UnRegister(this);
        }

        /**
         * TODO: docs 
         */
        public FungiData GetFungi(string pid) => Players[pid];

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
    }
}