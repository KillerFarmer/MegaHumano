using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool moveright;
    public float speed = 250.0f;
    public float jumpForce = 12.0f;

    public GameObject flame;

    public static int ShotFlames;

    public int maxFlames = 1;

    private float directionOfFlame = 1;
    private float move = 1;
    public float HP;

    public float movimiento;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxcollider;

    Transform myTrans;
    float myWidth;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = this.transform;
        myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        rigidbody = this.GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();

        ShotFlames = 0;
    }


    void FixedUpdate(){

        float movX = move * speed * Time.deltaTime;
        Vector2 velocity = new Vector2(movX, rigidbody.velocity.y);
        rigidbody.velocity = velocity;


        if (moveright)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movX, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movX, GetComponent<Rigidbody2D>().velocity.y);
        }


        Vector3 max = boxcollider.bounds.max;
        Vector3 min = boxcollider.bounds.min;

        Vector2 RightCorner = new Vector2(max.x, min.y - 0.1f);
        Vector2 LeftCorner = new Vector2(min.x, min.y - 0.2f);


        Vector2 lineCastPos = myTrans.position + myTrans.right * myWidth;
        isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down);

        Collider2D hit = Physics2D.OverlapArea(RightCorner, LeftCorner);

        bool onGround = false;
        if (hit != null){
            onGround = true;
        }

        rigidbody.gravityScale = onGround && movX == 0 ? 0 : 1;
        if(onGround && Input.GetKeyDown(KeyCode.Space)){
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


    }


    void Update(){


        movimiento = rigidbody.velocity.magnitude;

        if (!isGrounded || movimiento < 0.5)
        {
            moveright = !moveright;
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "PowerUp"){
            Debug.Log("Here");
            Destroy(col.gameObject);
        }
    }

}
