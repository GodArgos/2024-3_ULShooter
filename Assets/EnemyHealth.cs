using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    private int lifes = 3;
    

    public Transform target;
    
    private void Update() 
    {
        if(lifes == 0)
        {
            Destroy(transform.parent.gameObject);
            target.GetComponent<PlayerController>().EnemigoEliminado();
        }    
    }
    
    public void TakeDamage()
    {
        lifes -= 1;
        Debug.Log("Lifes left:" + lifes);
        
    }
}
