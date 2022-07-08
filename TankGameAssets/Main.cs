using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Text Score;
    private void Awake() {
       if(!PlayerPrefs.HasKey("HighScore")){
           PlayerPrefs.SetInt("HighScore",0);
           PlayerPrefs.Save();
       }
       Score.text = "HighScore : " + PlayerPrefs.GetInt("HighScore",0);
    }
    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    public void Quit(){
        Application.Quit();
    }
    public void OpenBlog(){
        Application.OpenURL("https://blog.naver.com/1204junho");
    }
}
