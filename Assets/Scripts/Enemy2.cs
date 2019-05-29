using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;

    public GameObject arrow;
    public static int ShotArrow;
    private int maxArrow = 200;

    private float directionOfArrow, i, myWidth;
    private bool facingRight = true;
    public bool lookleft;
    Vector2 shootDir;

    void Start()
    {
        i = Random.Range(5, 50);
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        myWidth = mySprite.bounds.extents.x;
        ShotArrow = 0;

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

        if (ShotArrow < maxArrow)
        {
            i++;
            if (i == 60)
            {
                ShotArrow++;
                GameObject shoot = Instantiate(arrow, shootDir, Quaternion.identity) as GameObject;
                shoot.GetComponent<Arrow>().direction = directionOfArrow;
                i = 0;
            }
        }
    }
}