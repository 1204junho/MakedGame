using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TMPro.TMP_Text scoreText;
    void Start()
    {
        ScoreUpdate();
    }
    void ScoreUpdate()
    {
        if (!PlayerPrefs.HasKey("Rank1")) { PlayerPrefs.SetInt("Rank1", 0); PlayerPrefs.Save(); }
        if (!PlayerPrefs.HasKey("Rank2")) { PlayerPrefs.SetInt("Rank2", 0); PlayerPrefs.Save(); }
        if (!PlayerPrefs.HasKey("Rank3")) { PlayerPrefs.SetInt("Rank3", 0); PlayerPrefs.Save(); }

        int score = PlayerPrefs.GetInt("Score");
        if (score > PlayerPrefs.GetInt("Rank1"))
        {
            PlayerPrefs.SetInt("Rank3", PlayerPrefs.GetInt("Rank2"));
            PlayerPrefs.SetInt("Rank2", PlayerPrefs.GetInt("Rank1"));
            PlayerPrefs.SetInt("Rank1", score);
            PlayerPrefs.Save();
        }
        else if (score > PlayerPrefs.GetInt("Rank2"))
        {
            PlayerPrefs.SetInt("Rank3", PlayerPrefs.GetInt("Rank2"));
            PlayerPrefs.SetInt("Rank2", score);
            PlayerPrefs.Save();
        }
        else if (score > PlayerPrefs.GetInt("Rank3"))
        {
            PlayerPrefs.SetInt("Rank3", score);
            PlayerPrefs.Save();
        }
        scoreText.text = "1. " + PlayerPrefs.GetInt("Rank1") +
            "\n2. " + PlayerPrefs.GetInt("Rank2") +
            "\n3. " + PlayerPrefs.GetInt("Rank3") + "\nYour Score : " + (score>0 ? score : "Too Late");
    }
    public void ResetScore()
    {
        PlayerPrefs.SetInt("Rank1", 0);
        PlayerPrefs.SetInt("Rank2", 0);
        PlayerPrefs.SetInt("Rank3", 0);
        PlayerPrefs.Save();
        scoreText.text = "1. 0\n2. 0\n3. 0";
    }
}
