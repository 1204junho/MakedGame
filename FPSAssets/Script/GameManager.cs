using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] facePlayerObj;
    public Objectmanager objectmanager;
    public AudioSource BGM;
    public Light towerLight;
    public Text scoreText;
    public Slider towerHealth;
    public int score;
    bool isPause = false;
    void Start()
    {
        score = 0;
        Cursor.visible = false;
        InvokeRepeating("SpawnEnemy",1,3);
        facePlayerObj = GameObject.FindGameObjectsWithTag("Item");
        foreach(GameObject obj in facePlayerObj)
            obj.SetActive(false);
    }
    private void Update() {
        if(Input.GetButtonDown("Cancel"))
            Pause();
        if(isPause && Input.GetKeyDown("q"))
            ReStart(0);
            transform.LookAt(player.transform.position);
        foreach(GameObject obj in facePlayerObj )
            obj.transform.LookAt(player.transform.position);
    }
    void SpawnEnemy(){
        Vector3 enemypos = new Vector3(Random.Range(2, 6),0,(Random.Range(-5, 6))).normalized*60;
        if (score >= 20 && score % 3 == 0)
            objectmanager.Spawn("flyenemy",enemypos+Vector3.up*Random.Range(30, 61));
        else
            objectmanager.Spawn("enemy",enemypos); 
    }
    public void ScoreChange(int add){
        score += add;
        scoreText.text = "| " + score +" |";

        if (score % 10 == 0 && score <= 100)
        {
            CancelInvoke();
            InvokeRepeating("SpawnEnemy",1,3-(float)score/40);
        }
    }
    IEnumerator Damaged(){
        towerHealth.value--;
        Debug.Log(towerHealth.value);
        if(towerHealth.value <= 0){
            GameObject.Find("PlayerUI").transform.GetChild(3).gameObject.SetActive(true);
            GameObject.Find("PlayerUI").transform.GetChild(4).gameObject.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0f;
            if (score > PlayerPrefs.GetInt("HighScore")){
                PlayerPrefs.SetInt("HighScore",score);
                PlayerPrefs.Save();
                }
            }
        towerLight.range = towerHealth.value*10;
        towerLight.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        towerLight.color = Color.cyan;
    }
    public void ReStart(int scene){
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    public void Pause(){
        Time.timeScale = isPause ? 1f : 0f;
        player.GetComponent<AudioListener>().enabled = isPause;
        GameObject.Find("PlayerUI").transform.GetChild(5).gameObject.SetActive(!isPause);
        isPause = !isPause;
    }
}