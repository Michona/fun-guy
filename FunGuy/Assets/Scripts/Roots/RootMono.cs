using Event;
using Fungi;
using Game;
using JetBrains.Annotations;
using Roots.Data;
using UnityEngine;

namespace Roots
{
    public class RootMono : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public RootUnit Data = null;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (Data == null) return;

            if (col.CompareTag("Wall"))
            {
                Debug.Log("WALL HIT");
                EventBus<StopRootingEvent>.Raise(new StopRootingEvent
                {
                    PID = Data.PID
                });
            }

            if (col.CompareTag("Destroy"))
            {
                Debug.Log("DESTROY HIT");
                EventBus<StopRootingEvent>.Raise(new StopRootingEvent
                {
                    PID = Data.PID
                });
                EventBus<DestroyRootEvent>.Raise(new DestroyRootEvent
                {
                    PID = Data.PID,
                    GroupID = Data.GroupID
                });
            }

            if (col.CompareTag("Player"))
            {
                Debug.Log("PLAYER HIT");
                var fungiPid = col.GetComponent<FungiRootsMovement>().PID;
                GameManager.INSTANCE.Players[fungiPid] = GameManager.INSTANCE.Players[fungiPid] with
                {
                    IsOnRoot = true
                };
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("PLAYER Exited");
                var fungiPid = col.GetComponent<FungiRootsMovement>().PID;
                GameManager.INSTANCE.Players[fungiPid] = GameManager.INSTANCE.Players[fungiPid] with
                {
                    IsOnRoot = false
                };
            }
        }
    }
}