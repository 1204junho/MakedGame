using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("InAct");
    }
    IEnumerator InAct(){
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
}
