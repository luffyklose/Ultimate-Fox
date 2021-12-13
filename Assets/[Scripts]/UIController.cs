////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: UIController.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for HUD
//Revision History:
//12/10/2021: Add UI of jumping and moving
//12/12/2021: Add UI of firing
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("On Screen Controls")] 
    public GameObject onScreenControls;

    [Header("Button Control Events")]
    public static bool jumpButtonDown;
    public static bool attackButtonDown;

    // Start is called before the first frame update
    void Start()
    {
        CheckPlatform();
    }

    // PRIVATE METHODS

    private void CheckPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.WindowsEditor:
                onScreenControls.SetActive(true);
                break;
            default:
                onScreenControls.SetActive(false);
                break;
        }
    }

    // EVENT FUNCTIONS

    public void OnJumpButton_Down()
    {
        jumpButtonDown = true;
    }

    public void OnJumpButton_Up()
    {
        jumpButtonDown = false;
    }

    public void OnAttackButton_Down()
    {
        attackButtonDown = true;
    }

    public void OnAttackButton_Up()
    {
        attackButtonDown = false;
    }
}
