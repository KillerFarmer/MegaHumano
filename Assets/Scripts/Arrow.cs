using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{   

    public float speed = 15;

    public float direction = 0;

    private Rigidbody2D RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        RigidBody.velocity = Vector2.right * speed * direction;

    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Wall"){
            Destroy(gameObject);
        } else if(col.gameObject.tag == "Flame"){
            Destroy(gameObject);
        }
    }


    void OnBecameInvisible(){

        Destroy(gameObject);
        
    }
}
