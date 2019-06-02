using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighscoreTable : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;




    private void Awake(){

        //ResetScore();
        
        
    }


    private void ResetScore(){
        Highscores hg = new Highscores();
        hg.highscoreEntryList = new List<HighscoreEntry>();

        string json = JsonUtility.ToJson(hg);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }


    public void InitateScore(){
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");


        entryTemplate.gameObject.SetActive(false);


        //AddHighscoreEntry(10000);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscoreEntryList = highscores.highscoreEntryList;

        

        for(int i = 0; i < highscoreEntryList.Count; i++){
            for(int j = i + 1; j < highscoreEntryList.Count; j++){
                
                if(highscoreEntryList[j].score > highscoreEntryList[i].score){

                    HighscoreEntry temp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = temp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();

        int k = 0;
        foreach(HighscoreEntry entry in highscoreEntryList){
            CreateHighscoreEntryTransform(entry, entryContainer, highscoreEntryTransformList);
            if(k >= 7){
                break;
            }
            k++;
        }

        
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, 
                                                List<Transform> transformList){
        
        float templateHeight = 20f;

        Transform entryTransform = Instantiate( entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank){
            case 1:
                rankString = "1st";
                break;
            case 2:
                rankString = "2nd";
                break;
            case 3:
                rankString = "3rd";
                break;
            default:
                rankString = rank + "th";
                break;
        }

        int score = highscoreEntry.score;

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();


        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score){
        HighscoreEntry highscoreEntry = new HighscoreEntry{score = score};

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

    }

    [System.Serializable]
    private class Highscores{
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry{
         
        public int score;
    }

}
