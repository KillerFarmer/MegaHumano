using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
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


    void OnBecameInvisible(){

        Dragon.ShotFlames--;
        Destroy(gameObject);
        
    }
}
