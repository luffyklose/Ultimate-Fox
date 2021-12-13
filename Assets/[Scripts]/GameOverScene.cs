////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: GameOverScene.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for GameOverScene
//Revision History:
//12/12/2021: Implement feature of changing scenes and setting level data
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    public Text WinText;
    public Text ScoreText;

    private AudioSource audioSource;

    // Based on Leveldata set text content and BGM
    void Start()
    {
        bool isWin = GameObject.Find("GameController").GetComponent<LevelData>().isWin;
        if (isWin)
        {
            WinText.text = "YOU WIN";
        }
        else
        {
            WinText.text = "GAME OVER";
        }

        ScoreText.text = "Score: " + GameObject.Find("GameController").GetComponent<LevelData>().Gem.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}