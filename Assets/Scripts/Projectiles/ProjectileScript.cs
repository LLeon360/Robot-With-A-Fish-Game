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

        //set rotation to match direction in 2d
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle > 90 || angle < 90) ? angle-180 : angle+180;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
