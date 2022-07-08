using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) {
       other.gameObject.SetActive(false);
       other.gameObject.transform.position = Vector3.left*6 + Vector3.down*-2.675f;
   }
}
