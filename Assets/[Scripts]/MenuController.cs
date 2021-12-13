////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: MenuController.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Class for Main Menu
//Revision History:
//12/12/2021: Implement feature of entering game scene
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public void OnStartButton_Pressed()
    {
        SceneManager.LoadScene("Main");
    }
}
