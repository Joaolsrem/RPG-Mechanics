using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTheEnemy : MonoBehaviour
{
    public Transform enemy;
    public bool podeMirar;
    public bool estaMirando;
    public Camera camPlayer;
    public Camera camAiming;
    
    void Start()
    {
        camAiming.enabled = false;
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && podeMirar)
        {
            estaMirando = true;
        }
        if (!podeMirar && estaMirando)
        {
            estaMirando = false;
            camPlayer.enabled = true;
        }
  
        if (estaMirando)
        {
            Vector3 playercam = enemy.position - transform.position;
            Vector3 smoothCam = Vector3.Lerp(transform.position, playercam, 0.05f);
            camPlayer.enabled = false;
            camAiming.enabled = true;
            camAiming.transform.LookAt(smoothCam);
        }
        camAiming.transform.rotation = Quaternion.Euler(9.01f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
