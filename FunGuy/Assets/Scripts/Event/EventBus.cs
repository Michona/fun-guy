using System;
using System.Collections.Generic;

namespace Event
{
    public interface IEvent
    {
    }

    public interface IEventReceiverBase
    {
    }

    public interface IEventReceiver<in T> : IEventReceiverBase where T : struct, IEvent
    {
        void OnEvent(T e);
    }

    public static class EventBus<T> where T : struct, IEvent
    {
        private static IEventReceiver<T>[] buffer;
        private static int count;
        private static readonly int blocksize = 256;

        private static HashSet<IEventReceiver<T>> hash;

        static EventBus()
        {
            hash = new HashSet<IEventReceiver<T>>();
            buffer = Array.Empty<IEventReceiver<T>>();
        }

        public static void Register(IEventReceiverBase handler)
        {
            count++;
            hash.Add(handler as IEventReceiver<T>);
            if (buffer.Length < count)
            {
                buffer = new IEventReceiver<T>[count + blocksize];
            }


            hash.CopyTo(buffer);
        }

        public static void UnRegister(IEventReceiverBase handler)
        {
            hash.Remove(handler as IEventReceiver<T>);
            hash.CopyTo(buffer);
            count--;
        }

        public static void Raise(T e)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i].OnEvent(e);
            }
        }
        
        public static void RaiseAsInterface(IEvent e)
        {
            Raise((T)e);
        }

        public static void Clear()
        {
            hash.Clear();
        }
    }
}