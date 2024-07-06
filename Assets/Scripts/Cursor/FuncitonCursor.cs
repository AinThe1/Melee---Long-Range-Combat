using System;
using UnityEngine;

[Serializable]
public class FuncitonCursor
{
    public static FuncitonCursor Instance = new FuncitonCursor();

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
