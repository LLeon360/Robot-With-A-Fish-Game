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
    [SerializeField]
    public float lifetime = 30f;
    private float endTime;
    [SerializeField]
    public bool canHitBuildings = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        unitInfo = GetComponent<UnitInfoScript>();
        endTime = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        //move projectile
        rb.velocity = direction * speed;

        //set rotation to match direction in 2d
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle > 90 || angle < 90) ? angle : angle-180;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //destroy if lifetime is up
        if (Time.time > endTime) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<UnitInfoScript>() != null) {
            UnitInfoScript otherUnitInfo = other.gameObject.GetComponent<UnitInfoScript>();
            if(!canHitBuildings && otherUnitInfo.type == "Building") {
                return;
            }
            if (otherUnitInfo.player != player) {
                HealthScript otherHealthScript = other.gameObject.GetComponent<HealthScript>();
                otherHealthScript.Damage(damage, null);
                Destroy(gameObject);
            }
        }
    }
}
