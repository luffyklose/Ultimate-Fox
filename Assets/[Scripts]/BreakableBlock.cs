////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: BreakableBlock.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 102/12/2021
//Description : Class for Breakable blocks
//Revision History:
//12/12/2021: Implement feature of breaking block and generate gems
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public GameObject gemPrefab;
    public LayerMask playerLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if the block collides with player, check if the player is below the block, if so destroy itself and generate a gem
        if (other.gameObject.CompareTag("Player"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, playerLayerMask);
            //Debug.DrawRay(transform.position, Vector3.down * 1.0f, Color.red);
            if (hit)
            {
                var tempGem = Instantiate(gemPrefab,
                    new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
                    Quaternion.identity);
                    tempGem.GetComponent<Gem>().GetComponent<Rigidbody2D>().velocity = Vector2.up * 1.0f;
                    other.gameObject.GetComponent<PlayerBehaviour>().PlayBlockBreakSound();
                    Destroy(gameObject);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 0.6f);
    }
}
