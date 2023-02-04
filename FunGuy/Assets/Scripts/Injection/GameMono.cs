using Game;
using Roots;
using UnityEngine;

namespace Injection
{
    public class GameMono : MonoBehaviour
    {
        private void Start()
        {
            RootsController.INSTANCE.Init();
            GameManager.INSTANCE.Init();
        }

        private void OnDestroy()
        {
            RootsController.INSTANCE.Destroy();
            GameManager.INSTANCE.Destroy();
        }
    }
}