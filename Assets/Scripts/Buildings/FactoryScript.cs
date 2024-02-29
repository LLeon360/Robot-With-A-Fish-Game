using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitInfoScript))]
public class FactoryScript : MonoBehaviour
{
    [SerializeField]
    private float productionCooldown;
    private float nextProductionTime;
    [SerializeField]
    private int productionAmount;
    private UnitInfoScript unitInfoScript;

    // Start is called before the first frame update
    void Start()
    {
        unitInfoScript = GetComponent<UnitInfoScript>();
        nextProductionTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextProductionTime)
        {
            nextProductionTime = Time.time + productionCooldown;
            ResourceManager.Instance.AddMoney(unitInfoScript.player, productionAmount);
        }
    }
}
