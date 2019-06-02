using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{

    public int seconds;

    public GameObject TimerTag;


    // Start is called before the first frame update
    void Start()
    {
        seconds = 0;
        StartCoroutine(AddSecond());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AddSecond(){

        while(true){
            yield return new WaitForSeconds(1);
            seconds++;
            UpdateTimer();
        }
    }


    void UpdateTimer(){


        int sec = seconds % 60;
        int min = seconds / 60;

        string time;

        if(min < 10){
            time = "0" + min;
        } else{
            time = min.ToString();
        }

        time += ":";

        if(sec < 10){
            time += "0" + sec;
        } else{
            time += sec.ToString();
        }
        TimerTag.GetComponent<Text>().text = time;
    

    }
}
