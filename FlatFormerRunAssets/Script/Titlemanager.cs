using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Titlemanager : MonoBehaviour
{
    public GameObject[] Howtoplay;
    public Text HighScore;
    public Text BgmNum;
    private void Awake() {
        if (!PlayerPrefs.HasKey("HighScore")){
            PlayerPrefs.SetInt("HighScore",0);
            PlayerPrefs.Save();
            }
        if (!PlayerPrefs.HasKey("Coin")){
            PlayerPrefs.SetInt("Coin",0);
            PlayerPrefs.Save();
            }
        if (!PlayerPrefs.HasKey("Item")){
            PlayerPrefs.SetInt("Item",0);
            PlayerPrefs.Save();
            }
        if (!PlayerPrefs.HasKey("BgmNum")){
            PlayerPrefs.SetInt("BgmNum",0);
            PlayerPrefs.Save();
            }
        HighScore.text = "HighScore : " + PlayerPrefs.GetInt("HighScore").ToString();
        BgmShow();
    }
    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    public void GameReset(){
        PlayerPrefs.SetInt("HighScore",0);
        PlayerPrefs.Save();
        HighScore.text = "HighScore : 0";
    }
    public void BgmChange(){
        PlayerPrefs.SetInt("BgmNum",1 - PlayerPrefs.GetInt("BgmNum"));
        PlayerPrefs.Save();
        BgmShow();
    }
    public void OpenBlog(){
        Application.OpenURL("https://blog.naver.com/1204junho");
    }
    public void BgmShow(){
        switch (PlayerPrefs.GetInt("BgmNum"))
        {
            case 0:
                BgmNum.text = "BGM : A";
                break;
            case 1:
                BgmNum.text = "BGM : B";
                break;
            default:
                break;
        }
    }
    public void Quit(){
        Application.Quit();
    }
}