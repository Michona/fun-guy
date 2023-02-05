using System.Collections;
using Event;
using Fungi;
using Game;
using JetBrains.Annotations;
using Roots.Data;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Roots
{
    public class RootMono : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public RootUnit Data = null;

        [SerializeField] private GameObject SpotObject;
        [CanBeNull] private GameObject _spotInstance;

        private void Start()
        {
            StartCoroutine(StartExpirationClock());
        }

        IEnumerator StartExpirationClock()
        {
            yield return new WaitForSeconds(GlobalConst.RootExpiration);
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (Data == null) return;

            /* We hit a Wall. Just stop rooting. */
            if (col.CompareTag("Wall"))
            {
                EventBus<StopRootingEvent>.Raise(new StopRootingEvent
                {
                    PID = Data.PID
                });
            }

            /* We hit object that should destroy this Root. */
            if (col.CompareTag("Destroy"))
            {
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

            /* We hit a Fungi player. */
            if (col.CompareTag("Player") && col.friction == 0)
            {
                _spotInstance = Instantiate(SpotObject, transform);

                var fungiPid = col.GetComponent<FungiRootsMovement>().PID;
                var isYourRoot = fungiPid == Data.PID;

                if (isYourRoot)
                {
                    GameManager.INSTANCE.Players[fungiPid] = GameManager.INSTANCE.Players[fungiPid] with
                    {
                        IsOnRoot = true
                    };
                }
                else
                {
                    /* hit enemy root */
                    EventBus<TakeDamageEvent>.Raise(new TakeDamageEvent
                    {
                        PID = Data.PID
                    });
                }
            }

            /* We hit another Root. */
            if (col.CompareTag("Root"))
            {
                var otherPid = col.GetComponent<RootMono>().Data?.PID;
                var position = transform.position;
                if (otherPid != null && otherPid != Data.PID)
                {
                    /* check if close enough to delete */
                    var isHead = RootsNetwork.INSTANCE.IsCloseToHead(Data,
                        new Vector2(position.x, position.y));
                    if (isHead)
                    {
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
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Player") && col.friction == 0)
            {
                var fungiPid = col.GetComponent<FungiRootsMovement>().PID;
                GameManager.INSTANCE.Players[fungiPid] = GameManager.INSTANCE.Players[fungiPid] with
                {
                    IsOnRoot = false
                };

                if (_spotInstance != null && !_spotInstance.IsDestroyed()) Destroy(_spotInstance);
            }
        }
    }
}