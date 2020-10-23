using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text lives;
    private int livesValue = 3;
    public Text gameEndText;
    public Transform level2Start;

    public AudioClip overworldMusic;
    public AudioClip victoryMusic;
    public AudioSource musicSource;

    Animator playerAnimation;
    private bool facingRight = true;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        playerAnimation = GetComponent<Animator>();

        score.text = "Score: " + scoreValue.ToString();
        gameEndText.text = " ";
        lives.text = "Lives: " + livesValue.ToString();
        

        musicSource.clip = overworldMusic;
        musicSource.Play();


    }

    // Update is called once per frame
    void Update()
    {
        float vertMovement = Input.GetAxis("Vertical");

        if (Input.GetKey("escape"))

        {
          Application.Quit();
        }

        if (vertMovement > 0 || vertMovement < 0)
        {
            playerAnimation.SetInteger("Jumping", 1);
            isJumping = true;
        }
        else if (vertMovement == 0 && isJumping == true)
        {
            playerAnimation.SetInteger("Jumping", 0);
            isJumping = false;
        }
        else if ((Input.GetKey("d") || Input.GetKey("a")) && isJumping == false)
        {
            playerAnimation.SetInteger("Moving", 1);
        }

        else if ((Input.GetKey("d") == false && Input.GetKey("a") == false) && isJumping == false)
        {
            playerAnimation.SetInteger("Moving", 0);
        }
    }
    
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag ==  "Coin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();

            if(scoreValue == 4)
            {
                this.transform.position = level2Start.transform.position;
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }

            if(scoreValue >= 8)
            {
                gameEndText.text = "You win! Game created by R Mike Livingston.";
                musicSource.clip = victoryMusic;
                musicSource.Play();
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();

            if(livesValue <= 0)
            {
                gameEndText.text = "You lose! GAME OVER";
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
