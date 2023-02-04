using System;
using System.Collections.Generic;
using Event;
using Game.Data;
using Injection;

namespace Game
{
    public class GameManager : IEventReceiver<StartRootingEvent>, IEventReceiver<StopRootingEvent>, IGameSingleton
    {
        private readonly Dictionary<string, FungiData> _players = new();

        private GameManager()
        {
        }

        private static readonly Lazy<GameManager> _instance = new(() => new GameManager());
        public static GameManager INSTANCE => _instance.Value;

        public void Init()
        {
            // TODO: Constants!
            _players.Add("0", new FungiData(FungiState.Walking, null));
            _players.Add("1", new FungiData(FungiState.Walking, null));

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
        public FungiData GetFungi(string pid) => _players[pid];

        public void OnEvent(StartRootingEvent e)
        {
            var pid = e.PID;
            _players[pid] = _players[pid] with
            {
                State = FungiState.Rooting,
            };
        }

        public void OnEvent(StopRootingEvent e)
        {
            var fungi = _players[e.PID];
            _players[e.PID] = fungi with { State = FungiState.Walking };
        }
    }
}