using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public float h,v;
    bool isAttackNow = false, untouchable = false;
    public int speed, jumpPower,health;
    int attackAngle;
    Vector3 attackVec;
    public Transform cam;
    public GameObject[] shotgun;
    public GameObject shield;
    public Rigidbody2D rigid;
    public Sprite[] playerHead;
    public SpriteRenderer[] playerSprites;
    public GameManager ui;
    public AudioSource[] sounds;
    WaitForSeconds delayTime;
    private void Start() {
        delayTime = new WaitForSeconds(.375f);
    }
    private void OnEnable() {
        health = 3;
        ui.HealthChange(health);
    }
    public void ButtonDown(int n){
        switch (n)
        {
            case 0:
            case 2:
                h = n-1;
                break;
            case 1:
                v = 1;
                break;
        }
    }
    public void ButtonUp(int n){
        switch (n)
        {
            case 0:
            case 2:
                h = 0;
                break;
            case 1:
                v = 0;
                break;
        }
    } 
    public void Shot(){
        if (!isAttackNow && gameObject.activeSelf)
            StartCoroutine("Attack");
    }
    IEnumerator Attack(){
        isAttackNow = true;
        shotgun[1].SetActive(true);
        sounds[0].Play();
        yield return delayTime;
        shotgun[1].SetActive(false);
        isAttackNow = false;
    }
    public void Jump(){
        rigid.velocity += Vector2.up*jumpPower;
        sounds[2].Play();
    } 
    void Update()
    {
        cam.position = Vector3.Lerp(cam.position, Vector3.up*transform.position.y+Vector3.back*10,Time.deltaTime*3);
        if( v != 0 ) {
            //player
            playerSprites[0].sprite = (v < 0) ? playerHead[0] : playerHead[1];
            //shotgun
            if (!isAttackNow){
                shotgun[0].transform.localRotation = Quaternion.Euler(0, 0, (v < 0) ? 270 : 90);
                shotgun[0].transform.localPosition = Vector2.up *((v < 0) ? -1.25f : 1.25f);
                playerSprites[3].sortingOrder = (v < 0) ? 3 : 0;
            }
        }
        else if(h != 0){
            rigid.velocity = Vector2.right*h*speed + Vector2.up*rigid.velocity.y;//move
            playerSprites[3].sortingOrder = 3;
            //shotgun
            if (!isAttackNow){
                shotgun[0].transform.localRotation = Quaternion.Euler(0, 0, (h < 0) ? 180 : 0);
                shotgun[0].transform.localPosition = Vector2.right * ((h < 0) ? -1.25f : 1.25f);
                playerSprites[3].flipY = (h < 0);
            }
            //player
            playerSprites[0].sprite = playerHead[2];
            playerSprites[0].flipX = (h < 0);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(!untouchable && other.gameObject.tag =="EnemyAttack"){
            ui.HealthChange(--health);
            if(health <= 0){
                ui.Gameover();
                shotgun[1].SetActive(false);
                isAttackNow = false;
                gameObject.SetActive(false);
                }
            else{
                    if (other.gameObject.name == "Frame")
                    {
                        transform.position *= 0.5f;
                    }
                    else rigid.velocity = (transform.position - other.transform.position).normalized*20;
                    StartCoroutine("Hit");
                }
        }
        if(other.gameObject.tag =="Heart"){
            other.gameObject.SetActive(false);
            sounds[1].Play();
            rigid.velocity += Vector2.up*5;
            if (health < 5)
                ui.HealthChange(++health);
            else ui.ScoreChange(1000);
        }
    }
    IEnumerator Hit(){
        untouchable = true;
        shield.SetActive(true);
        yield return delayTime;
        untouchable = false;
        shield.SetActive(false);
    }
}
