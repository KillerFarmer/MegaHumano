using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    public float MaxHP = 90;
    private float HP;
    public Image healthBar;

    public float speed = 250.0f;
    public float jumpForce = 12.0f;
    public float damageForce = 10.0f;

    public GameObject flame;
    public GameObject poweUpFlame;

    public static int ShotFlames;

    public int maxFlames = 2;

    private float directionOfFlame = 1;

    private Rigidbody2D RigidBody;
    private BoxCollider2D boxcollider;

    private bool isPowerUpActive = false;
    private bool invensibilityByDamage = false;
    private bool facingRight = true;
    private bool underDamage = false;
    private bool dead = false;



    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        RigidBody = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();

        isPowerUpActive = false;
        ShotFlames = 0;
    }


    void FixedUpdate(){

        // Horizontal Movement
        float movX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 velocity = new Vector2(movX, RigidBody.velocity.y);

        if(!underDamage && !dead){    
            RigidBody.velocity = velocity;
            FlipSprite();
        }

        
        
        // Jump
        Vector3 max = boxcollider.bounds.max;
        Vector3 min = boxcollider.bounds.min;

        Vector2 RightCorner = new Vector2(max.x, min.y -0.1f);
        Vector2 LeftCorner = new Vector2(min.x, min.y - 0.2f);

        Collider2D hit = Physics2D.OverlapArea(RightCorner, LeftCorner);

        bool onGround = false;
        if(hit != null){
            onGround = true;
        }

        RigidBody.gravityScale = onGround && movX == 0 ? 0 : 1;
        if(onGround && Input.GetKeyDown(KeyCode.Space) && !dead){

            RigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.jump);
        }

    }


    void Update(){

        // Direction of Flames
        // if(Input.GetButtonDown("Horizontal")){

        //     float temp = directionOfFlame;
        //     directionOfFlame = Input.GetAxisRaw("Horizontal");

        //     if(directionOfFlame == 0){
        //         directionOfFlame = temp;
        //     }
        // }
        if(facingRight){
            directionOfFlame = 1;
        } else{
            directionOfFlame = -1;
        }
        

        // Shot a Flame without Power Up
        if(!isPowerUpActive){

            if(ShotFlames < maxFlames & Input.GetButtonDown("Fire1")){

                ShotFlames++;
    
                GameObject shoot = Instantiate(flame, transform.position, Quaternion.identity) as GameObject;
                shoot.GetComponent<Flame>().direction = directionOfFlame;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shoot);

            } 
        
        // Shot Flame with Power Up
        } else{

            if(ShotFlames < maxFlames & Input.GetButtonDown("Fire1")){

                ShotFlames++;
                ShotFlames++;

                StartCoroutine(DoubleShot());
            } 
        }
    
        
    }

    void OnCollisionEnter2D(Collision2D col){

        // Obtain Power Up
        if(col.gameObject.tag == "PowerUp"){
            isPowerUpActive = true;
            maxFlames = 4;
            StartCoroutine(PowerUpTimeUp());
            Destroy(col.gameObject);

        // Touch An enemy, recieve damage and impulse
        } else if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Arrow" || col.gameObject.tag == "Boss"
                  ||  col.gameObject.tag == "Axe"){

            if(!invensibilityByDamage){
                
                if(col.gameObject.tag == "Enemy"){
                    HP = HP - (30 / 3);
                } else if( col.gameObject.tag == "Arrow"){
                    HP = HP - (30 / 5);
                    Destroy(col.gameObject);
                } else if(col.gameObject.tag == "Boss"){
                    HP = HP - 30;
                } else if(col.gameObject.tag == "Axe"){
                    HP = HP - 30;
                    Destroy(col.gameObject);
                }

                healthBar.fillAmount = Mathf.Abs(HP / MaxHP);
                
                invensibilityByDamage = true;
                underDamage = true;

                if(HP > 0){
                    SoundManager.Instance.PlayOneShot(SoundManager.Instance.damage);
                }
                

                if(transform.position.x < col.transform.position.x){

                    StartCoroutine(Impulse(true));

                } else{

                    StartCoroutine(Impulse(false));
                }

                StartCoroutine(BecomeMortal());
            }
            
        }

        if((HP <= 0 || col.gameObject.tag == "Fall") && !dead){
            healthBar.fillAmount = 0;
            StartCoroutine(Die());
        }
    }


    IEnumerator DoubleShot(){
       
        GameObject shoot = Instantiate(poweUpFlame, transform.position, Quaternion.identity) as GameObject;
        shoot.GetComponent<Flame>().direction = directionOfFlame;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.shoot);

        yield return new WaitForSeconds(0.1f);        

        GameObject shoot2 = Instantiate(poweUpFlame, transform.position, Quaternion.identity) as GameObject;
        shoot2.GetComponent<Flame>().direction = directionOfFlame;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.shoot);
    }

    IEnumerator PowerUpTimeUp(){
        
        yield return new WaitForSeconds(5);

        maxFlames = 2;
        isPowerUpActive = false;

    }

    IEnumerator BecomeMortal(){

        yield return new WaitForSeconds(0.5f);
        invensibilityByDamage = false;
    }

    IEnumerator Impulse(bool direction){

        if(!direction){
            RigidBody.velocity = Vector2.right * damageForce;

        } else{
            RigidBody.velocity = Vector2.left * damageForce;
        }

        yield return new WaitForSeconds(0.3f);
        RigidBody.velocity = Vector2.zero;
        underDamage = false;

    }


    IEnumerator Die(){
        dead = true;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.death);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void FlipSprite(){

        if(facingRight && RigidBody.velocity.x < 0){
            
            Vector3 scale = transform.localScale;
		    scale.x *= -1;
		    transform.localScale = scale; 

            facingRight = false;

        } else if(!facingRight && RigidBody.velocity.x > 0){
            Vector3 scale = transform.localScale;
		    scale.x *= -1;
		    transform.localScale = scale; 

            facingRight = true;
        }
        

    }




}
