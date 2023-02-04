using Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Roots
{
    public class FungiRootInput : MonoBehaviour
    {
        [SerializeField] private string PID;
        
        private Vector2 aimDirection = Vector2.up;
        private bool isSproutPressed = false;

        // Start is called before the first frame update
        private void Start()
        {
            InvokeRepeating(nameof(CheckDirection), 0f, 0.5f);
        }

        public void onAim(InputAction.CallbackContext context)
        {
            aimDirection = context.ReadValue<Vector2>();
        }
        
        public void onSprout(InputAction.CallbackContext context)
        {
            var triggered = context.action.triggered;
            if (triggered && !isSproutPressed)
            {
                /* initial click */
                var currentPosition = transform.position;
                EventBus<StartRootingEvent>.Raise(new StartRootingEvent
                {
                    PID = PID,
                    Position = new Vector2(currentPosition.x, currentPosition.y)
                }); 
            }
            else if (!triggered && isSproutPressed) 
            {
                /* triggering was released. */
                EventBus<StopRootingEvent>.Raise(new StopRootingEvent
                {
                    PID = PID,
                }); 
            }

            isSproutPressed = triggered;
            Debug.Log("SPROUT" + triggered);
        }

        private void CheckDirection()
        {
            if (isSproutPressed)
            {
                var rootUnit = RootsController.INSTANCE.Connect(PID, aimDirection);
                if (rootUnit != null)
                {
                    EventBus<SpawnRootEvent>.Raise(new SpawnRootEvent
                    {
                        PID = PID,
                        Unit = rootUnit
                    });
                }
            }
        }
    }
}