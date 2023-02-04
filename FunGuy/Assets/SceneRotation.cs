using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.R))
            Rotate();
        
        RotationState.Instance.Update();
        transform.rotation = RotationState.Instance.Rotation;
    }
}