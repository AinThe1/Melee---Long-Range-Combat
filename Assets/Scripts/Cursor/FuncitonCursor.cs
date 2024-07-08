using UnityEngine;

public class FuncitonCursor : MonoBehaviour
{
    [SerializeField] private bool _cursorLock;

    private void Start() => LockCursor();
    public void LockCursor()
    {     
        if(_cursorLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
