using Rotation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fungi
{
    public class FungiMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask jumpableGround;

        private Rigidbody2D _rb;
        private Collider2D _coll;
        private Vector2 _direction = Vector2.up;

        // Start is called before the first frame update
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _coll = GetComponent<Collider2D>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.triggered && CanJump())
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 17f);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector2>();
        }

        // Update is called once per frame
        void Update()
        {
            // Update rotation
            var eulerAngles = RotationState.Instance.Rotation.eulerAngles;
            transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);

            _rb.velocity = new Vector2(_direction.x * 7f, _rb.velocity.y);
        }

        private bool CanJump()
        {
            var bounds = _coll.bounds;
            return Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        }
    }
}