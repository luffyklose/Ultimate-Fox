////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Player.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/12/2021
//Description : Class for Player
//Revision History:
//12/10/2021:
//Implement basic movement
//Implement touch control
//12/11/2021:
//Implement basic attack;
//Set basic data;
//Implement HP and score UI display;
//Implement hurt and death;
//12/12/2021:
//Add sound effect;
//Add feature of shooting fireball
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Touch Input")] 
    public Joystick joystick;
    [Range(0.01f, 1.0f)]
    public float sensitivity;
    
    [Header("Movement")] 
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public LayerMask enemyLayerMask;
    [Range(0.1f, 0.9f)]
    public float airControlFactor;

    [Header("Animation")] 
    public PlayerAnimationState state;

    [Header("Sound FX")] 
    public List<AudioSource> audioSources;
    public AudioSource jumpSound;
    public AudioSource hitSound;
    public AudioSource getGemSound;
    public AudioSource stampEnemySound;
    public AudioSource blockBreakSound;
    

    [Header("Dust Trail")] 
    public ParticleSystem dustTrail;
    public Color dustTrailColour;

    [Header("Screen Shake Properties")] 
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float shakeDuration;
    public float shakeTimer;
    public bool isCameraShaking;

    private Rigidbody2D rigidbody;
    private Animator animatorController;

    [Header("Attribute")]
    public int maxHP;
    private int HP;
    private bool isFacingRight = true;
    private bool isInvincible = false;
    public float invincibleTime;
    private float invincibleCounter = 0.0f;
    private int gemCount = 0;

    [Header("Attack")] 
    public GameObject fireballPrefab;
    public float fireballFlySpeed;
    public float fireCD;
    public bool canFire = true;
    private float fireCounter;
    public Transform fireballSpawn;

    [Header("UI")] 
    public Text lifeCount;
    public Text gemCountText;

    // Start is called before the first frame update
    void Start()
    {
        isCameraShaking = false;
        shakeTimer = shakeDuration;

        rigidbody = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();

        // Assign Sounds
        audioSources = GetComponents<AudioSource>().ToList();
        jumpSound = audioSources[0];
        hitSound = audioSources[1];
        getGemSound = audioSources[2];
        stampEnemySound = audioSources[3];
        blockBreakSound = audioSources[4];

        dustTrail = GetComponentInChildren<ParticleSystem>();

        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        HP = maxHP;
        lifeCount.text = "x " + HP.ToString();
        gemCountText.text = "x " + gemCount.ToString();
    }

    void Update()
    {
        Fire();
        if(isInvincible)
        {
            invincibleCounter += Time.deltaTime;
            if (invincibleCounter >= invincibleTime)
            {
                invincibleCounter = 0.0f;
                isInvincible = false;
            }
        }

    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isInvincible)
        {
            Move();
        }

        CheckIfGrounded();

        // Camera Shake Control
        if (isCameraShaking)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f) // timed out
            {
                perlin.m_AmplitudeGain = 0.0f;
                shakeTimer = shakeDuration;
                isCameraShaking = false; 
            }
        }
    }

    //Get input of moving and jump, move the player and set correct animation
    private void Move()
    {
        float x = (Input.GetAxisRaw("Horizontal") + joystick.Horizontal) * sensitivity ;

        if (isGrounded)
        {
            // Keyboard Input
            float y = (Input.GetAxisRaw("Vertical") + joystick.Vertical) * sensitivity;
            float jump = Input.GetAxisRaw("Jump") + ((UIController.jumpButtonDown) ? 1.0f : 0.0f);

            // jump activated
            if (jump > 0)
            {
                jumpSound.Play();
            }

            // Check for Flip

            if (x != 0)
            {
                x = FlipAnimation(x);
                animatorController.SetInteger("AnimationState", (int) PlayerAnimationState.RUN); // RUN State
                state = PlayerAnimationState.RUN;
                CreateDustTrail();
            }
            else
            {
                animatorController.SetInteger("AnimationState", (int)PlayerAnimationState.IDLE); // IDLE State
                state = PlayerAnimationState.IDLE;
            }

            float horizontalMoveForce = x * horizontalForce;
            float jumpMoveForce = jump * verticalForce; 

            /*float mass = rigidbody.mass * rigidbody.gravityScale;


            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);
            rigidbody.velocity *= 0.99f; // scaling / stopping hack*/

            rigidbody.velocity = new Vector2(horizontalMoveForce, jumpMoveForce);
        }
        else // Air Control
        {
            animatorController.SetInteger("AnimationState", (int)PlayerAnimationState.JUMP); // JUMP State
            state = PlayerAnimationState.JUMP;

            if (x != 0)
            {
                x = FlipAnimation(x);

                float horizontalMoveForce = x * horizontalForce * airControlFactor;
                float mass = rigidbody.mass * rigidbody.gravityScale;

                rigidbody.AddForce(new Vector2(horizontalMoveForce, 0.0f) * mass);
            }
        }

    }

    //Get fire input,  generate and initialize fireball, update fire counter
    public void Fire()
    {
        //float fire = (Input.GetKeyDown(KeyCode.A) || ((UIController.attackButtonDown)) ? 1.0f : 0.0f);
        if (Input.GetKeyDown(KeyCode.Z) || UIController.attackButtonDown)
        {
            //Debug.Log("Fire");
            if (canFire)
            {
                var tempFireball = Instantiate(fireballPrefab, fireballSpawn.position, quaternion.identity);
                tempFireball.GetComponent<Fireball>().direction =
                    Vector3.Normalize(new Vector3(fireballSpawn.position.x - transform.position.x, 0.0f, 0.0f));
                if (tempFireball.GetComponent<Fireball>().direction.x < 0.0f)
                {
                    tempFireball.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                //tempFireball.SetVelocity(new Vector2(fireballFlySpeed,0.0f));
                canFire = false;
                //Debug.Log(tempFireball);
            }
        }

        if (!canFire)
        {
            fireCounter += Time.deltaTime;
            if (fireCounter >= fireCD)
            {
                fireCounter = 0.0f;
                canFire = true;
            }
        }
    }

    //check if the player is on ground using ray cast
    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    //flip animation in x-axis
    private float FlipAnimation(float x)
    {
        // depending on direction scale across the x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        if (x == -1)
        {
            isFacingRight = false;
        }
        else
        {
            isFacingRight = true;
        }
        return x;
    }

    private void CreateDustTrail()
    {
        dustTrail.GetComponent<Renderer>().material.SetColor("_Color", dustTrailColour);
        dustTrail.Play();
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking = true;
        Debug.Log("Shake!");
    }

    //Player falling when dying, store level data and go to game over scene
    private void Death()
    {
        rigidbody.velocity = new Vector2(10.0f, 20.0f);
        GetComponent<Collider2D>().isTrigger = true;
        GameObject.Find("GameController").GetComponent<LevelData>().isWin = true;
        GameObject.Find("GameController").GetComponent<LevelData>().Gem = GetGemCount();
        SceneManager.LoadScene("GameOver");
    }

    //Update HP, set player invincible and step back
    public void GetHit()
    {
        DecreseHP();
        isInvincible = true;
        if (HP <= 0)
        {
            Death();
        }
        else
        {
            //Debug.Log("Diao Xue");
            if (isFacingRight)
            {
                //Debug.Log("?????????????????????");
                rigidbody.velocity = new Vector2(-10.0f, 10.0f);
                //rigidbody.AddForce(new Vector2(-100.0f, -100.0f), ForceMode2D.Impulse);
            }
            else
            {
                //Debug.Log("?????????????????????");
                rigidbody.velocity = new Vector2(10.0f, 10.0f);
                //rigidbody.AddForce(new Vector2(100.0f, 100.0f), ForceMode2D.Impulse);
            }
        }
    }

    //Decrese HP
    public void DecreseHP()
    {
        HP--;
        lifeCount.text = "x " + HP.ToString();
    }

    //Get a gem and update UI
    public void GetGem()
    {
        gemCount++;
        gemCountText.text = "x " + gemCount.ToString();
        getGemSound.Play();
    }

    //Play sound when player breaks a block
    public void PlayBlockBreakSound()
    {
        blockBreakSound.Play();
    }

    // EVENTS

    //Detect if player stamp enemies or get hit by enemies
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(other.transform);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            //RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down,
                //groundRadius, enemyLayerMask);
                RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(groundOrigin.position.x, groundOrigin.position.y),
                    Vector2.down, groundRadius, enemyLayerMask);
                RaycastHit2D hit2 = Physics2D.Raycast(
                    new Vector2(groundOrigin.position.x + 0.2f, groundOrigin.position.y),
                    Vector2.down, groundRadius, enemyLayerMask);
                RaycastHit2D hit3 = Physics2D.Raycast(
                    new Vector2(groundOrigin.position.x - 0.2f, groundOrigin.position.y),
                    Vector2.down, groundRadius, enemyLayerMask);
            if ((hit1 || hit2 || hit3) && other.gameObject.GetComponent<EnemyController>().IsAlive)
            {
                //Debug.Log("Hit!");
                other.gameObject.GetComponent<EnemyController>().Death();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 20.0f);
                stampEnemySound.Play();
            }
            else if(!isInvincible)
            {
                //Debug.Log("Not Hit!");
                GetHit();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            hitSound.Play();
            ShakeCamera();
        }
    }

    public int GetGemCount()
    {
        return gemCount;
    }

   

    // UTILITIES

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(groundOrigin.position.x, groundOrigin.position.y,groundOrigin.position.z), Vector3.down * groundRadius);
        Gizmos.DrawRay(new Vector3(groundOrigin.position.x + 0.2f, groundOrigin.position.y, groundOrigin.position.z),
            Vector3.down * groundRadius);
        Gizmos.DrawRay(new Vector3(groundOrigin.position.x - 0.2f, groundOrigin.position.y, groundOrigin.position.z),
            Vector3.down * groundRadius);
    }

}
