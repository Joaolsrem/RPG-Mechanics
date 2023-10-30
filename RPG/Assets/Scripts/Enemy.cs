using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int Life;
    public float lookRadius;
    [Space]

    [Header("Attack Config")]
    public float AttackRange;
    public int AttackDamage;
    public bool readyAttack;
    [Space]

    [Header("Enemy Ability")]
    public GameObject fireBall;
    public Transform pointFire;
    public float TimeToSpawn;
    public bool spawnBola;
    public bool aumentadoSize;
    public bool bolaPodeIr;
    [Space]

    [Header("Enemy Patrol")]
    public Transform[] points;
    public Transform firstPoint;
    public int currentPoint;
    public bool canPatrol;
    bool encostouPoint;
    public bool estaPatrol;
    [Space]

    Vector3 dest;
    Rigidbody rb;
    public Animator anim;
    public float TimeCount;
    public Transform p1;
    PlayerMovement player;
    NavMeshAgent agent;
    public LookTheEnemy LookEnemy;
    void Start()
    {
        estaPatrol = true;
        TimeCount = 2.6f;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Life <= 0)
        {
            Destroy(gameObject, 1f);
        }
        EnemyAI();

    }

    void EnemyPatrol()
    {
        if (currentPoint >= 4)
        {
            currentPoint = 0;
        }
        if (!encostouPoint)
        {
            anim.SetBool("walk", true);
        }

        dest = points[currentPoint].transform.position;
        agent.SetDestination(dest);
    }

    IEnumerator bolaDeFogo()
    {
        if (!spawnBola)
        {
            anim.SetBool("walk", false);
            spawnBola = true;
            bolaPodeIr = false;
            aumentadoSize = false;
            fireBall.transform.localScale = Vector3.zero;
            Instantiate(fireBall, pointFire.transform.position, pointFire.transform.rotation);
            agent.speed = 0;
            yield return new WaitForSeconds(1f);
            aumentadoSize = true;
            yield return new WaitForSeconds(2f);
            agent.speed = 2;
            bolaPodeIr = true;
            aumentadoSize = false;
            spawnBola = false;
        }
    }

    void EnemyAI()
    {
        float distance = Vector3.Distance(p1.position, transform.position);
        float distancePatrol = Vector3.Distance(firstPoint.position, transform.position);

        if (distancePatrol > lookRadius && distance > lookRadius && !estaPatrol)
        {
            StartCoroutine(olhandoPorAi());
        }
      
        if (distance > lookRadius && !canPatrol && estaPatrol)
        {
            StartCoroutine(irPatrolhar());
        }
        

        if (distance <= lookRadius)
        {
            readyAttack = false;
            estaPatrol = false;
            if (distance >= 2.8f)
            {
                if (!readyAttack && !spawnBola)
                {
                    TimeToSpawn += Time.deltaTime;
                }
                if (readyAttack)
                {
                    TimeToSpawn = 0;
                }
                if (!readyAttack && TimeToSpawn >= 5)
                {
                    StartCoroutine(bolaDeFogo());
                    TimeToSpawn = 0;
                }
            }

            LookEnemy.podeMirar = true;
            agent.SetDestination(p1.position);
            if (!spawnBola)
            {
                anim.SetBool("walk", true);
            }
            if (distance <= agent.stoppingDistance)
            {
                readyAttack = true;
                OlharParaPlayer();
                anim.SetBool("walk", false);
                TimeCount -= Time.deltaTime;
                if (TimeCount <= 0)
                {
                    StartCoroutine(Attacar());
                    TimeCount = 2.6f;
                }
            }

            if (distance > agent.stoppingDistance)
            {
                player.inimigoHitou = false;
                readyAttack = false;
                TimeCount = 2.6f;
            }
        } else
        {
            LookEnemy.podeMirar = false;
        }
        

        if (player.bateuEnemy && TimeCount < 3.5f)
        {
            TimeCount = 2.6f;
            player.bateuEnemy = false;
        }
    }

    IEnumerator olhandoPorAi()
    {
        if (!canPatrol)
        {
            canPatrol = true;
            yield return new WaitForSeconds(2f);
            anim.SetBool("olhando", true);
            agent.speed = 0;
            anim.SetBool("walk", false);
            yield return new WaitForSeconds(3f);
            anim.SetBool("olhando", false);
            encostouPoint = false;
            estaPatrol = true;
            canPatrol = false;
        }
    }

    IEnumerator irPatrolhar()
    {
        yield return new WaitForSeconds(2f);
        estaPatrol = true;
        agent.speed = 3;
        EnemyPatrol();
    }

    IEnumerator Attacar()
    {
        readyAttack = true;
        agent.speed = 0;
        anim.SetBool("atacar", true);
        anim.SetBool("walk", false);
        yield return new WaitForSeconds(1.1f);
        AtcPegarPosicao();
        player.tomouDano = true;
        if (player.estaDefendendo)
        {
            player.defenseValue -= 5;
        }
        yield return new WaitForSeconds(1.4f);
        player.tomouDano = false;
        anim.SetBool("atacar", false);
        anim.SetBool("walk", true);
        player.inimigoHitou = false;
        agent.speed = 2;
        readyAttack = false;
    }

    void OlharParaPlayer()
    {
        Vector3 direcao = (player.transform.position - transform.position).normalized;
        Quaternion lookRotacao = Quaternion.LookRotation(new Vector3(direcao.x, 0, direcao.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotacao, Time.deltaTime * 5f);
    }

    public void AtcPegarPosicao()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward), AttackRange))
        {
            if (c.transform.CompareTag("Player"))
            {
                if (!player.estaDefendendo)
                {
                    player.Life -= AttackDamage;
                }
                if (player.estaDefendendo)
                {
                    player.tomouDano = true;
                    player.inimigoHitou = true;
                    Debug.Log("Player esta defendendo");
                }
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere((transform.position + transform.forward), AttackRange);
    }

    public void GetHit(int attackDamage)
    {
        agent.speed = 0;
        anim.SetBool("tomoDano", true);
        StartCoroutine(AcabarAnim());
        Life -= attackDamage;
        rb.AddForce((-transform.forward + transform.up) * 2f, ForceMode.Impulse);
    }

    IEnumerator AcabarAnim()
    {
        if (anim.GetBool("tomoDano"))
        {
            yield return new WaitForSeconds(1.2f);
            agent.speed = 2;
            anim.SetBool("tomoDano", false);
        }
    }
    IEnumerator AcabarAnim1()
    {
        if (anim.GetBool("tomoDano"))
        {
            yield return new WaitForSeconds(1.2f);
            anim.SetBool("tomoDano", false);
        }
    }

    private void OnTriggerEnter(Collider colisao)
    {
        if (colisao.gameObject.tag == "FireBall")
        {
            anim.SetBool("tomoDano", true);
            StartCoroutine(AcabarAnim1());
            Life -= 45;
        }
        if (colisao.gameObject.tag == "pointsPatrol")
        {
            encostouPoint = true;
            StartCoroutine(irAtePonto());
        }
    }

    IEnumerator irAtePonto()
    {
        anim.SetBool("walk", false);
        yield return new WaitForSeconds(2f);
        anim.SetBool("walk", true) ;
        currentPoint++;
    }

}
