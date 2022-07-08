using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Ball : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Text scoreText;
    public Text HighScore;
    AudioSource goalSound;
    private int score = 0;
    private void Awake() {
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore",0);
        HighScore.text = "High Score : " + PlayerPrefs.GetInt("HighScore");
        goalSound = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
       switch(other.gameObject.name){
            case "Bar":
                rigid.velocity += Vector2.up*10 + Vector2.right*other.gameObject.GetComponent<Bar>().moveVec*50;
                break;
            case "Goal":
                scoreText.text = ""+ (++score);
                other.gameObject.transform.position = Vector3.right*Random.Range(-8, 9) + Vector3.up*Random.Range(-2, 4);
                rigid.velocity += Vector2.right*(0.5f+Random.Range(-1, 1));
                goalSound.Play();
                break;
            case "Dead":
                if (score > PlayerPrefs.GetInt("HighScore")){
                    PlayerPrefs.SetInt("HighScore",score);
                    PlayerPrefs.Save();
                }
                GetComponent<SpriteRenderer>().color = Color.black;
                Invoke("ReStart",2);
                break;
       }
   }
   void ReStart(){
       SceneManager.LoadScene(0);
   }
}
