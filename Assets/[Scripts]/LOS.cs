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
        if (!other.gameObject.CompareTag("Bullet") && !other.gameObject.CompareTag("Fireball"))
        {
            collidesWith = other;
            //Debug.Log(other+" add");
        }
    }
}
