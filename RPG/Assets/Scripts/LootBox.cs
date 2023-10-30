using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    [Header("Animations and Audio")]
    public Animator anim;
    public AudioSource audio;
    [Space]

    public bool bauAberto;
    public PlayerMovement p1;

    void Start()
    {
        anim = GetComponent<Animator>();
        p1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!bauAberto && p1.encostouBau && Input.GetKeyDown(KeyCode.F))
        {
            p1.Money += 30;
            p1.comida += 5;
            anim.SetBool("abriu", true);
            audio.Play();
            bauAberto = true;
        }
    }
}
