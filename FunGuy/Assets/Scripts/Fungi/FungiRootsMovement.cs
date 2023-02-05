using Event;
using Game;
using Roots;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Fungi
{
    public class FungiRootsMovement : MonoBehaviour
    {
        [SerializeField] public string PID;

        private Vector2 _aimDirection = Vector2.up;
        private bool _isSproutPressed = false;

        // Start is called before the first frame update
        private void Start()
        {
            InvokeRepeating(nameof(CheckDirection), 0f, GlobalConst.RootSpawnInterval);
        }

        public void onAim(InputAction.CallbackContext context)
        {
            _aimDirection = context.ReadValue<Vector2>();
        }

        public void onSprout(InputAction.CallbackContext context)
        {
            var triggered = context.action.triggered;
            if (triggered && !_isSproutPressed && GameManager.INSTANCE.Players[PID].CanRoot)
            {
                /* initial click */
                var currentPosition = transform.position;
                EventBus<StartRootingEvent>.Raise(new StartRootingEvent
                {
                    PID = PID,
                    Position = new Vector2(currentPosition.x, currentPosition.y)
                });
            }
            else if (!triggered && _isSproutPressed)
            {
                /* triggering was released. */
                EventBus<StopRootingEvent>.Raise(new StopRootingEvent
                {
                    PID = PID,
                });
            }

            _isSproutPressed = triggered;
        }

        private void CheckDirection()
        {
            if (_isSproutPressed && GameManager.INSTANCE.Players[PID].CanRoot)
            {
                RootsNetwork.INSTANCE.Connect(PID, _aimDirection);
            }
        }
    }
}