using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public int HP = 3;

    //public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;

    public GameObject arrow;
    //public static int ShotArrow;
    // private int maxArrow = 200;

    private float directionOfArrow, i, myWidth;
    public bool lookleft;
    Vector2 shootDir;

    void Start()
    {
        i = Random.Range(5, 50);
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        myWidth = mySprite.bounds.extents.x;
    
        StartCoroutine(ShootOnePerSecond());

    }
    
    void FixedUpdate()
    {
        if (lookleft)
        {
            shootDir = myTrans.position - myTrans.right * myWidth;
            directionOfArrow = -1;
        }
        else
        {
            shootDir = myTrans.position;
            directionOfArrow = 1;
        }

        // if (ShotArrow < maxArrow)
        // {
        //     i++;
        //     if (i == 60)
        //     {
        //         ShotArrow++;
        //         GameObject shoot = Instantiate(arrow, shootDir, Quaternion.identity) as GameObject;
        //         shoot.GetComponent<Arrow>().direction = directionOfArrow;
        //         i = 0;
        //     }
        // }
    }


    IEnumerator ShootOnePerSecond(){

        while(true){
            Shoot();
            yield return new WaitForSeconds(1);
        }
        
    }

    void Shoot(){

        GameObject shoot = Instantiate(arrow, shootDir, Quaternion.identity) as GameObject;
        shoot.GetComponent<Arrow>().direction = directionOfArrow;
        
    }


     void OnCollisionEnter2D(Collision2D col){

        if(col.gameObject.tag == "Flame"){
            Destroy(col.gameObject);
            HP -= 1;

        }


        if(HP <= 0){
            Destroy(gameObject);
        }
    }
}