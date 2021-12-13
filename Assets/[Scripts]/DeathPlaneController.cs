using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneController : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = playerSpawnPoint.position;
            other.gameObject.GetComponent<PlayerBehaviour>().DecreseHP();
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().DecreseHP();
            other.transform.position = playerSpawnPoint.position;
        }
        else
        {
            other.gameObject.SetActive(false);
        }
    }
}
