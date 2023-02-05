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
        private Collider2D _collFeet;
        private Vector2 _direction = Vector2.up;
        private float _gravityScale;

        private Animator _animator;

        private string PID;
        private static readonly int FungiAnimState = Animator.StringToHash("FungiAnimState");

        // Start is called before the first frame update
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collFeet = GetComponent<Collider2D>();
            _gravityScale = _rb.gravityScale;
            _animator = GetComponent<Animator>();

            PID = GetComponent<FungiRootsMovement>().PID;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.triggered && GameManager.INSTANCE.Players[PID].CanJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 17f);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (GameManager.INSTANCE.Players.ContainsKey(PID))
            {
                _direction = GameManager.INSTANCE.Players[PID].State == FungiState.Walking
                    ? context.ReadValue<Vector2>()
                    : Vector2.zero;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateGroundState();

            // Update rotation
            var eulerAngles = RotationManager.INSTANCE.Rotation.eulerAngles;
            transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);

            if (GameManager.INSTANCE.Players[PID].State == FungiState.Rooting)
            {
                _rb.velocity = Vector2.zero;
                _rb.gravityScale = 0;
            }
            else
            {
                _rb.velocity = new Vector2(_direction.x * 7f, _rb.velocity.y);
                _rb.gravityScale = _gravityScale;
            }

            /* update animation */
            var state = GameManager.INSTANCE.Players[PID].State;
            if (state == FungiState.Walking)
            {
                if (_rb.velocity == Vector2.zero) state = FungiState.Idle;
                if (!GameManager.INSTANCE.Players[PID].IsOnGround) state = FungiState.Jumping;
            }

            _animator.SetInteger(FungiAnimState, (int)state);
        }

        private void UpdateGroundState()
        {
            var bounds = _collFeet.bounds;
            var isOnGround = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, .1f, jumpableGround);

            GameManager.INSTANCE.Players[PID] = GameManager.INSTANCE.Players[PID] with
            {
                IsOnGround = isOnGround
            };
        }
    }
}