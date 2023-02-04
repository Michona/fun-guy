using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update rotation
        var eulerAngles = RotationState.Instance.Rotation.eulerAngles;
        transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);
        
        float dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            rb.velocity = new Vector2(rb.velocity.x, 17f);
        }
    }

    private bool CanJump()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
