using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuMono: MonoBehaviour
    {
        private void Update()
        {
            var gamepad = Keyboard.current;
            if (gamepad.tKey.isPressed)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}