using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int HP = 40;

    public LayerMask enemyMask;
    public float speed = 1;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;

    public GameObject arrow, human;
    public static int ShotArrow;
    private int maxArrow = 200;

    private float directionOfArrow, i;
    private bool facingRight = false;
    Vector2 shootDir;


    public GameObject score;
    public GameObject dragon;
    public GameObject ui;



    void Start()
    {
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = human.GetComponent<SpriteRenderer>();
        myWidth = mySprite.bounds.extents.x;
        myHeight = mySprite.bounds.extents.y;
        i = 0;
        ShotArrow = 0;
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + Vector2.up * myHeight;
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f);
        

        if (!isGrounded || isBlocked)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
            facingRight = !facingRight;

        }

        Vector2 myVel = myBody.velocity;
        myVel.x = -myTrans.right.x * speed;
        myBody.velocity = myVel;

        if (!facingRight)
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


     void OnCollisionEnter2D(Collision2D col){

        if(col.gameObject.tag == "Flame"){
            Destroy(col.gameObject);
            HP -= 1;
        }

        if(HP <= 0){

            int seconds = ui.GetComponent<UserInterface>().seconds;
            int points = 1000 - seconds * 2;

            if(points < 0){
                points = 0;
            }

            // Comment to reset High Score
            score.GetComponent<HighscoreTable>().AddHighscoreEntry(points);
            score.GetComponent<HighscoreTable>().InitateScore();

            //
            score.SetActive(true);

            dragon.GetComponent<Dragon>().invensibilityByDamage = true;
            dragon.GetComponent<Dragon>().underDamage = true;

            Destroy(gameObject);
        }
    }
}
