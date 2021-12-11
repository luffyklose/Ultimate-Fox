using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;
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

    [Header("UI")] 
    public Text lifeCount;

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

        dustTrail = GetComponentInChildren<ParticleSystem>();

        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        HP = maxHP;
        lifeCount.text = "x " + HP.ToString();
    }

    void Update()
    {
        
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
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

                //rigidbody.AddForce(new Vector2(horizontalMoveForce, 0.0f) * mass);
            }
        }

    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

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
    }

    private void Death()
    {
        rigidbody.velocity = new Vector2(10.0f, 20.0f);
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void GetHit()
    {
        HP--;
        lifeCount.text = "x " + HP.ToString();
        if (HP <= 0)
        {
            Death();
        }
        else
        {
            Debug.Log("Diao Xue");
            if (isFacingRight)
            {
                Debug.Log("右边画一道彩虹");
                //rigidbody.velocity = new Vector2(-50.0f, 50.0f);
                rigidbody.AddForce(new Vector2(-100.0f,-100.0f));
            }
            else
            {
                Debug.Log("左边画个郭富城");
                //rigidbody.velocity = new Vector2(50.0f, 50.0f);
                rigidbody.AddForce(new Vector2(100.0f,100.0f));
            }
        }
    }

    public void DecreseHP()
    {
        HP--;
        lifeCount.text = "x " + HP.ToString();
    }

    // EVENTS

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
            if (hit1 || hit2 || hit3)
            {
                Debug.Log("Hit!");
                other.gameObject.GetComponent<EnemyController>().Death();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 20.0f);
            }
            else
            {
                Debug.Log("Not Hit!");
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
