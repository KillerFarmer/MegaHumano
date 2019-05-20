using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    public float speed = 250.0f;
    public float jumpForce = 12.0f;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxcollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
    }


    void Update(){

        float movX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 velocity = new Vector2(movX, rigidbody.velocity.y);
        rigidbody.velocity = velocity;


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
        }

    }

}
