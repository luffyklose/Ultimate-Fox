////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: LOS.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Class for enemy's LOS
//Revision History:
//12/10/2021: Update enemy's LOS list when the trigger colliding with other objects
//12/12/2021: Modify negligible object
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[System.Serializable]
public class LOS : MonoBehaviour
{
    [Header("Detection Properties")] 
    public Collider2D collidesWith; // debug
    public ContactFilter2D contactFilter;
    public List<Collider2D> colliderList;

    private PolygonCollider2D LOSCollider;

    // Start is called before the first frame update
    void Start()
    {
        LOSCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        //remove null objects
        foreach (var collider in colliderList)
        {
            if (collider == null)
                colliderList.Remove(collider);
        }
    }

    void FixedUpdate()
    {
        Physics2D.GetContacts(LOSCollider, contactFilter, colliderList);
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //ignore object which could destroy like fireball/gem/bullet
        if (!other.gameObject.CompareTag("Bullet") && !other.gameObject.CompareTag("Fireball") && !other.gameObject.CompareTag("Item"))
        {
            collidesWith = other;
            //Debug.Log(other+" add");
        }
    }
}
