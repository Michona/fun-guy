using Game;
using Game.Data;
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

        private string PID;

        // Start is called before the first frame update
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _coll = GetComponent<Collider2D>();

            PID = GetComponent<FungiRootsMovement>().PID;
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
            _direction = GameManager.INSTANCE.GetFungi(PID).State == FungiState.Walking
                ? context.ReadValue<Vector2>()
                : Vector2.zero;
        }

        // Update is called once per frame
        private void Update()
        {
            // Update rotation
            var eulerAngles = RotationManager.INSTANCE.Rotation.eulerAngles;
            transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);

            _rb.velocity = new Vector2(_direction.x * 7f, _rb.velocity.y);
        }

        private bool CanJump()
        {
            if (GameManager.INSTANCE.GetFungi(PID).State == FungiState.Rooting) return false;

            var bounds = _coll.bounds;
            return Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        }
    }
}