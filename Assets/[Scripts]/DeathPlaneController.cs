////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: DeathPlaneController.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Class for death plane
//Revision History:
//10/22/2021: Implement feature of killing enemies and player
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneController : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = playerSpawnPoint.position;
            other.gameObject.GetComponent<PlayerBehaviour>().DecreseHP();
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }

    //Kill enemy and player when colliding with them
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().DecreseHP();
            other.transform.position = playerSpawnPoint.position;
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }
}
