﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    public float speed = 250.0f;
    public float jumpForce = 12.0f;
    public float damageForce = 10.0f;

    public GameObject flame;

    public static int ShotFlames;

    public int maxFlames = 2;

    private float directionOfFlame = 1;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxcollider;

    private bool isPowerUpActive;

    private bool invensibilityByDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();

        isPowerUpActive = false;
        ShotFlames = 0;
    }


    void FixedUpdate(){

        float movX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 velocity = new Vector2(movX, rigidbody.velocity.y);
        rigidbody.velocity = velocity;

        if(Input.GetButtonDown("Horizontal")){

            float temp = directionOfFlame;
            directionOfFlame = Input.GetAxisRaw("Horizontal");

            if(directionOfFlame == 0){
                directionOfFlame = temp;
            }
        }
        

        Vector3 max = boxcollider.bounds.max;
        Vector3 min = boxcollider.bounds.min;

        Vector2 RightCorner = new Vector2(max.x, min.y -0.1f);
        Vector2 LeftCorner = new Vector2(min.x, min.y - 0.2f);

        Collider2D hit = Physics2D.OverlapArea(RightCorner, LeftCorner);

        bool onGround = false;
        if(hit != null){
            onGround = true;
        }

        rigidbody.gravityScale = onGround && movX == 0 ? 0 : 1;
        if(onGround && Input.GetKeyDown(KeyCode.Space)){
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.jump);
        }

    }


    void Update(){
        
        if(directionOfFlame == 0){
            return;
        }

        if(!isPowerUpActive){

            if(ShotFlames < maxFlames & Input.GetButtonDown("Fire1")){

                //float direction = Input.GetAxisRaw("Fire1");

                GameObject shoot = Instantiate(flame, transform.position, Quaternion.identity) as GameObject;
                //shoot.GetComponent<Flame>().direction = direction;
                shoot.GetComponent<Flame>().direction = directionOfFlame;
                ShotFlames++;
            } 
           

        } else{

            if(ShotFlames < maxFlames & Input.GetButtonDown("Fire1")){

                

                ShotFlames++;
                ShotFlames++;
                StartCoroutine(DoubleShot());


            } 
        }
    
        
    }

    void OnCollisionEnter2D(Collision2D col){

        if(col.gameObject.tag == "PowerUp"){
            isPowerUpActive = true;
            maxFlames = 4;
            StartCoroutine(PowerUpTimeUp());
            Destroy(col.gameObject);

        } else if(col.gameObject.tag == "Enemy"){

            if(!invensibilityByDamage){
                invensibilityByDamage = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.damage);

                

                if(transform.position.x < col.transform.position.x){

                    Vector2 verticalImpulse = Vector2.up * damageForce;
                    rigidbody.AddForce(verticalImpulse, ForceMode2D.Impulse);

                    Vector2 horizontalImpulse = Vector2.right * damageForce; 
                    rigidbody.AddForce( horizontalImpulse, ForceMode2D.Impulse);

                } else{
                    Vector2 verticalImpulse = Vector2.up * damageForce;
                    rigidbody.AddForce(verticalImpulse, ForceMode2D.Impulse);

                    Vector2 horizontalImpulse = Vector2.left * damageForce; 
                    rigidbody.AddForce( horizontalImpulse, ForceMode2D.Impulse);
                }
                StartCoroutine(BecomeMortal());
            }
            
        }
    }


    IEnumerator DoubleShot(){
       
        GameObject shoot = Instantiate(flame, transform.position, Quaternion.identity) as GameObject;
        shoot.GetComponent<Flame>().direction = directionOfFlame;

        yield return new WaitForSeconds(0.1f);        

        GameObject shoot2 = Instantiate(flame, transform.position, Quaternion.identity) as GameObject;
        shoot2.GetComponent<Flame>().direction = directionOfFlame;
    }

    IEnumerator PowerUpTimeUp(){
        
        yield return new WaitForSeconds(5);

        maxFlames = 2;
        isPowerUpActive = false;

    }

    IEnumerator BecomeMortal(){
        yield return new WaitForSeconds(2);

        invensibilityByDamage = false;
    }

}
