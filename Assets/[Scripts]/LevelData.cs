////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: LevelData.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for Level Data
//Revision History:
//12/12/2021: Use this class to store data of the level
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public bool isWin;
    public int Gem;
    
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}