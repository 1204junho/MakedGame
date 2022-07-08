using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    int health;
    public int hasGold, speed, goldRate, durability, damaged;
    bool isBeatable, isSword;
    public bool isInSpawn, isCanMove, isCanDash;
    public GameObject sword;
    public Gamemanager gamemanager;
    public Sprite[] playerImages;
    public AudioSource walkSound,attackSound;
    Vector2 mouseVec;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    
    
    private void Start()
    {
        StartCoroutine("Stun",1);//Show left Life page one sec.
        isBeatable = true;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gamemanager.HealthChange(health);
        InvokeRepeating("GoldChange",3,5);
    }
    private void FixedUpdate(){
        Move();
        Attack();
    }
    private void Move(){
        if (!isCanMove) return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h == 0 && v == 0) walkSound.mute = true;//if you don't move, walkSound will be mute
        else{
            transform.position += new Vector3(h,v,0).normalized*speed*0.01f;
            walkSound.mute = false;
            }
        if (Input.GetMouseButton(1) && isCanDash)//it can use when you buy dash
            StartCoroutine("Dash",new Vector3(h,v,0).normalized*speed);
    }
    private void Attack(){  
        mouseVec = new Vector2(Input.mousePosition.x - Screen.width*0.5f, Input.mousePosition.y - Screen.height*0.5f).normalized;
        if (mouseVec.y <= -0.7){//when you see up, sword sprite is under player sprite.
            sprite.sprite = playerImages[0];
            sprite.sortingOrder = 1;}
        else if (mouseVec.x >= 0.7)
            sprite.sprite = playerImages[1];
        else if (mouseVec.y >= 0.7){
            sprite.sprite = playerImages[2];
            sprite.sortingOrder = 3;}
        else if (mouseVec.x <= -0.7)
            sprite.sprite = playerImages[3];
        if(sword != null){
            if (Input.GetMouseButton(0) && !isInSpawn){//when you in spawn, you can't attack
                sword.transform.position = transform.position + new Vector3(mouseVec.x*2, mouseVec.y*2, 0);
                sword.transform.rotation = Quaternion.Inverse(Quaternion.FromToRotation(mouseVec, transform.up));
                if (!isSword)
                {
                    attackSound.Play();
                    sword.SetActive(true);    
                    isSword = true;
                }
            }
            else{
                sword.SetActive(false);
                isSword = false;
                }
            }
        if (sword != null && durability <= 0){//sword is broken
            sword.SetActive(false);
            sword = null;
            }
    }
    IEnumerator Dash(Vector3 dashing){
        isCanDash = false;
        rigid.velocity = dashing;
        yield return new WaitForSeconds(1);
        isCanDash = true;
    }
    IEnumerator beatable(){
        isBeatable = false;
        sprite.color = new Color(1,1,1,0.6f);
        yield return new WaitForSeconds(0.5f);
        isBeatable = true;
        sprite.color = new Color(1,1,1,1);
    }
    IEnumerator Stun(float delay){
        isCanMove = false;        
        yield return new WaitForSeconds(delay);//you can't move when isCanMove = false
        isCanMove = true;
    }
    public void Heal(int heal){
        health = health + heal > gamemanager.maxhealth ? gamemanager.maxhealth : health + heal;
        gamemanager.HealthChange(health);
    }
    public void Heal(){
        if (health >= gamemanager.maxhealth) return;
        health++;
        gamemanager.HealthChange(health);
    }
    public void GoldChange(int gold)
    {
        hasGold += gold;
        gamemanager.gold.text = hasGold.ToString();
    }
    public void GoldChange()
    {
        hasGold++;
        gamemanager.gold.text = hasGold.ToString();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item"){
            if (other.gameObject.name == "Coin(Clone)")
                GoldChange(goldRate);
            else if (other.gameObject.name == "Potion(Clone)")
                gamemanager.PotionChange(20);//4 health.
            else if (other.gameObject.name == "JewelR(Clone)")
                gamemanager.PotionChange(5);//1 health.
            else if (other.gameObject.name == "JewelY(Clone)")
                goldRate++;//+100%
        other.gameObject.SetActive(false);
        }
        if (other.gameObject.name == "Shop")
            gamemanager.shopPanel[0].SetActive(true);
        if (other.gameObject.name == "HealZone"){
            InvokeRepeating("Heal",1,1.5f);
            gamemanager.shopPanel[1].SetActive(true);
            }
        if (other.gameObject.tag == "Respawn"){
            isInSpawn = true;
            isSword = false;
            InvokeRepeating("Heal",2,2);
            gamemanager.PlayerRespawn(isInSpawn);
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" && isBeatable){
            StartCoroutine("beatable");
            health-=damaged;
            gamemanager.HealthChange(health);
            if (health <= 0) gamemanager.PlayerDead();
            else
                rigid.velocity = new Vector3(transform.position.x - other.transform.position.x,
                transform.position.y - other.transform.position.y,0).normalized*15;
        }   
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Respawn"){
            isInSpawn = false;
            CancelInvoke("Heal");
            gamemanager.PlayerRespawn(isInSpawn);
            }
        if (other.gameObject.name == "Shop")
            gamemanager.shopPanel[0].SetActive(false);
        if (other.gameObject.name == "HealZone"){
            CancelInvoke("Heal");
            gamemanager.shopPanel[1].SetActive(false);
            }
    }
}