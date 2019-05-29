using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{   

    public float speed = 15;

    public float direction = 0;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.right * speed * direction;

    }


    void OnBecameInvisible(){

        Enemy2.ShotArrow--;
        Destroy(gameObject);
        
    }
}
