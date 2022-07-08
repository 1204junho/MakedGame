using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Player player;
    const int EnemyLen = 10;
    public int shotDelay = 25;
    public int coin;
    public Text coinText;
    public Text ShotText;
    public Text HealthText;
    public Text[] UpgradeText;
    public Slider playerHealth, senSlider, bgmSlider, SFXSlider;
    public GameObject[] Enemys;
    public GameObject[] drone;
    public GameObject rusher, ShotButton,blast,gameOverTank,pauseScreen;
    public AudioSource crushSound,BGM;
    int score;
    int[] UpgradeLevel = {0,0,0};
    bool isLock = false;
    Dictionary<float, WaitForSeconds> waitSec;
    private void Start() {
        Time.timeScale = 1;
        waitSec = new Dictionary<float, WaitForSeconds>();
        waitSec[.1f] = new WaitForSeconds(0.1f);
        waitSec.Add(1f,new WaitForSeconds(1f));
        waitSec.Add(2.5f,new WaitForSeconds(2.5f));
        waitSec.Add(.25f,new WaitForSeconds(.25f));
        StartCoroutine("Spawn");
        StartCoroutine("Fix");
    }
    public void Pause(){
        float ts = Time.timeScale;
        Time.timeScale = 1 - ts;//toggle.
        pauseScreen.SetActive(ts == 1);
    }
    public void SoundControl(int num){
        switch (num)
        {
            case 0:
                BGM.volume = bgmSlider.value;
                break;
            case 1:
                player.sounds.volume = SFXSlider.value;
                crushSound.volume = SFXSlider.value;
                break;
        }
    }
    public void CoinAmountChange(int num){
        coin += num;
        coinText.text = " " + coin;
    }
    public void PlayerHealthChange(int num){
        if(num < 0) {
            crushSound.Play();
            if(playerHealth.value < -num) GameOver();
            
            }
        playerHealth.value += num;
        HealthText.text = playerHealth.value + " / " + playerHealth.maxValue + "(+" +(int)playerHealth.maxValue/20 + ")";
    }
    void GameOver(){
        StopCoroutine("Spawn");
        foreach (var enemy in Enemys)
            enemy.SetActive(false);

        ShotButton.GetComponent<Button>().interactable = false;
        ShotText.text = "Oops.";
        playerHealth.gameObject.SetActive(false);
        StartCoroutine("MoveGameOverTank");
        PlayerPrefs.SetInt("HighScore",spawnTurn);
        PlayerPrefs.Save();
    }
    IEnumerator MoveGameOverTank(){
        var t = new WaitForFixedUpdate();
        while(gameOverTank.transform.position.x > 0){
            gameOverTank.transform.position += Vector3.left*0.375f;
            yield return t;
        }
        blast.transform.localScale*=2;
        blast.transform.position = player.gameObject.transform.position;
        blast.SetActive(true);
        
    }
    public void Restart(){
        Debug.Log("RE");
        SceneManager.LoadScene(0);
    }
    IEnumerator Fix(){
        while (true)
        {
            PlayerHealthChange((int)playerHealth.maxValue/20);
            yield return waitSec[1f];
        }
    }
    const int UpgradeIncrease = 20;
    public void Upgrade(int num){
        if(coin >= UpgradeLevel[num]*UpgradeIncrease+10){
            CoinAmountChange(-UpgradeLevel[num]*UpgradeIncrease-10);
            UpgradeLevel[num]++;
            UpgradeText[num].text = "LV : " + UpgradeLevel[num] + "\n" + (UpgradeLevel[num]*UpgradeIncrease+10) + " Coin";
            switch(num){
                case 0:
                    player.power++;
                    break;
                case 1:
                    shotDelay--;
                    break;
                case 2:
                    playerHealth.maxValue +=10;
                    break;
                default:
                    Debug.LogError("Out of Range at Upgrade Order.");
                    break;
            }
        }
    }
    public void sensitivityChange(){
        player.sensitivity = senSlider.value;
    }
    public void TryShot(){ if(!isLock) StartCoroutine("Reload");}
    IEnumerator Reload(){
        isLock = true;
        player.Shot();
        for (int i = shotDelay; i > 0; i--)
        {
            ShotText.text = i/10 + "." + i%10 +"sec";
            yield return waitSec[.1f];
        }
        isLock = false;
        ShotText.text = "Fire!";
    }
    public IEnumerator Blast(Vector3 pos){
        blast.transform.position = pos; 
        blast.SetActive(true);
        yield return waitSec[.25f];
        blast.SetActive(false);
     }
    WaitForSeconds SecChange(float time){
        if(!waitSec.ContainsKey(time)){
            waitSec.Add(time,new WaitForSeconds(time));
        }
        return waitSec[time];
    }
    public int spawnTurn = 0;
    IEnumerator Spawn(){
        WaitForSeconds sec = SecChange(2.5f);
        while (true)
        {
            Enemys[spawnTurn % Enemys.Length].SetActive(true);
            Enemys[spawnTurn++ % Enemys.Length].transform.position = new Vector2(10,-4+2*Random.value);
            Debug.Log(spawnTurn);
            switch (spawnTurn)
            {
                case 10:
                    Destroy(Enemys[0]);
                    Enemys[0] = Instantiate(drone[0], Vector3.zero,transform.rotation);
                    Enemys[0].SetActive(false);
                    Destroy(Enemys[4]);
                    Enemys[4] = Instantiate(drone[0], Vector3.zero,transform.rotation);
                    Enemys[4].SetActive(false);
                    break;
                case 30:
                    sec = SecChange(2f);
                    break;
                case 45:
                    Destroy(Enemys[6]);
                    Enemys[6] = Instantiate(rusher, Vector3.zero,transform.rotation);
                    Enemys[6].SetActive(false);
                    break;
                case 75:
                    Destroy(Enemys[5]);
                    Enemys[5] = Instantiate(drone[1], Vector3.zero,transform.rotation);
                    Enemys[5].SetActive(false);
                    Destroy(Enemys[8]);
                    Enemys[8] = Instantiate(drone[1], Vector3.zero,transform.rotation);
                    Enemys[8].SetActive(false);
                    break;
                case 90:
                    sec = SecChange(1.75f);
                    Destroy(Enemys[1]);
                    Enemys[1] = Instantiate(rusher, Vector3.zero,transform.rotation);
                    Enemys[1].SetActive(false);
                    break;
                case 120:
                    sec = SecChange(1.5f);
                    break;
                case 150:
                    sec = SecChange(1.25f);
                    break;
                case 180:
                    sec = SecChange(1f);
                    break;
                default:
                    break;
            }
            yield return sec;
        }
    }
}
