using System.Collections.Generic;
using Event;
using Unity.VisualScripting;
using UnityEngine;

namespace Roots
{
    public class RootSpawner : MonoBehaviour, IEventReceiver<SpawnRootEvent>, IEventReceiver<DestroyRootEvent>
    {
        [SerializeField] private GameObject PlayerOneRoot;

        [SerializeField] private GameObject PlayerTwoRoot;

        private readonly List<GameObject> _roots = new();

        private void Start()
        {
            EventBus<SpawnRootEvent>.Register(this);
            EventBus<DestroyRootEvent>.Register(this);
        }

        private void OnDestroy()
        {
            EventBus<SpawnRootEvent>.UnRegister(this);
            EventBus<DestroyRootEvent>.Register(this);
        }

        /** Spawns a new root unit prefab. */
        public void OnEvent(SpawnRootEvent e)
        {
            var rootUnit = e.Unit;

            var rootToSpawn = e.PID == "0" ? PlayerOneRoot : PlayerTwoRoot;

            var rootObject = Instantiate(rootToSpawn, new Vector3(rootUnit.Position.x, rootUnit.Position.y, 0f),
                rootUnit.Rotation);
            rootObject.GetComponent<RootMono>().Data = e.Unit;
            rootObject.transform.parent = transform;

            _roots.Add(rootObject);
        }

        public void OnEvent(DestroyRootEvent e)
        {
            _roots.ForEach((root) =>
            {
                if (!root.IsDestroyed())
                {
                    var data = root.GetComponent<RootMono>().Data;
                    if (data != null && data.GroupID == e.GroupID && data.PID == e.PID)
                    {
                        /* should destroy */
                        Destroy(root);
                    }
                }
            });
        }
    }
}