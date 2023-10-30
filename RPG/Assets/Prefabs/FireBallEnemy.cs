using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEnemy : MonoBehaviour
{
    public float speedFire;
    Enemy enemy;
    public GameObject explosaoEffect;

    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        transform.localScale = Vector3.zero;
    }

   
    void Update()
    {
        if (enemy.aumentadoSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 4f);
        }
        if (enemy.bolaPodeIr)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speedFire);
            Destroy(gameObject, 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("FireBall"))
        {
            Vector3 posDaExplosao = transform.position + transform.forward * 1;
            Instantiate(explosaoEffect, posDaExplosao, Quaternion.identity);
        }
    }
}
