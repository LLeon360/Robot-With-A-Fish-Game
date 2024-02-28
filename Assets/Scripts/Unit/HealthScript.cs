using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;

    public Vector2 targetpos;
    public delegate void DeathAction(GameObject deadObject);
    public delegate void DamageAction(GameObject damagedObject, int damage);

    public event DeathAction OnDeath;
    public event DamageAction OnDamage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //default damage flash
        OnDamage = (GameObject damagedObject, int damage) =>
        {
            FlashWhite();
        };

        //default death action is destroy
        OnDeath = (GameObject deadObject) =>
        {
            Destroy(deadObject);
        };
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        if(OnDamage != null)
        {
            OnDamage(gameObject, damage);
        }
        if(currentHealth <= 0)
        {
            OnDeath(gameObject);
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        //clamp to maxhealth
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        this.currentHealth = currentHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private void OnGUI()
    {
        targetpos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Box(new Rect(targetpos.x - 25, Screen.height - targetpos.y - 40, 60, 20), currentHealth.ToString() + '/' + maxHealth.ToString());
    }

    public void FlashWhite()
    {
        StartCoroutine(FlashWhiteCoroutine());
    }

    private IEnumerator FlashWhiteCoroutine()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        // Change all sprites to use the flashWhiteMaterial
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material.SetFloat("_FlashAmount", 1);
        }

        yield return new WaitForSeconds(0.1f); // wait for 0.1 seconds

        // Change back to original materials
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].material.SetFloat("_FlashAmount", 0);
        }
    }
}
