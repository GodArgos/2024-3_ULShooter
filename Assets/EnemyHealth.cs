using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int lifes = 3;

    private void Update() 
    {
        if(lifes == 0)
        {
            Destroy(gameObject);
        }    
    }

    public void TakeDamage()
    {
        lifes -= 1;
        Debug.Log("Lifes left:" + lifes);
    }
}
