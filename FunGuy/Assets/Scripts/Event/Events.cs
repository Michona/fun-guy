using Roots.Data;
using UnityEngine;

namespace Event
{
    public struct StartRootingEvent : IEvent
    {
        public string PID;
        public Vector2 Position;
    }

    public struct StopRootingEvent : IEvent
    {
        public string PID;
    }

    public struct SpawnRootEvent : IEvent
    {
        public string PID;
        public RootUnit Unit;
    }
}