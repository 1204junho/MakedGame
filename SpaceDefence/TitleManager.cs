using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public UnityEngine.UI.Image backGround;
    private void Start()
    {
        backGround = GameObject.Find("Back").GetComponent<UnityEngine.UI.Image>();
    }
    private void Update()
    {
        backGround.color = Color.white * (Mathf.Sin(Time.time*0.5f)*0.25f+0.25f);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SetDifficult(int num)
    {
        PlayerPrefs.SetInt("Difficult", num);
        PlayerPrefs.Save();
        MoveScene(1);
    }
    public void MoveScene(int num)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
    }
}
