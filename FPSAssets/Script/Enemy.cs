using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    int maxHealth;
    public int score;
    public GameObject healthSlider;
    Slider health;
    GameManager gamemanager;
    Objectmanager objectmanager;
    AudioSource hitSound;
    GameObject player;
    private void Awake() {
        gamemanager = GameObject.Find("Manager").GetComponent<GameManager>();
        objectmanager = GameObject.Find("Manager").GetComponent<Objectmanager>();
        hitSound = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        health = healthSlider.GetComponent<Slider>();
        maxHealth = (int)health.value;
    }
    private void OnEnable() {
        health.value = maxHealth;
        transform.localScale = new Vector3(2,2,2);
    }
    private void FixedUpdate() {
        transform.position *= 0.985f;
        ViewPlayer();
    }
    void ViewPlayer(){
        Vector3 toPlayer;
        toPlayer = player.transform.position - transform.position;
        toPlayer.y = 0;
        transform.LookAt(toPlayer);
    }
    string SelectItem(){
        switch (Random.Range(0, 10))
        {
            case 0:
                return "BulletPlus";
            case 1:
                return "BulletSpeed";
            case 2:
                return "Speed";
            case 3:
                return "TowerHeart";
            default:
                return "BulletLoad";
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Bullet"){
            health.value--;
            if (health.value <= 0){
                gamemanager.ScoreChange(score);
                if (transform.position.y > 1)
                    transform.position += (transform.position.y - 1)*Vector3.down;
                objectmanager.Spawn(SelectItem(),transform.position);
                this.gameObject.SetActive(false);
                return;
            }
            hitSound.Play();
            transform.localScale -= new Vector3(0.5f,0.5f,0.5f);
            transform.position -= new Vector3(0,0.25f,0);
            }
        if(other.gameObject.tag == "Respawn"){
            this.gameObject.SetActive(false);
            gamemanager.StartCoroutine("Damaged");
            }
    }
}
