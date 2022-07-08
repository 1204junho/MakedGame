using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Titlemanager : MonoBehaviour
{
    public Text HighScore;
    private void Awake() {
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore",0);
        HighScore.text = string.Format("High Score : {0:n0}",PlayerPrefs.GetInt("HighScore"));
    }
    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    public void OpenBlog(){
        Application.OpenURL("https://blog.naver.com/1204junho");
        }
    public void Quit(){
        Application.Quit();
    }
}
