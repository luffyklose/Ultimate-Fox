////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: GameController.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for GameController
//Revision History:
//12/12/2021: Implement feature of changing scenes and setting level data
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform player;
    public Transform playerSpawnPoint;
    public bool isWin = false;

    // Start is called before the first frame update
    void Start()
    {
        player.position = playerSpawnPoint.position;
    }

}
