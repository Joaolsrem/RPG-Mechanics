using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalsJavali : MonoBehaviour
{
    public int life;
    public int darComida;
    public PlayerMovement p1;
    bool estaMorto;

    public void GetHit(int attackDamage)
    {
        if (life > 0)
        {
            life -= Random.Range(5, 20);
            darComida = Random.Range(5, 10);
        }
        if (life <= 0 && !estaMorto)
        {
            p1.comidaRuim += darComida;
            life = 0;
            estaMorto = true;
        }
    }
}
