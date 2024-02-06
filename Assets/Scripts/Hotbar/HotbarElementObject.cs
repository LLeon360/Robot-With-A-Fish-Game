using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hotbar Element", menuName = "Hotbar")]
public class HotbarElementObject : ScriptableObject
{
    [StringInList("Building", "Unit")] 

    public string slotType = "Building";
    public GameObject deployPrefab;
    public int cost;
    public float cooldown;

    public Sprite icon;

}