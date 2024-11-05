using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int lifes = 3;
    private HudManager hudManager;

    private void Start()
    {
        hudManager = FindObjectOfType<HudManager>();
    }

    private void Update() 
    {
        if(lifes == 0)
        {
            hudManager.UpdateKillCount();
            Destroy(transform.parent.gameObject);
        }    
    }

    public void TakeDamage()
    {
        lifes -= 1;
        Debug.Log("Lifes left:" + lifes);
    }
}
