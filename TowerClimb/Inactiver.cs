using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inactiver : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag != "Player")
            other.gameObject.SetActive(false);
    }
}
