////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: BigCastle.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for castles
//Revision History:
//12/12/2021: Implement feature of entering game over scene
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigCastle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Win");
            GameObject.Find("GameController").GetComponent<LevelData>().isWin = true;
            GameObject.Find("GameController").GetComponent<LevelData>().Gem =
                other.GetComponent<PlayerBehaviour>().GetGemCount();
            SceneManager.LoadScene("GameOver");
        }
    }
}
