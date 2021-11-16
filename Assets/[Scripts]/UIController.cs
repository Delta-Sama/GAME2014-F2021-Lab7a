using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static bool jump = false;

    public void OnJumpButton_Down()
    {
        jump = true;

    }

    public void OnJumpButton_Up()
    {
        jump = false;
        Invoke("StopJump", 0.05f);
    }

    private void StopJump()
    {
        
    }
}
