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
        if (other.gameObject.CompareTag("Player"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, playerLayerMask);
            //Debug.DrawRay(transform.position, Vector3.down * 1.0f, Color.red);
            var tempGem = Instantiate(gemPrefab,
                new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
                Quaternion.identity);
            tempGem.GetComponent<Gem>().GetComponent<Rigidbody2D>().velocity = Vector2.up * 1.0f;
            Destroy(gameObject);
        }
    }
}
