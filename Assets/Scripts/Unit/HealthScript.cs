using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    private int currentHealth;
    // public delegate void DeathAction(GameObject deadObject);
    public delegate void DamageAction(GameObject damagedObject, int damage, GameObject attacker);

    [SerializeField]
    // public event DeathAction OnDeath;
    public UnityEvent<GameObject> OnDeath;
    [SerializeField]
    public event DamageAction OnDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //default damage flash
        OnDamage = (GameObject damagedObject, int damage, GameObject attacker) =>
        {
            FlashWhite();
        };

        //default death action is destroy
        OnDeath.AddListener((GameObject deadObject) =>
        {
            Destroy(deadObject);
        });
    }

    public void Damage(int damage, GameObject attacker)
    {
        currentHealth -= damage;
        if(OnDamage != null)
        {
            OnDamage(gameObject, damage, attacker);
        }
        if(currentHealth <= 0)
        {
            OnDeath.Invoke(gameObject);
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
        //draw health bar
        Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Box(new Rect(targetPos.x - 25, Screen.height - targetPos.y - 40, 60, 20), currentHealth.ToString() + '/' + maxHealth.ToString());
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
