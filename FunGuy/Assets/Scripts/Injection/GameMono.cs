using Game;
using Roots;
using Rotation;
using UnityEngine;

namespace Injection
{
    public class GameMono : MonoBehaviour
    {
        private void Start()
        {
            RootsNetwork.INSTANCE.Init();
            GameManager.INSTANCE.Init();
            RotationManager.INSTANCE.Init();
        }

        private void OnDestroy()
        {
            RootsNetwork.INSTANCE.Destroy();
            GameManager.INSTANCE.Destroy();
            RotationManager.INSTANCE.Destroy();
        }
    }
}