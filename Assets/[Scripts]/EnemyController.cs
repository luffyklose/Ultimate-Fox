////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: EnemyController.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for doors
//Revision History:
//12/10/2021: Implement basic movement and auto attack based on LOS
//12/12/2021: Modify LOS logic when the collided object is destroyed
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Player Detection")] 
    public LOS enemyLOS;

    [Header("Movement")] 
    public float runForce;
    public Transform lookAheadPoint;
    public Transform lookInFrontPoint;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public bool isGroundAhead;

    [Header("Animation")] 
    public Animator animatorController;

    [Header("Bullet Firing")] 
    public Transform bulletSpawn;
    public float fireDelay;
    public GameObject player;
    public GameObject bulletPrefab;
    public AudioSource spitSound;

    private Rigidbody2D rigidbody;
    public GameObject gemPrefab;
    public float gemGenerateProbility;
    private bool isAlive = true;

    public bool IsAlive
    {
        get { return isAlive; }
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemyLOS = GetComponent<LOS>();
        animatorController = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerBehaviour>().gameObject;
        spitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAhead();
        LookInFront();

        if (!HasLOS())
        {
            animatorController.enabled = true;
            animatorController.Play("Run");
            MoveEnemy();
        }
        else
        {
            animatorController.enabled = false;
            FireBullet();
        }
        
    }

    //Check if enemy see player
    private bool HasLOS()
    {
        if (enemyLOS.colliderList.Count > 0)
        {
            // Case 1 enemy polygonCollider2D collides with player and player is at the top of the list
            if ((enemyLOS.collidesWith.gameObject.CompareTag("Player")) &&
                (enemyLOS.colliderList[0].gameObject.CompareTag("Player")))
            {
                return true;
            }
            // Case 2 player is in the Collider List and we can draw ray to the player
            else
            {
                foreach (var collider in enemyLOS.colliderList)
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        var hit = Physics2D.Raycast(lookInFrontPoint.position, Vector3.Normalize(collider.transform.position - lookInFrontPoint.position), 5.0f, enemyLOS.contactFilter.layerMask);
                        
                        if((hit) && (hit.collider.gameObject.CompareTag("Player")))
                        {
                            Debug.DrawLine(lookInFrontPoint.position, collider.transform.position, Color.red);
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    //Check if there's way for enemy walking
    private void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, lookAheadPoint.position, groundLayerMask);
        isGroundAhead = (hit) ? true : false;
    }

    //Check if there's any block in front of enemy
    private void LookInFront()
    {
        var hit1 = Physics2D.Linecast(transform.position, lookInFrontPoint.position, wallLayerMask);
        var hit2=Physics2D.Linecast(transform.position, lookInFrontPoint.position, groundLayerMask);
        if (hit1 || hit2)
        {
            Flip();
        }
    }

    //Enemy's movement
    private void MoveEnemy()
    {
        if (isGroundAhead)
        {
            rigidbody.AddForce(Vector2.left * runForce * transform.localScale.x);
            rigidbody.velocity *= 0.90f;
        }
        else
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    private void FireBullet()
    {
        // delay bullet firing
        if (Time.frameCount % fireDelay == 0)
        {
            var temp_bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            temp_bullet.GetComponent<BulletController>().direction = Vector3.Normalize(player.transform.position - bulletSpawn.position);
            spitSound.Play();
        }
    }


    // EVENTS

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(other.transform);
        }
        else if (other.gameObject.CompareTag("Fireball"))
        {
            Death();
            Destroy(other.gameObject);
            Debug.Log("Fireball hit");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    //Enemy falls after death
    public void Death()
    {
        if (Random.Range(0.0f,1.0f) <= gemGenerateProbility)
        {
            var tempGem = Instantiate(gemPrefab,
                new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
                Quaternion.identity);
            tempGem.GetComponent<Gem>().GetComponent<Rigidbody2D>().velocity = Vector2.up * 1.0f;
        }
        rigidbody.velocity = new Vector2(10.0f, 20.0f);
        GetComponent<Collider2D>().isTrigger = true;
        isAlive = false;
    }

    // UTILITIES

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, lookAheadPoint.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, lookInFrontPoint.position);
    }
}
