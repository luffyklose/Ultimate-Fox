////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Door.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Class for enemies's bullets
//Revision History:
//12/20/2021: Implement feature of bullets moving and hitting players
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Properties")] 
    public Vector3 direction;
    public float speed;
    public float duration;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = duration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
        MoveBullet();
    }

    //Bullet moves automatically based on direction
    private void MoveBullet()
    {
        direction.z = 0.0f;
        transform.position += direction * speed * Time.deltaTime;
    }


    //Destroy when colliding with platform and player.
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Platform":
                Destroy(this.gameObject);
                break;
            case "Player":
                Destroy(this.gameObject);
                other.GetComponent<PlayerBehaviour>().GetHit();
                break;
        }
    }
}
