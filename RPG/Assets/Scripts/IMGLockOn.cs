using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IMGLockOn : MonoBehaviour
{
    public Transform target;
    public LookTheEnemy lookEnemy;
    public Image imgLockon;
    public float imgY;

    void Update()
    {
        if (lookEnemy.estaMirando)
        {
            Vector3 playerPos = target.position - transform.position;
            imgLockon.transform.position = transform.position - Vector3.up * imgY;
            imgLockon.transform.rotation = Quaternion.LookRotation(playerPos);
            imgLockon.gameObject.SetActive(true);
        } else
        {
            imgLockon.gameObject.SetActive(false);
        } 
    }
}
