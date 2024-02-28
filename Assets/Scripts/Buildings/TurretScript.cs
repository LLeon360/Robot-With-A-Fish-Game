using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerscirpt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemytoattack;

    public bool canattack;

    public int attackpower;


    void Start()
    {
        canattack = true ; 
        attackpower = 50;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canattack && enemytoattack != null) {
            StartCoroutine(Attack());
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "enemy")
        {
            if(enemytoattack == null) {
            enemytoattack = col.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject == enemytoattack) {
            enemytoattack = null;

        }
    }

    IEnumerator Attack()
    {
        canattack = false;
        enemytoattack.GetComponent<HealthScript>().Damage(attackpower);
        yield return new WaitForSeconds(0.5f);
        canattack = true;
    }


}
