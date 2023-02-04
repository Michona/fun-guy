using System.Collections.Generic;
using Event;
using UnityEngine;

namespace Roots
{
    public class RootSpawner : MonoBehaviour, IEventReceiver<SpawnRootEvent>
    {
        [SerializeField] private GameObject RootUnit;

        private List<GameObject> roots = new();

        private void Start()
        {
            EventBus<SpawnRootEvent>.Register(this);
        }

        private void OnDestroy()
        {
            EventBus<SpawnRootEvent>.UnRegister(this);
        }

        /** Spawns a new root unit prefab. */
        public void OnEvent(SpawnRootEvent e)
        {
            var rootUnit = e.Unit;
            var rootObject = Instantiate(RootUnit, new Vector3(rootUnit.Position.x, rootUnit.Position.y, 0f),
                rootUnit.Rotation);
            roots.Add(rootObject);
        }
    }
}