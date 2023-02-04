using UnityEngine;

namespace Rotation
{
    public class SceneRotation : MonoBehaviour
    {
        private void Start()
        {
            RotationState.Instance.Rotation = transform.rotation;
            InvokeRepeating(nameof(Rotate), 5, 5);
        }

        private void Rotate() => RotationState.Instance.Rotate();

        private void Update()
        {
            RotationState.Instance.Update();
            transform.rotation = RotationState.Instance.Rotation;
        }
    }
}