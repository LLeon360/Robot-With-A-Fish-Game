using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hotbar Element", menuName = "Hotbar Element Object")]
public class HotbarElementObject : ScriptableObject
{
    [StringInList("Building", "Unit")] 

    public string slotType = "Building";
    public string elementName; //this should be a unique identifier
    public GameObject deployPrefab;
    public int cost;
    public float cooldown;
    public Sprite icon;

}