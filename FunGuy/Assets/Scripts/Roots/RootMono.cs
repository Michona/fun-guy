using Event;
using JetBrains.Annotations;
using Roots.Data;
using UnityEngine;

namespace Roots
{
    public class RootMono : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public RootUnit data = null;

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.name);
            if (data != null && col.CompareTag("Wall"))
            {
                EventBus<StopRootingEvent>.Raise(new StopRootingEvent
                {
                    PID = data.PID
                });
            }
        }
    }}