using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SceneRotation : MonoBehaviour
{
    Vector3 CurrentRotation = new(0, 0, 0);
    float RotationDeltaDeg = -90;
    float RotationSpeed = .01f;

    private void Start()
    {
        InvokeRepeating(nameof(Rotate), 5, 5);
    }

    private void Rotate() => CurrentRotation.z += RotationDeltaDeg;

    private void Update()
    {
        var rotGoal = Quaternion.Euler(CurrentRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotGoal, RotationSpeed);
    }
}