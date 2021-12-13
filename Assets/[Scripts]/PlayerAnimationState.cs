////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: PlayerAnimationState.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Enum of player animation state
//Revision History:
//12/10/2021: Create enum
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerAnimationState 
{
    IDLE,
    RUN,
    JUMP,
    NUM_OF_ANIMATION_STATES
}
