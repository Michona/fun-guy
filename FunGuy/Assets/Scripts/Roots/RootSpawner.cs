using System.Collections.Generic;
using Event;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Roots
{
    public class RootSpawner : MonoBehaviour, IEventReceiver<SpawnRootEvent>, IEventReceiver<DestroyRootEvent>
    {
        [SerializeField] private List<GameObject> PlayerOneRoots;
        [SerializeField] private GameObject PlayerOneBaseRoot;

        [SerializeField] private List<GameObject> PlayerTwoRoots;
        [SerializeField] private GameObject PlayerTwoBaseRoot;

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

            var randomRootIndex = (new Random()).Next(0, PlayerOneRoots.Count);
            var rootToSpawn = e.PID == "0"
                ? (rootUnit.isBase ? PlayerOneBaseRoot : PlayerOneRoots[randomRootIndex])
                : (rootUnit.isBase ? PlayerTwoBaseRoot : PlayerOneRoots[randomRootIndex]);

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