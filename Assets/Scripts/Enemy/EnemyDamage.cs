using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private HudManager hudManager; // Referencia al script de HudManager
    private Transform player; // Referencia al jugador
    [SerializeField] private float attackRange = 2.0f; // Distancia mínima para atacar
    [SerializeField] private int damageAmount = 5; // Daño a aplicar al jugador
    [SerializeField] private float attackCooldown = 1.5f; // Cooldown entre ataques

    private bool canAttack = true;

    private void Awake()
    {
        hudManager = FindObjectOfType<HudManager>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(ApplyDamageWithCooldown());
        }
    }

    private IEnumerator ApplyDamageWithCooldown()
    {
        canAttack = false;
        hudManager.ApplyDamage(damageAmount); // Llama a ApplyDamage en HudManager
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
