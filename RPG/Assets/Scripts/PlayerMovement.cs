using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    public int Life = 100;
    public int Money = 0;
    public float speed;
    public float speedRotation;
    public Slider LifeBar;

    // Sistema para recuperar vida
    public float timeToHealthy;
    public bool tomouDano;
    [Space]

    [Header("Hungry System")]
    public Slider barFome;
    public float fome;
    public float timeFood;
    public float timeToGetHit;

    public int comida;
    public int comidaRuim;

    public Text textComida;
    public Text textFoodBad;

    public float timeToEat;
    public bool estaComendo;
    public bool estaComendoRuim;
    bool noHaveFood;
    [Space]
    

    [Header("UI LootBox")]
    public Text TextLoot;
    public bool encostouBau;
    [Space]

    [Header("Animations and Audio")]
    public Animator anim;
    public AudioSource audio;
    [Space]

    [Header("Player Attack")]
    public int AttackDamage;
    public float ColliderRadius;
    public GameObject attackSlash;
    public Transform pointSlash;
    List<Transform> EnemiesList = new List<Transform>();
    [Space]

    [Header("Player Defense")]
    public Slider DefenseBar;
    public int defenseValue;
    public GameObject MenuDefense;
    public bool estaDefendendo;
    public bool inimigoHitou;
    public float TimeForRecovery;
    [Space]

    [Header("Player Died")]
    public bool estaMorto;
    public Camera camMain, camAim, camDie;
    public GameObject menuDie;
    [Space]

    [Header("Player Combo")]
    public GameObject effectSlash;
    public Transform pointCombo;
    public bool firstCombo;
    public bool secondCombo;
    [Space]

    bool atacando;
    [HideInInspector] public bool bateuEnemy;
    [HideInInspector] public bool encostouPorcao;
    public bool encostouFornalha;
    public LookTheEnemy lookEnemy;
    public Enemy enemy;
    public LootBox lt;

    void Start()
    {
        menuDie.SetActive(false);
        MenuDefense.SetActive(false);
        camDie.gameObject.SetActive(false);
        defenseValue = 25;
        LifeBar.maxValue = Life;
        TextLoot.enabled = false;
        barFome.maxValue = fome;
        tomouDano = false;
        barFome.value = fome;
        timeToEat = 2f;
    }

    void Update()
    {
        DefenseBar.value = defenseValue;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        if (!estaMorto)
        {
            Movement();
            Attack();
            HealthSystem();
            Shield();
            foodSystem();
        }
        PlayerHit();
        if (Life < 100)
        {
            timeToHealthy += Time.deltaTime;
        }
        if (tomouDano)
        {
            timeToHealthy = 0;
        }
        if (timeToHealthy >= 13)
        {
            Life += 8;
            timeToHealthy = 0;
        }
    }

    public void PlayerHit()
    {
        if (Life <= 0)
        {
            camDie.gameObject.SetActive(true);
            Life = 0;
            estaMorto = true;
            anim.SetBool("morreu", estaMorto);
            camAim.gameObject.SetActive(false);
            camMain.gameObject.SetActive(false);
            menuDie.SetActive(true);
        }
        if (Life > 0)
        {
            estaMorto = false;
            menuDie.SetActive(false);
        }
    }

    public void eatComidaRuim()
    {
        if (comidaRuim > 0 && barFome.value < 100)
        {
            estaComendoRuim = true;
        }

        if (comidaRuim <= 0)
        {
            StartCoroutine(semComidaRuim());
            comidaRuim = 0;
        }

    }

    IEnumerator semComidaRuim()
    {
        if (!noHaveFood)
        {
            noHaveFood = true;
            yield return new WaitForSeconds(0.1f);
            textFoodBad.fontSize += 20;
            textFoodBad.color = Color.red;
            yield return new WaitForSeconds(1f);
            textFoodBad.fontSize -= 20;
            textFoodBad.color = Color.white;
            noHaveFood = false;
        }
    }

    public void eatComidaBoa()
    {
        if (comida > 0 && barFome.value < 100)
        {
            estaComendo = true;
        }

        if (comida <= 0)
        {
            StartCoroutine(semComida());
            comida = 0;
        }
    }

    public void foodSystem()
    {
        textComida.text = comida.ToString();
        textFoodBad.text = comidaRuim.ToString();

        if (estaComendoRuim)
        {
            timeToEat -= Time.deltaTime;
        }

        if (estaComendoRuim && timeToEat <= 0)
        {
            fome += Random.Range(5, 20);
            Life -= Random.Range(5, 20);
            comidaRuim -= 1;
            timeToEat = 2f;
            estaComendoRuim = false;
        }

        if (estaComendo)
        {
            timeToEat -= Time.deltaTime;
        }
        if (estaComendo && timeToEat <= 0)
        {
            Debug.Log("Comi");
            fome += 25;
            comida -= 1;
            timeFood -= 4f;
            timeToEat = 2;
            estaComendo = false;
        }

        barFome.value = fome;

        if (barFome.value >= 100 || fome >= 100)
        {
            fome = 100;
            barFome.value = 100;
        }

        if (barFome.value > 0 && fome > 0)
        {
            timeFood += Time.deltaTime;
        }
        if (timeFood >= 10)
        {
            timeFood = 0;
            fome -= 5;
        }
        
        if (barFome.value <= 0 && fome <= 0)
        {
            fome = 0;
            barFome.value = 0;
            timeToGetHit += Time.deltaTime;
            if (timeToGetHit >= 8)
            {
                timeToGetHit = 0;
                Life -= 5;
            }
        }
    }

    IEnumerator semComida()
    {
        if (!noHaveFood)
        {
            noHaveFood = true;
            yield return new WaitForSeconds(0.1f);
            textComida.fontSize += 20;
            textComida.color = Color.red;
            yield return new WaitForSeconds(1f);
            textComida.fontSize -= 20;
            textComida.color = Color.white;
            noHaveFood = false;
        }
    }

    void Shield()
    {
        if (Input.GetButtonDown("Fire2") && !anim.GetBool("atacando") && !encostouFornalha)
        {
            estaDefendendo = true;
        }

        if (estaDefendendo && defenseValue > 0)
        {
            anim.SetBool("defesa", true);
            MenuDefense.SetActive(true);
        }

        if (inimigoHitou)
        {
            TimeForRecovery = 0;
        }
        if (defenseValue <= 0)
        {
            anim.SetBool("defesa", false);
            estaDefendendo = false;
            defenseValue = 0;
        }

        if (DefenseBar.value < DefenseBar.maxValue)
        {
            MenuDefense.SetActive(true);
        }
        

        if (!inimigoHitou && !estaDefendendo && DefenseBar.value < DefenseBar.maxValue)
        {
            TimeForRecovery += Time.deltaTime;
            if (TimeForRecovery >= 5f)
            {
                defenseValue += 5;
                TimeForRecovery = 0;
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            MenuDefense.SetActive(false);
            estaDefendendo = false;
            anim.SetBool("defesa", false);
        }

        if (defenseValue > 25)
        {
            defenseValue = 25;
        }
    }

    void Attack()
    {
        if (Input.GetMouseButton(0) && !estaDefendendo && !firstCombo && !secondCombo && !encostouFornalha)
        {
            StartCoroutine(Ataque());
            anim.SetBool("walking", false);
            anim.SetBool("walkBack", false);
        }

        if (Input.GetMouseButton(0) && !estaDefendendo && !secondCombo && firstCombo && !encostouFornalha)
        {
            StartCoroutine(comboAtaque());
            anim.SetBool("walking", false);
            anim.SetBool("walkBack", false);
        }
    }

    IEnumerator comboAtaque()
    {
        if (!atacando)
        {
            atacando = true;
            speed = 0;
            speedRotation = 0;
            secondCombo = true;
            anim.SetBool("combo", true);
            yield return new WaitForSeconds(0.5f);
            audio.Play();
            StartCoroutine(effectCombo());
            yield return new WaitForSeconds(0.8f);
            GetEnemiesRange();
            foreach (Transform enemies in EnemiesList)
            {
                Enemy inimigo = enemies.GetComponent<Enemy>();
                animalsJavali javali = enemies.GetComponent<animalsJavali>();

                if (javali != null)
                {
                    javali.GetHit(AttackDamage);
                }

                if (inimigo != null)
                {
                    bateuEnemy = true;
                    inimigo.GetHit(AttackDamage);
                }
            }
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("combo", false);
            speed = 6;
            speedRotation = 180;
            atacando = false;
            secondCombo = false;
            firstCombo = false;
        }
    }

    IEnumerator effectCombo()
    {
        yield return new WaitForSeconds(0.14f);
        Quaternion playerPos = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Quaternion attackRotation = playerPos * Quaternion.Euler(0, 180, 0);
        Instantiate(effectSlash, pointCombo.transform.position, attackRotation);
    }


    IEnumerator Ataque()
    {
        if (!atacando)
        {
            speed = 0;
            speedRotation = 0;
            atacando = true;
            firstCombo = true;
            anim.SetBool("atacando", true);
            yield return new WaitForSeconds(0.5f);
            audio.Play();
            StartCoroutine(attackEffect());
            yield return new WaitForSeconds(0.8f);
            GetEnemiesRange();
            foreach (Transform animais in EnemiesList)
            {
                animalsJavali javali = animais.GetComponent<animalsJavali>();
                if (javali != null)
                {
                    javali.GetHit(AttackDamage);
                }
            }
            foreach (Transform enemies in EnemiesList)
            {
                Enemy inimigo = enemies.GetComponent<Enemy>();
                if (inimigo != null)
                {
                    bateuEnemy = true;
                    inimigo.GetHit(AttackDamage);
                }
            }
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("atacando", false);
            speed = 6;
            speedRotation = 180;
            atacando = false;
        }
    }

    IEnumerator attackEffect()
    {
        yield return new WaitForSeconds(0.44f);
        Quaternion playerPos = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        Quaternion attackRotacao = playerPos * Quaternion.Euler(0, 180, 0);
        Instantiate(attackSlash, pointSlash.transform.position, attackRotacao);
    }

    void GetEnemiesRange()
    {
        EnemiesList.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward + transform.up * 1), ColliderRadius))
        {
            EnemiesList.Add(c.transform);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.position + transform.forward + transform.up * 1), ColliderRadius);
    }

    public void HealthSystem()
    {
        LifeBar.value = Life;
    }

    public void OnTriggerEnter(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("PorcaoVida"))
        {
            encostouPorcao = true;
        }

        if (colisao.gameObject.tag == "FireBallEnemy")
        {
            Life -= 25;
        }

        if (colisao.gameObject.CompareTag("fornalha"))
        {
            encostouFornalha = true;
        }

        if (lt.bauAberto == false)
        {
            if (colisao.gameObject.tag == "Bau")
            {
                TextLoot.enabled = true;
                encostouBau = true;
            }
        }
    }

    public void OnTriggerExit(Collider colisao)
    {
        if (colisao.gameObject.CompareTag("PorcaoVida"))
        {
            encostouPorcao = false;
        }

        if (colisao.gameObject.tag == "fornalha")
        {
            encostouFornalha = false;
        }

        if (colisao.gameObject.CompareTag("Bau"))
        {
            TextLoot.enabled = false;
            encostouBau = false;
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.W) && !anim.GetBool("atacando") && !anim.GetBool("combo"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            anim.SetBool("walking", true);
        }
        if (Input.GetKey(KeyCode.S) && !anim.GetBool("atacando") && !anim.GetBool("combo"))
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * speed);
            anim.SetBool("walkBack", true);
        }

        if (Input.GetKey(KeyCode.D) && !anim.GetBool("atacando") && !lookEnemy.estaMirando)
        {
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * speedRotation);
        }
        if (Input.GetKey(KeyCode.A) && !anim.GetBool("atacando") && !lookEnemy.estaMirando)
        {
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * speedRotation);
        }

        if (Input.GetKey(KeyCode.D) && lookEnemy.estaMirando)
        {
            Quaternion playerRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            Quaternion rotacionarPlayer = playerRot * Quaternion.Euler(0, -1, 0);
            transform.rotation = rotacionarPlayer;
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 4);
            transform.LookAt(enemy.transform.position);
        }
        if (Input.GetKey(KeyCode.A) && lookEnemy.estaMirando)
        {
            Quaternion playerRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            Quaternion rotacionarPlayer = playerRot * Quaternion.Euler(0, -1, 0);
            transform.rotation = rotacionarPlayer;
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 4);
            transform.LookAt(enemy.transform.position);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("walking", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("walkBack", false);
        }

    }

}
