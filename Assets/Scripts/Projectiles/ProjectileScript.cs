using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private UnitInfoScript unitInfo;

    [SerializeField]
    public Vector3 direction;
    [SerializeField]
    public int player;
    [SerializeField]
    public int damage;
    [SerializeField]
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        unitInfo = GetComponent<UnitInfoScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //move projectile
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<UnitInfoScript>() != null) {
            UnitInfoScript otherUnitInfo = other.gameObject.GetComponent<UnitInfoScript>();
            if (otherUnitInfo.player != player) {
                HealthScript otherHealthScript = other.gameObject.GetComponent<HealthScript>();
                otherHealthScript.Damage(damage, null);
                Destroy(gameObject);
            }
        }
    }
}
