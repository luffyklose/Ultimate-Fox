////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: MovingPlatformDirection.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Enum of moving direction
//Revision History:
//12/10/2021: Create enum
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MovingPlatformDirection 
{
    HORIZONTAL,
    VERTICAL,
    DIAGONAL_UP,
    DIAGONAL_DOWN,
    NUM_OF_DIRECTIONS
}
