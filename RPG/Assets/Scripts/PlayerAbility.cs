using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbility : MonoBehaviour
{
    [Header("Bola de fogo")]
    public Transform firePoint;
    public GameObject prefabFBall;
    public bool abilidadeAtivada;
    public PlayerMovement p1;
    [Space]

    [Header("Super bola de fogo")]
    public Transform ballPoint;
    public bool abilityAtivada;
    public float timeHoldingMouse;
    public bool bigFireBall;
    [Space]

    // Aumentar tamanho da bola de fogo, de uma maneira suave
    public bool prontoBola;
    public bool podeIr;
    [Space]

    public bool clicouTecla;

    public Slider abilityBar;
    public float TimeCount;

    void Start()
    {
        p1 = GetComponent<PlayerMovement>();
        abilityBar.value = 0;
    }

    void Update()
    {
        TimeCount += Time.deltaTime;
        if (abilityBar.value < abilityBar.maxValue)
        {
            abilityBar.value = TimeCount;
        }
        if (abilityBar.value >= abilityBar.maxValue)
        {
            TimeCount = 15;
            abilityBar.value = abilityBar.maxValue;
        }

        if (abilidadeAtivada)
        {
            abilityBar.value = 0;
            TimeCount = 0;
            timeHoldingMouse = 0;
            abilidadeAtivada = false;
        }

        BigFireAbility();
        p1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void BigFireAbility()
    {
        if (abilityBar.value == abilityBar.maxValue && Input.GetKeyDown(KeyCode.K) && !p1.anim.GetBool("atacando") && !abilidadeAtivada)
        {
            clicouTecla = true;
        }
        if (abilityBar.value == abilityBar.maxValue && Input.GetKeyUp(KeyCode.K) && !p1.anim.GetBool("atacando") && !abilidadeAtivada)
        {
            timeHoldingMouse = 0;
            clicouTecla = false;
        }

        if (clicouTecla)
        {
            timeHoldingMouse += Time.deltaTime;
        }

        if (timeHoldingMouse >= 2)
        {
            podeIr = false;
            clicouTecla = false;
            StartCoroutine(spawnBigFire());
            timeHoldingMouse = 0;
        }
    }

    IEnumerator spawnBigFire()
    { 
        if (!bigFireBall)
        {
            podeIr = false;
            bigFireBall = true;
            prontoBola = false;
            Instantiate(prefabFBall, ballPoint.transform.position, ballPoint.transform.rotation);
            prefabFBall.transform.localScale = Vector3.zero;
            p1.anim.SetBool("attackSpecial", true);
            yield return new WaitForSeconds(1f);
            prontoBola = true;
            p1.anim.speed = 0;
            yield return new WaitForSeconds(2f);
            p1.anim.speed = 1;
            prontoBola = false;
            abilidadeAtivada = true;
            p1.anim.SetBool("attackSpecial", false);
            podeIr = true;
            bigFireBall = false;
        }
    }

}
