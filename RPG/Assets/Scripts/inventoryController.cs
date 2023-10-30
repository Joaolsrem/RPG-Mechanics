using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryController : MonoBehaviour
{
    public Object[] slots;
    public Image[] slotImg;
    public int[] slotAmount;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider colisao)
    {
        if (colisao.gameObject.tag == "PorcaoVida")
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null || slots[i].name == colisao.gameObject.GetComponent<ObjectType>().objectType.name)
                {
                    slots[i] = colisao.gameObject.GetComponent<ObjectType>().objectType;
                    slotAmount[i]++;
                    slotImg[i].sprite = slots[i].itemIcon;

                    Destroy(colisao.gameObject);
                    break;
                }
            }
        }
    }
}
