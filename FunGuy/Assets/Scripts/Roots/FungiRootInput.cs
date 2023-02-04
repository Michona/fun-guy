using Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Roots
{
    public class FungiRootInput : MonoBehaviour
    {
        [SerializeField] private string PID;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating(nameof(CheckDirection), 0f, 1f);
        }

        private void CheckDirection()
        {
            var gamepad = Gamepad.current;
            if (gamepad == null)
            {
                Debug.Log("NO GAMEPAD");
                return;
            }

            if (gamepad.yButton.isPressed)
            {
                EventBus<StartRootingEvent>.Raise(new StartRootingEvent
                {
                    PID = PID,
                    Position = new Vector2(0f, 0f)
                });
            }

            if (gamepad.xButton.isPressed)
            {
                var direction = gamepad.leftStick.ReadValue();
                var rootUnit = RootsController.INSTANCE.Connect(PID, direction);

                if (rootUnit != null)
                {
                    EventBus<SpawnRootEvent>.Raise(new SpawnRootEvent
                    {
                        Unit = rootUnit
                    });
                }
            }
        }
    }
}