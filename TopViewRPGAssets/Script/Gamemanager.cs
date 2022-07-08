using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Gamemanager : MonoBehaviour
{
    public Player Player;
    public Image[] healthImage;
    public Sprite[] WeaponImage;
    public Image Weapon;
    public GameObject battlePanel, BOSS, bossWarning, gameOverPanel, respawn, healZone, reButton, dashButton, bossButton, pauseScreen, scSword;
    public GameObject[] Weapons, backGound,shopPanel;
    AudioSource BGM;
    public Text infoEnemy, gold, durability, scoreText, lifeText, potionText, posText;
    public Text[] priceTextChange;
    public Objectmanager objectmanager;
    public int count, maxhealth, maxdur, difficult;
    public int[] priceCount;
    int score, potionPoint;
    string type; 
    void Start()
    {   
        score = 0; potionPoint = 0; maxhealth = 4;
        Player.Heal(4);
        gold.text = Player.hasGold.ToString();
        InvokeRepeating("BackGoundMove",1,1);
        BGM = GetComponent<AudioSource>();
    }
    void BackGoundMove(){
        posText.text = string.Format("[ {0:0.0}, {1:0.0} ]", Player.transform.position.x,Player.transform.position.y);
        foreach (var bg in backGound)
        {
            Vector3 dirToPlayer = new Vector3(Player.transform.position.x-bg.transform.position.x,Player.transform.position.y-bg.transform.position.y,0);
            if (dirToPlayer.x > 32)
                bg.transform.position += Vector3.right*64;
            if (dirToPlayer.x < -32)
                bg.transform.position += Vector3.left*64;
            if (dirToPlayer.y > 32)
                bg.transform.position += Vector3.up*64;
            if (dirToPlayer.y < -32)
                bg.transform.position += Vector3.down*64;
        }
    }
    public void HealthChange(int health){
        for(int i = 0; i < maxhealth; i++)
            healthImage[i].color = new Color(1,1,1,0);
        for(int i = 0; i < health; i++)
            healthImage[i].color = new Color(1,1,1,1);
    }
    public void PotionChange(int point){
        if (potionPoint >= 10 && point == -10)
            Player.Heal(2);
        potionPoint = potionPoint + point >= 0 ? potionPoint + point : potionPoint;
        potionText.text = potionPoint + " / 10";
    }
    public void EnemyInfo(string name, int health, int max){
        Player.durability--;
        WeaponDurability();
        if(health > 0){
            battlePanel.SetActive(true);
            infoEnemy.text = name + "\nHealth : " + health + " / " + max;
        }
        else
        {
            battlePanel.SetActive(false);
            count++;
            if (count%40 == 0 && count > 0){
                bossButton.SetActive(true);
                difficult++;
                }
            if (count == 120)
                StartCoroutine("BossSummon");
        }
    }
    public void ScoreChange(int addScore){
        score += addScore;
        scoreText.text = string.Format("{0:n0}", score);
    }
    public void SpawnEnemy(){
        int ran = Random.Range(0, count);
        type = ran < 20 ? "slime" : "flyingcube";
        objectmanager.Spawn(type);
    }
    
    IEnumerator BossSummon(){
        bossButton.SetActive(false);
        bossWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        respawn.SetActive(false);
        CancelInvoke("SpawnEnemy");
        objectmanager.DeSpawn("slime");
        objectmanager.DeSpawn("flyingcube");
        BOSS.SetActive(true);
        bossWarning.SetActive(false);
        count = 0;
        if (difficult % 5 == 0 && difficult > 0){
            scSword.SetActive(true);
            Player.damaged++;
        }
    }
    public void ItemDrop(string type, Vector2 pos){
        objectmanager.ItemDrop(type,pos);
    }
    public void WeaponDurability(){
        if (Player.durability <= 0){
            Weapon.gameObject.SetActive(false);
            return;
        }
        Weapon.gameObject.SetActive(true);
        durability.text = Player.durability + "/" + maxdur;
    }
    public void PlayerDead(){
        gameOverPanel.SetActive(true);
        reButton.SetActive(true);
        lifeText.text = string.Format("Score : {0:n0}",score);
        if (score > PlayerPrefs.GetInt("HighScore")){
            PlayerPrefs.SetInt("HighScore",score);
            PlayerPrefs.Save();
            }
        Time.timeScale = 0f;
        BGM.Stop();
    }
    public void PlayerRespawn(bool isIn)
    {
        if(isIn)
            CancelInvoke("SpawnEnemy");
        else
            InvokeRepeating("SpawnEnemy",2,3);
    }
    public void Shoping(int i){
        switch (i)
        {
            case 0://woodsword
                if (Player.hasGold < 15) return;
                Player.GoldChange(-15);
                Player.sword = Weapons[0];
                Player.durability = 30;
                maxdur = Player.durability;
                Weapon.sprite = WeaponImage[0];
                WeaponDurability();
                break;
            case 1://sword
                if (Player.hasGold < 50) return;
                Player.GoldChange(-50);
                Player.sword = Weapons[1];
                Player.durability = 80;
                maxdur = Player.durability;
                Weapon.sprite = WeaponImage[1];
                WeaponDurability();
                break;
            case 2://speed
                if (Player.hasGold < priceCount[0]) return;
                Player.GoldChange(-priceCount[0]);
                priceCount[0]+=10;
                priceTextChange[0].text = priceCount[0] + " GOLD";
                Player.speed++;
                if (Player.speed == 15)
                    dashButton.SetActive(true);
                break;
            case 3://dash
                if (Player.hasGold < 30) return;
                Player.GoldChange(-30);
                Player.isCanDash = true;
                dashButton.SetActive(false);
                break;
            case 4://maxhealth
                if (Player.hasGold < priceCount[1] || maxhealth >= 10)
                    return;
                Player.GoldChange(-priceCount[1]);
                priceCount[0]+=15;
                priceTextChange[1].text = priceCount[1] + " GOLD";
                maxhealth++;
                break;
            case 5://potion
                if (Player.hasGold < 15) return;
                Player.GoldChange(-15);
                PotionChange(20);
                break;
            case 6://slimecubesword
                if (Player.hasGold < 120) return;
                Player.GoldChange(-120);
                Player.sword = Weapons[2];
                Player.durability = 100;
                maxdur = Player.durability;
                Weapon.sprite = WeaponImage[2];
                WeaponDurability();
                break;
        }
    }
    public void ReStart(){
        Time.timeScale = 1f;
         SceneManager.LoadScene(1);
    }
    public void GoTitle(){
        Time.timeScale = 1f;
         SceneManager.LoadScene(0);
    }
    public void BossSummonButton(){
        StartCoroutine("BossSummon");
    }
    public void Pause(bool doPause){
        Time.timeScale = doPause ? 0f : 1f;
        pauseScreen.SetActive(doPause);
        if(doPause) BGM.Pause();
        else BGM.Play();
    }
}
