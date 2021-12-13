using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Fireball : MonoBehaviour
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
        MoveFireBall();
    }

    private void MoveFireBall()
    {
        direction.z = 0.0f;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Platform":
                Destroy(this.gameObject);
                break;
        }
    }
}
