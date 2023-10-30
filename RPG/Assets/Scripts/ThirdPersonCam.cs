using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform player;
    public float YOffset;
    public float LimitRotation;
    public float Sensibility;

    float rotX;
    float rotY;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    { 
        float mouse_X = Input.GetAxis("Mouse Y");
        float mouse_Y = Input.GetAxis("Mouse X");

        rotX -= mouse_X * Sensibility * Time.deltaTime;
        rotY += mouse_Y * Sensibility * Time.deltaTime;

        rotX = Math.Clamp(rotX, -LimitRotation, LimitRotation);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);

    }

    void LateUpdate()
    {
        transform.position = player.position + player.up * YOffset;
    }
}


