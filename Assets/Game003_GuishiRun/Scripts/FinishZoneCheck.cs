using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZoneCheck : MonoBehaviour
{
    private bool hasFinished = false;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasFinished)
        {
            Debug.Log("触发通关");
            hasFinished = true;
        }
    }
}
