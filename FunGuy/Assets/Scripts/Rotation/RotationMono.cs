using UnityEngine;
using Util;

namespace Rotation
{
    public class RotationMono : MonoBehaviour
    {
        private void Start()
        {
            RotationManager.INSTANCE.Rotation = transform.rotation;
            InvokeRepeating(nameof(Rotate), GlobalConst.RotationInterval, GlobalConst.RotationInterval);
        }

        private void Rotate() => RotationManager.INSTANCE.Rotate();

        private void Update()
        {
            RotationManager.INSTANCE.Update();
            transform.rotation = RotationManager.INSTANCE.Rotation;
        }
    }
}