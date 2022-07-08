using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public string enemyType, dropItem;
    public int health, score, dropRate;
    int maxhealth, moveCount, difficult;
    bool isBeatable = true;
    public AudioSource damageSound;
    Vector2 toPlayer;
    GameObject Player;
    Gamemanager gamemanager;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    private void Awake() {
        moveCount = 0; maxhealth = health; difficult = 0;
        Player = GameObject.Find("Player");
        gamemanager = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        damageSound = GameObject.Find(enemyType == "FlyingCube" ? "CubeSound" : "SlimeSound").GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable() {
        difficult = gamemanager.difficult;
        Setting();
        StartCoroutine("ChangeDir");
        health = maxhealth;
        moveCount = 0;
        transform.position = Player.transform.position + new Vector3(Random.Range(-5, 6),Random.Range(-5, 6),0).normalized*20;
    }
    IEnumerator ChangeDir(){
        float cycle = 0.5f; moveCount++;
        toPlayer = (Player.transform.position - transform.position).normalized;
        if(enemyType == "Slime")
            rigid.velocity = rigid.velocity*0.5f + toPlayer*(Random.Range(3,6) + difficult);
        if(enemyType == "FlyingCube"){
            if(moveCount%2 == 0){
                rigid.velocity = new Vector2(0,0);
                cycle = 1.5f; }
            else rigid.velocity = toPlayer*(10 + 2*difficult);
        }
        if(enemyType == "SlimeCube"){
            if(moveCount% 5 != 0)
                rigid.velocity = rigid.velocity*0.25f + toPlayer*(Random.Range(4,7) + difficult);
            else{
                gamemanager.SpawnEnemy();
                rigid.velocity = toPlayer*(8+difficult);
                cycle = 1; }
            }
        yield return new WaitForSeconds(cycle);
        StartCoroutine("ChangeDir");
    }
    IEnumerator Beatable(){
        isBeatable = false;
        sprite.color = sprite.color - new Color(0,0,0,0.4f);
        yield return new WaitForSeconds(0.25f);
        isBeatable = true;
        sprite.color = sprite.color + new Color(0,0,0,0.4f);
    }
    private void Setting(){
        sprite.color = new Color(1,1-(difficult*0.1f),1,1);
        if(enemyType == "Slime") maxhealth = 3 + difficult;
        if(enemyType == "FlyingCube") maxhealth = 15 + 3*difficult;
        if(enemyType == "SlimeCube"){
            maxhealth = 15*(difficult+1);
            transform.localScale = new Vector3((4+difficult)/2,(4+difficult)/2,0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Weapon" && isBeatable){
            damageSound.Play();
            if (other.gameObject.name == "WoodSword")
                health--;
            else if (other.gameObject.name == "Sword")
                health-=2;
            else if (other.gameObject.name == "SlimeCubeSword")
                health-=3;
            gamemanager.EnemyInfo(enemyType,health,maxhealth);            
            if (health <= 0){
                gamemanager.ScoreChange(score);
                this.gameObject.SetActive(false);
                if(Random.Range(0, 10) < dropRate){
                    gamemanager.ItemDrop(dropItem,transform.position);
                    if(enemyType == "SlimeCube"){
                        gamemanager.respawn.SetActive(true);
                        gamemanager.healZone.SetActive(true);
                        gamemanager.healZone.transform.position = transform.position;
                        }
                    }
                }
            else{
                StartCoroutine("Beatable");
                if(enemyType == "Slime"){ //Only slime has knockBack
                    Vector2 knockBack = new Vector2(transform.position.x - other.transform.position.x,
                        transform.position.y - other.transform.position.y).normalized;
                    rigid.velocity = knockBack*15;
                }
            }
            gamemanager.ItemDrop("coin",transform.position);
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Respawn" && isBeatable){
        Vector2 knockBack = new Vector2(transform.position.x - other.transform.position.x,transform.position.y - other.transform.position.y);
        rigid.velocity = knockBack*5;
        }
    }
}
