using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Gamemanager : MonoBehaviour
{
    private AudioSource Audio;
    public AudioSource Metal;
    public GameObject Player;
    Player PlayerS;
    public GameObject GameOverMenu, PauseMenu, enemy;
    public GameObject[] platform, Item, BackGroundImages, BGM;
    public Text ScoreText, SpeedText, HighScoreText;
    public Image[] ItemUI;
    string sText;
    int count = 0;
    float currentTimeSpeed;
    int platNum,platColor,itemNum = 0;
    bool isPurpleStop = false;
    public bool hasShield;
    public float[] platPoint;
    void Awake() {
        PlayerS = this.Player.GetComponent<Player>();
        Metal = Metal.GetComponent<AudioSource>();
        HighScoreText.text = "HighScore : " + PlayerPrefs.GetInt("HighScore").ToString();
        int ItemSold = PlayerPrefs.GetInt("Item");
        if(ItemSold%2 == 1) PlayerS.speed += 1;
        SpeedChange();
        
        if(ItemSold%4 >= 2) {
            hasShield = true;
            PlayerS.GetSheild(true);
        }
        else hasShield = false;
        Audio = BGM[PlayerPrefs.GetInt("BgmNum")].GetComponent<AudioSource>();
        Audio.Play();
        if(PlayerPrefs.GetInt("Item")%8 >= 4){
            currentTimeSpeed = 1.25f;
            BGMspeedAdd(0.25f);
            }
        else currentTimeSpeed = 1f;
        PlayerPrefs.SetInt("Item",0);
        PlayerPrefs.Save();
    }
    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
            if(currentTimeSpeed != 0)
                Pause();
        ObjectMaker(); 
        BackGroundMove();      
    }
    void ObjectMaker(){
        count++;
        if (count % 40 == 0){
            platNum += Random.Range(1,3);
            platColor = (count/40 + Random.Range(1, 120) < 150 || isPurpleStop) ? 0 : 1;
            Instantiate(platform[platColor], new Vector3(12,platPoint[platNum % 3],0),Quaternion.identity);
            if (platNum % 5 == 0){
                Instantiate(platform[platColor], new Vector3(12,platPoint[(platNum + 1) % 3],0),Quaternion.identity);
                Instantiate(Item[0], new Vector3(12,platPoint[(platNum + 1)%3]+1,0),Quaternion.identity);
                }

            if(count % 200 != 0) itemNum = 0;
            else if (hasShield){
                do
                {
                    itemNum = Random.Range(1,6);
                } while (itemNum == 3);
            }
            else itemNum = Random.Range(1,6);
            
            Instantiate(Item[itemNum], new Vector3(12,platPoint[platNum%3]+1,0),Quaternion.identity);
            }
            
        if (count % 70 == 0 && !isPurpleStop)
            Instantiate(enemy, new Vector3(Random.Range(-10 , 11), 8),Quaternion.identity);
        if (count % 3600 == 0)
            BGMspeedAdd(0.25f);
    }
    public void BGMspeedAdd(float add){
        Audio.pitch += add;
    }
    void BackGroundMove(){
        BackGroundImages[0].transform.Translate(-0.025f,0,0);
        BackGroundImages[1].transform.Translate(-0.025f,0,0);
        BackGroundImages[2].transform.Translate(-0.05f,0,0);
        BackGroundImages[3].transform.Translate(-0.05f,0,0);
        BackGroundImages[4].transform.Translate(-0.075f,0,0);
        BackGroundImages[5].transform.Translate(-0.075f,0,0);
    }
    public void ScoreChange(int score){
        ScoreText.text = score.ToString();
    }
    public void SpeedChange(){
        sText = "";
        for (int i = 0; i < PlayerS.speed; i++) sText += '▶';
        SpeedText.text = sText;
    }
    public void GameOver(){
        Metal.Play();
        GameOverMenu.gameObject.SetActive(true);
        Time.timeScale = 1f;
        Audio.mute = true;
        if (int.Parse(ScoreText.text) > PlayerPrefs.GetInt("HighScore"))
        {
            HighScoreText.text = "HighScore : " + ScoreText.text;
            PlayerPrefs.SetInt("HighScore",int.Parse(ScoreText.text));
        }
        int addCoin = int.Parse(ScoreText.text)/10;
        PlayerPrefs.SetInt("Coin",PlayerPrefs.GetInt("Coin")+addCoin);
        PlayerPrefs.Save();
    }
    public void NewGame(){
        SceneManager.LoadScene(1);
    }
    public void GoTitle(){
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void Slow(){
        ItemUI[0].gameObject.SetActive(true);
        Time.timeScale = currentTimeSpeed-0.25f;
        Audio.pitch -= 0.25f;
        Invoke("Fast",2.5f);
    }
    public void Fast(){
        ItemUI[0].gameObject.SetActive(false);
        Audio.pitch += 0.25f;
        Time.timeScale += 0.25f;
        currentTimeSpeed = Time.timeScale;
        }
    public void StopPurple(){
        ItemUI[1].gameObject.SetActive(true);
        isPurpleStop = true;
        Invoke("StartPurple",3f);
    }
    public void StartPurple(){
        ItemUI[1].gameObject.SetActive(false);
        isPurpleStop = false;
    }
    public void IsShield(bool Shield){
        hasShield = Shield;
    }
    public void Pause(){
        currentTimeSpeed = Time.timeScale;
        Time.timeScale = 0;
        Audio.Pause();
        PauseMenu.SetActive(true);
    }
    public void Resume(){
        PauseMenu.SetActive(false);
        Time.timeScale = currentTimeSpeed;
        Audio.Play();
    }
}
