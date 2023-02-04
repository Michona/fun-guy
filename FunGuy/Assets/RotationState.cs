using System;
using UnityEngine;

public class RotationState
{
    // private Vector2 upDir;
    private Tuple<Vector2, Vector2> rotatorMatrix;
    private Tuple<Vector2, Vector2> rotationMatrix;

    private RotationState()
    {
        var identityMatrix = new Tuple<Vector2, Vector2>(new Vector2(1, 0), new Vector2(0, 1));
        rotationMatrix = identityMatrix;
        var rotation90 = new Tuple<Vector2, Vector2>(new Vector2(0, 1), new Vector2(-1, 0));
        rotatorMatrix = rotation90;
    }

    private static readonly Lazy<RotationState> _instance = new(() => new RotationState());
    public static RotationState Instance => _instance.Value;

    public static Vector2 RotateVector2(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    
    // Vector2 RotateVector(Vector2 vec)
    //     => new(vec.x * rotatorMatrix.Item1.x + vec.y * rotatorMatrix.Item1.y,
    //         vec.x * rotatorMatrix.Item2.x + vec.y * rotatorMatrix.Item2.y);

    // Tuple<Vector2, Vector2> MultiplyMatrices(Tuple<Vector2, Vector2> mat1, Tuple<Vector2, Vector2> mat2)
    //     => new(
    //         new Vector2(mat1.Item1.x * mat2.Item1.x + mat1.Item2.x * mat2.Item1.y,
    //             mat1.Item1.y * mat2.Item1.x + mat1.Item2.y * mat2.Item1.y),
    //         new Vector2(mat1.Item1.x * mat2.Item2.x + mat1.Item2.x * mat2.Item2.y,
    //             mat1.Item1.y * mat2.Item2.x + mat1.Item2.y * mat2.Item2.y));

    public void RotateState()
    {
        Debug.Log("RotationState: RotateState");
        // Debug.Log(Physics2D.gravity);
        // Physics2D.gravity = RotateVector(Physics2D.gravity);
        // upDir = RotateVector(upDir);
        // Debug.Log(Physics2D.gravity);
        // transform.eulerAngles 
    }

    // void Update()
    // {
    //     throw new NotImplementedException();
    // }
}