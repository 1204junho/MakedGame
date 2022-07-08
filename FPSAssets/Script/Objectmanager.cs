using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectmanager : MonoBehaviour
{
    public GameObject BulletPrefab, EnemyPrefab, FlyEnemyPrefab, BulletLoadPrefab, BulletPlusPrefab, BulletSpeedPrefab, SpeedPrefab, TowerHeartPrefab;
    GameObject[] Bullet, Enemy, FlyEnemy, BulletLoad, BulletPlus, BulletSpeed, Speed, TowerHeart;
    void Start()
    {
        Bullet          = Gen(Bullet,BulletPrefab,12);
        Enemy           = Gen(Enemy,EnemyPrefab,16);
        FlyEnemy        = Gen(FlyEnemy,FlyEnemyPrefab,8);
        BulletLoad      = Gen(BulletLoad,BulletLoadPrefab,10,true);
        BulletPlus      = Gen(BulletPlus,BulletPlusPrefab,5,true);
        BulletSpeed     = Gen(BulletSpeed,BulletSpeedPrefab,5,true);
        Speed           = Gen(Speed,SpeedPrefab,5,true);
        TowerHeart      = Gen(TowerHeart,TowerHeartPrefab,5,true);
    }
    GameObject[] Gen(GameObject[] Object, GameObject Prefab, int count){
        Object = new GameObject[count];
        for (int i = 0; i < count; i++){
            Object[i] = Instantiate(Prefab);
            Object[i].SetActive(false);
            }
        return Object;
    }
    GameObject[] Gen(GameObject[] Object, GameObject Prefab, int count,bool isActive){
        Object = new GameObject[count];
        for (int i = 0; i < count; i++){
            Object[i] = Instantiate(Prefab);
            Object[i].SetActive(isActive);
            }
        return Object;
    }
    public GameObject Spawn(string type, Vector3 pos){
            foreach (var i in Order(type))
                if (!i.activeSelf)
                {
                    i.SetActive(true);
                    i.transform.position = pos;
                    return i;
                }
        return null;
    }
    public GameObject Spawn(string type, Vector3 pos, Vector3 velocity, Vector3 view){
            foreach (var i in Order(type))
                if (!i.activeSelf)
                {
                    i.SetActive(true);
                    i.transform.position = pos;
                    i.transform.LookAt(view+pos);
                    Rigidbody rigid = i.GetComponent<Rigidbody>();
                    rigid.velocity = velocity;
                    return i;
                }
        return null;
    }
    GameObject[] Order(string type){
        switch (type)
        {
            case "bullet":
                return Bullet;
            case "enemy":
                return Enemy;
            case "flyenemy":
                return FlyEnemy;
            case "BulletLoad":
                return BulletLoad;
            case "BulletPlus":
                return BulletPlus;
            case "BulletSpeed":
                return BulletSpeed;
            case "Speed":
                return Speed;
            case "TowerHeart":
                return TowerHeart;
            default:
                return null;
        }
    }
}
