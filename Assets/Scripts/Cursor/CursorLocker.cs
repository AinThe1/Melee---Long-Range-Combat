using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    [SerializeField] private bool _cursorLock;

    private void Update()
    {
        if(_cursorLock)
            LockCursor();
    }

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
