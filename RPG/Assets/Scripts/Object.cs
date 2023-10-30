using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Inventory Object/Create new")]
public class Object : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
}
