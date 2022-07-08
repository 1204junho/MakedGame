using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gamemanager;
    public Objectmanager objectmanager;
    public GameObject fireLight;
    public Text leftBulletUI;
    public AudioSource WalkSound,FireSound,ReloadSound;
    static float sen = 1;
    float speed = 0.1f;
    int ammo = 0, bulletSpeed = 30;
    public int maxAmmo = 12;
    bool isReLoad,isShoot;
    private void FixedUpdate() {
       ViewPoint();
       if (Input.anyKey){
            Move();
            Shot();
        }
    }
    private void ViewPoint(){
        sen = sen >= 0.25f ? sen + (int)Input.mouseScrollDelta.y*0.1f : 0.25f;
        Vector3 view = new Vector3(transform.rotation.eulerAngles.x-Input.GetAxis("Mouse Y")*10*sen,transform.rotation.eulerAngles.y+Input.GetAxis("Mouse X")*10*sen,0);
        if(view.x > 180 && view.x < 290) view.x = 290;
        else if(view.x < 180 && view.x > 50) view.x = 50;
        transform.rotation = Quaternion.Euler(view);
    }
    private void Move(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if(h == 0 && v == 0) {
            WalkSound.mute = true;
            return;
        }
        WalkSound.mute = false;
        transform.position += (Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * new Vector3(h,0,v)).normalized*speed;
        if(transform.position.sqrMagnitude > 4400)
            transform.position += (Vector3.left * transform.position.x + Vector3.back * transform.position.z)*0.25f;
    }
    private void Shot(){
        if (Input.GetMouseButton(0) && ammo > 0 && !isReLoad && !isShoot){
            ammo--;
            StartCoroutine("WhenShot");
            FireSound.Play();
            leftBulletUI.text = string.Format("Ammo\n " + ammo + " / {0}",maxAmmo);
            Vector3 viewVec = transform.rotation * Vector3.forward;
            objectmanager.Spawn("bullet",transform.position + viewVec,viewVec * bulletSpeed,viewVec);
            }
        if (Input.GetKeyDown("r") || Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && ammo <= 0))
            StartCoroutine("ReLoad");
    }
    IEnumerator ReLoad(){
        if (isReLoad) yield break;
        ReloadSound.Play();
        isReLoad = true;
        leftBulletUI.text = "ReLoding!";
        ammo = maxAmmo;
        yield return new WaitForSeconds(1.5f);
        isReLoad = false;
        leftBulletUI.text = string.Format("Ammo\n {0} / {1}",ammo,maxAmmo);    
    }
    IEnumerator WhenShot(){
        fireLight.SetActive(true);
        isShoot = true;
        yield return new WaitForSeconds(0.1f);
        fireLight.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        isShoot = false;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Item"){
            other.gameObject.SetActive(false);
            switch (other.gameObject.name)
            {
                case "BulletLoad(Clone)":
                    ammo += 3;
                    if (!isReLoad) leftBulletUI.text = string.Format("Ammo\n {0} / {1}",ammo,maxAmmo);
                    break;
                case "BulletPlus(Clone)":
                    maxAmmo++;
                    leftBulletUI.text = string.Format("Ammo\n " + ammo + " / {0}",maxAmmo);
                    break;
                case "BulletSpeed(Clone)":
                    bulletSpeed += 5;
                    break;
                case "Speed(Clone)":
                    speed += 0.025f;
                    break;
                case "TowerHeart(Clone)":
                    gamemanager.towerHealth.value+=2;
                    gamemanager.towerLight.range = gamemanager.towerHealth.value*10;
                    break;
                default:
                    Debug.Log("Not Valid Item");
                    break;
            }
        }
    }
}

