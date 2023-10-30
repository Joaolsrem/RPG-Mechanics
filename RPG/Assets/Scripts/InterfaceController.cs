using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    public GameObject inventoryInterface;
    public GameObject menuFood;

    public GameObject menuFornalha;
    public GameObject selecionarComida;
    public GameObject camFornalha;
    public Animator animFornalha;
    public Image imgSelctRuim;
    public Image imgSelctBoa;
    public GameObject botaoEsquentarCmd;
    public GameObject menuSelecionarCmd;
    public Text textTimeFood;
    public PlayerMovement p1;
    public float timeFood;
    public bool selecionouCmd;
    public bool colocouEsquentar;
    bool fecharMenu;
    public Text textSemComida;
    public Transform spawnPlayer;

    public GameObject CameraMain;

    public bool abriuFood;

    public bool abriuMenu;
    public bool fecharInv;

    public bool abriuFornalha;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camFornalha.SetActive(false);
        inventoryInterface.SetActive(false);
        menuFood.SetActive(false);
        menuSelecionarCmd.SetActive(false);
        textSemComida.enabled = false;
        menuFornalha.SetActive(false);
        timeFood = 3f;
    }

    void Update()
    {
        // ABRIR INVENTARIO
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            abriuMenu = !abriuMenu;
            inventoryInterface.SetActive(!abriuMenu);
            fecharInv = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (abriuMenu && !fecharInv)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            fecharInv = true;
        }
        
        // ABRIR MENU COMIDA
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menuFood.SetActive(true);
            abriuFood = true;
        }
        if (Input.GetKeyUp(KeyCode.Z) && abriuFood)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            abriuFood = false;
            menuFood.SetActive(false);
        }

        // ABRIR MENU FORNALHA
        if (Input.GetKeyDown(KeyCode.F) && !p1.encostouBau && p1.encostouFornalha)
        {
            p1.transform.position = spawnPlayer.transform.position;
            p1.transform.rotation = Quaternion.Euler(0, -126.17f, 0);
            p1.speed = 0;
            p1.speedRotation = 0;
            abriuFornalha = !abriuFornalha;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            camFornalha.SetActive(true);
            fecharMenu = false;
            CameraMain.SetActive(false);
            animFornalha.SetBool("animFornalha", true);
            menuFornalha.SetActive(!abriuFornalha);
            selecionarComida.SetActive(true);
            imgSelctBoa.enabled = false;
            textTimeFood.enabled = false;
            imgSelctRuim.enabled = false;
            botaoEsquentarCmd.SetActive(false);
            textTimeFood.enabled = false;
        }

        if (abriuFornalha && !fecharMenu)
        {
            p1.speed = 6;
            p1.speedRotation = 180;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CameraMain.SetActive(true);
            animFornalha.SetBool("animFornalha", false);
            camFornalha.SetActive(false);
            menuSelecionarCmd.SetActive(false);
            fecharMenu = true;
        }

        if (selecionouCmd && colocouEsquentar)
        {
            if (timeFood <= 0)
            {
                p1.comida += 1;
                p1.comidaRuim -= 1;
                textTimeFood.text = "Tempo para acabar: ";
                timeFood = 3f;
            }
            StartCoroutine(EsquentarCmd());
        }
    }

    public void botaoEsquentarCmD()
    {
        colocouEsquentar = true;
    }

    IEnumerator EsquentarCmd()
    {
        timeFood -= Time.deltaTime;
        textTimeFood.text = "Tempo para acabar: " + timeFood.ToString();
        yield return new WaitForSeconds(3f);
        imgSelctRuim.enabled = false;
        textTimeFood.enabled = false;
        menuSelecionarCmd.SetActive(false);
        botaoEsquentarCmd.SetActive(false);
        colocouEsquentar = false;
        selecionouCmd = false;
    }

    public void btescolherComida()
    {
        if (!selecionouCmd)
        {
            menuSelecionarCmd.SetActive(true);
        }
    }

    public void SelecionarCmdRuim()
    {
        if (p1.comidaRuim > 0)
        {
            selecionouCmd = true;
            textTimeFood.enabled = true;
            imgSelctRuim.enabled = true;
            botaoEsquentarCmd.SetActive(true);
        }
        if (p1.comidaRuim <= 0)
        {
            StartCoroutine(semComida());
        }
    }

    IEnumerator semComida()
    {
        textSemComida.enabled = true;
        yield return new WaitForSeconds(1f);
        textSemComida.enabled = false;
    }

}
