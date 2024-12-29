using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            Application.Quit();
        }
    }
}
