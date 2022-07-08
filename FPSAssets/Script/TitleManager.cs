using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    int index = 0;
    public GameObject mouseImage;
    private void Awake() {
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore",0);
        GameObject.Find("HighScore").GetComponent<Text>().text = string.Format("High Score\n| {0} |",PlayerPrefs.GetInt("HighScore"));
    }
    private void Update() {
        if (Input.anyKeyDown){
            if(Input.GetAxisRaw("Vertical") > 0 && index > 0)
                index--;
            else if(Input.GetAxisRaw("Vertical") < 0 && index < 3)
                index++;
            if(index == 0) mouseImage.SetActive(true);
            else mouseImage.SetActive(false);
            GameObject.Find(">").transform.localPosition = Vector3.left*100 + (index*40+75)*Vector3.down;
            if (Input.GetAxisRaw("Submit") == 1)
                Select();
        }
    }
    void Select()
    {
        switch (index)
        {
            case 0:
                SceneManager.LoadScene(1);
                break;
            case 1:
                Application.OpenURL("https://blog.naver.com/1204junho");
                break;
            case 2:
                ScoreSet();
                break;
            case 3:
                Application.Quit();
                break;
            default:
                return;
        }
    }
    void ScoreSet(){
        PlayerPrefs.SetInt("HighScore",0);
        PlayerPrefs.Save();
        GameObject.Find("HighScore").GetComponent<Text>().text = "High Score\n| 0 |";
    }
}