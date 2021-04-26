using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    public ParticleSystem hurtEffect;
    public ParticleSystem fixedEffect; 
    public ParticleSystem PortalEffect;
    public ParticleSystem SpeedEffect;
    public GameObject youLose;
    public GameObject youWin;
    public Text cogText;
    public Text dashText;
    public int maxRobots = 10;
    int RobotFixed;
    public int Dashes = 3;
    public int maxDashes = 3;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip talkSound;
    public AudioClip dashSound;
    public bool gameOver = false;
    public bool gameWin = false;

    public int Cogs;
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    public float timeDashing = 0.5f;
    bool isDashing;
    float dashTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        RobotFixed = 0;
        youLose.SetActive(false);
        youWin.SetActive(false);
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        cogText.text = "Cogs: " + Cogs.ToString();
        audioSource = GetComponent<AudioSource>();
        dashText.text = "Dashes: " + Dashes.ToString();
    }
 
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        if (isDashing)
        {   
            
            dashTimer -= Time.deltaTime;
            speed = 9.0f;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            if (dashTimer < 0)
            {
                isDashing = false;
                speed = 3.0f;
            }
                
        }
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(Cogs >= 1)
            {    
            Launch();
            }


        }
        if (Input.GetKeyDown("space"))
            {
                if (Dashes >= 1)
                {
                    if (isDashing)
                        return;
            
                    isDashing = true;
                    dashTimer = timeDashing;
                    Instantiate(SpeedEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                    
                    PlaySound(dashSound);
                    Dashes = Dashes - 1;
                    dashText.text = "Dashes: " + Dashes.ToString();

                }
            }
        if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
   
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    PlaySound(talkSound);
                    if (gameWin == true)
                        {
            
                            SceneManager.LoadScene("Other");

                         }
                }
            }
        }
//work        
        if (currentHealth == 0)
        {
            gameOver = true;
            youLose.SetActive(true);
        }
        if (Input.GetKey(KeyCode.R))

        {
            if (gameWin == true)
            {
                Application.LoadLevel(Application.loadedLevel);

            }
            if (gameOver == true)

            {
            
             Application.LoadLevel(Application.loadedLevel);

            }

        }    
//work end
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
 
        rigidbody2d.MovePosition(position);
    }
 //work test
    public void ChangeRobot(int Robots)
    {
        RobotFixed = Mathf.Clamp(RobotFixed + Robots, 0, maxRobots);
        Debug.Log(RobotFixed + "/" + maxRobots);

        if (RobotFixed == 5 )
        {
            gameWin = true;
            youWin.SetActive(true);
        }
    }
    public void MoreCogs(int amount)
    {
        Cogs = Mathf.Clamp(Cogs + amount, 0, 100);
        cogText.text = "Cogs: " + Cogs.ToString();
        Instantiate(fixedEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    }
    public void ChangeDashes(int amount)
    {
        Dashes = Mathf.Clamp(Dashes + amount, 0, maxDashes);
        dashText.text = "Dashes: " + Dashes.ToString();
        Instantiate(fixedEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
		animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            Instantiate(hurtEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            
            PlaySound(hitSound);
        }
        if (amount > 0)
        {
            Instantiate(fixedEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Cogs = Cogs-1;
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        cogText.text = "Cogs: " + Cogs.ToString();

        PlaySound(throwSound);
    } 
    public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Portal") 
        {
            Instantiate(PortalEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
 

