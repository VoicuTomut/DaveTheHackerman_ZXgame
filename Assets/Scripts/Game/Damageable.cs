using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth; 
     
    // Start is called before the first frame update
    void Start()
    {
        currentHealth=maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakeDamage()
    {
        currentHealth--;
        return currentHealth <= 0;
    }
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }
}
