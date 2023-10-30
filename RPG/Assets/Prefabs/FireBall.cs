using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speedfire;
    public PlayerAbility pAb;

    void Start()
    {
        pAb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbility>();
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (pAb.prontoBola)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 4f);
        }
        if (!pAb.bigFireBall)
        {
            transform.Translate(Vector3.forward * speedfire * Time.deltaTime);

            Destroy(gameObject, 5f);
        }
        if (pAb.podeIr)
        {
            transform.Translate(Vector3.forward * speedfire * Time.deltaTime);

            Destroy(gameObject, 5f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("FireBallEnemy"))
        {
            Debug.Log("fireball bateu em fireballenemy");
        }
    }
}
