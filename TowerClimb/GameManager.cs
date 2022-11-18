using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public RectTransform healthImage;
    public TMP_Text scoreText, timerText,highScoreText;
    public int score, timeSec;
    public GameObject gameOverScreen,player;
    WaitForSeconds oneSec;
    ObjectManager obj;
    private void Awake() {
        if(!PlayerPrefs.HasKey("HighScore")){
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
        }
        else
            highScoreText.text = "high-score : "+PlayerPrefs.GetInt("HighScore");
        oneSec = new WaitForSeconds(1);
        obj = gameObject.GetComponent<ObjectManager>();
        Time.timeScale = 0;
    }
    // Update is called once per frame
    public void ScoreChange(int score){
        this.score += score;
        scoreText.text = "Score : "+ this.score;
    }
    public void GameStart(){
        GetComponent<AudioSource>().Play();
        scoreText.color = Color.white;
        Time.timeScale = 1;
        obj.Init();
        score = 0;
        timeSec = 0;
        ScoreChange(0);
        player.transform.position = Vector3.zero;
        player.SetActive(true);
        StartCoroutine("Timer");
    }
    public void Gameover(){
        gameOverScreen.SetActive(true);
        GetComponent<AudioSource>().Stop();
        StopCoroutine("Timer");
        obj.StopCoroutine("MapFall");
        Time.timeScale = 0;
        scoreText.text = "Final Score\n" + score + " + " + timeSec + "(sec) x 100\n= " + (score+timeSec*100);
        scoreText.color = Color.yellow;
        if(PlayerPrefs.GetInt("HighScore") < score+timeSec*100){
            PlayerPrefs.SetInt("HighScore", score+timeSec*100);
            highScoreText.text = "high-score : "+PlayerPrefs.GetInt("HighScore");
            PlayerPrefs.Save();
        }
    }
    public void Exit(){
        Application.Quit();
    }
    IEnumerator Timer(){
        while (true)
        {
            timerText.text =  (timeSec/60 >= 10 ? "" : "0") + timeSec/60 + " : "+ (timeSec%60 >= 10 ? "" : "0") + timeSec%60;
            yield return oneSec;
            timeSec++;
            switch (timeSec)
            {
                case 30:
                    obj.downSpeed = 0.10f;
                    break;
                case 60:
                    obj.BlockSetTime = new WaitForSeconds(2);
                    break;
                case 120:
                    obj.BlockSetTime = new WaitForSeconds(1.75f);
                    obj.downSpeed = 0.11f;
                    break;
                case 150:
                    obj.BlockSetTime = new WaitForSeconds(1.5f);
                    obj.downSpeed = 0.12f;
                    break;
                case 180:
                    obj.Promote(0);
                    obj.downSpeed = 0.13f;
                    break;
                case 210:
                    obj.BlockSetTime = new WaitForSeconds(1.25f);
                    break;
                 case 240:
                    obj.downSpeed = 0.15f;
                    break;
                case 270:
                    obj.Promote(1);
                    break;
                case 300:
                    obj.downSpeed = 0.16f;
                    break;
                case 330:
                    obj.downSpeed = 0.17f;
                    break;
                case 360:
                    obj.downSpeed = 0.18f;
                    break;
                default:
                    break;
            }
        }
    }
    public void HealthChange(int health){
        healthImage.sizeDelta = 200*(health*Vector2.right+Vector2.up);
    }
    public void Pause(bool doPause){
        Time.timeScale = doPause ? 0 : 1;
    }
}
