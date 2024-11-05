using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    private int lifes = 3;
    

    private PlayerController playerController;
    

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update() 
    {
        if(lifes == 0)
        {
            Destroy(gameObject);
            playerController.EnemigoEliminado();
            
        }    
    }
    
    public void TakeDamage()
    {
        lifes -= 1;
        Debug.Log("Lifes left:" + lifes);
        
    }
}
