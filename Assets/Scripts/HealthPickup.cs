using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    
    public int healthAmount = 25; // Cantidad de salud que recupera

    void OnTriggerEnter(Collider other)
    {
       
        // Verifica si el objeto que lo recoge es el jugador
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ha recogido el botiquín"); // Mensaje de depuración
            Destroy(gameObject);
            HudManager playerHealth = other.GetComponent<HudManager>();
            if (playerHealth != null)
            {
                
                playerHealth.RecoverHealth(healthAmount);
                // Destruye el objeto después de recogerlo
            }
            
        }
    }
}
