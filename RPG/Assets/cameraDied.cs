using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraDied : MonoBehaviour
{
    public PlayerMovement p1;
    public Animator anim;
    public Camera camMain, camAim;

    void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (p1.estaMorto)
        {
            anim.SetBool("morreu", true);
        }
    }
}
