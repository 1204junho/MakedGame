using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public bool isAir, isShield = false;
    public bool[] buttons ,isButtonDown;
    public int speed, score;
    public int h,v;
    public GameObject Shield;
    Rigidbody2D rigid;
    public AudioSource pop;
    SpriteRenderer spriteRenderer;
    public Gamemanager Gamemanager;
    void Awake()
    {
        h = 0; v = 0;
        pop = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid.velocity = new Vector2(rigid.velocity.x,-8);
    }
    void FixedUpdate()
    {
        Move();
        Ray_Jump();
    }
    public void Buttons(int num){
        if(num == 0){
            buttons[0] = true;
            buttons[1] = false;
        }
        if(num == 1){
            buttons[0] = false;
            buttons[1] = true;
        }
        if(num == 2){
            buttons[2] = true;
            buttons[3] = false;
            }
        if(num == 3){
            buttons[2] = false;
            buttons[3] = true;
        }
    }    
    public void ButtonDown(int num){
        isButtonDown[num] = true;        
    }
    public void ButtonUp(int num){
        buttons[num] = false;
        switch (num)
        {
            case 0:
            case 1:
            isButtonDown[0] = false;
            break; 
            case 2:
            case 3:
            isButtonDown[1] = false;    
            break;
        }
        
    }
    public void GetSheild(bool shield){
        Shield.SetActive(shield);
        isShield = shield;
    }
    void Move(){
        h = (int)Input.GetAxisRaw("Horizontal");
        if(isButtonDown[0])
        {
            if(buttons[0]) h = -1;
            if(buttons[1]) h = 1;
        }

        if (isAir == false)
            rigid.AddForce(Vector3.right* Time.deltaTime  * (5 * h * speed -7), ForceMode2D.Impulse);
        else rigid.AddForce(Vector3.right * h * 5 * speed * Time.deltaTime, ForceMode2D.Impulse);
        if (h != 0)
            spriteRenderer.flipX = h == 1 ? true : false;
    }
    void Ray_Jump(){
        RaycastHit2D ray = Physics2D.Raycast(rigid.position + Vector2.down * 0.7f, Vector3.down,0.05f);
        v = (int)Input.GetAxisRaw("Vertical");
        if(isButtonDown[1]){
                if(buttons[2]) v = 1;
                if(buttons[3]) v = -1;
            }
        if (ray.collider == null)
            rigid.AddForce(Vector3.down * (20 - v * 10) * Time.deltaTime, ForceMode2D.Impulse);
        else if(ray.collider.tag == "Platform"){
                if(v != 0)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, 3 + 7*v);
                        isAir = true;
                    }
                else if(rigid.velocity.y < 0)
                    {
                        rigid.gravityScale = 0;
                        rigid.velocity = new Vector2(rigid.velocity.x,0);
                        isAir = false;
                    }
                }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item"){
            Item item = collision.gameObject.GetComponent<Item>();
            collision.gameObject.SetActive(false);
            pop.Play(0);
            switch (item.ability)
            {
                case "SpeedUp":
                    speed++;
                    Gamemanager.SpeedChange();
                    break;
                case "SpeedDown":
                    speed--;
                    Gamemanager.SpeedChange();
                    break;
                case "Shield":
                    GetSheild(true);
                    break;
                case "Score":
                    score++;
                    Gamemanager.ScoreChange(score);
                    break;
                case "StopPurple":
                    Gamemanager.StopPurple();
                    break;
                case "Slow":
                    Gamemanager.Slow();
                    break;
                default:
                    Debug.Log("item 404");
                    break;
            }
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
            if (isShield)
                GetSheild(false);
            else
            {
                this.gameObject.SetActive(false);
                Gamemanager.GameOver();
            }
        }
        else if(collision.gameObject.tag == "Border"){
            this.gameObject.SetActive(false);
            Gamemanager.GameOver();
        }
    }
}
