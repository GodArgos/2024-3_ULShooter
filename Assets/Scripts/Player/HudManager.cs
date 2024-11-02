using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxArmor = 100;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Animator faceAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float distributionFactor = 0.2f;
    private Dictionary<int, int> fontIndex;
    public bool test = false;
    private int currentHealth;
    private int currentArmor;

    private void Start()
    {
        fontIndex = new Dictionary<int, int>()
        {
            {0, 1},
            {1, 72},
            {2, 2},
            {3, 3},
            {4, 4},
            {5, 5},
            {6, 6},
            {7, 7},
            {8, 8},
            {9, 9},
        };

        currentHealth = maxHealth;
        currentArmor = maxArmor;

        UpdateStatus(currentHealth, healthText);
        UpdateStatus(currentArmor, armorText);
        UpdateHealthFactor();
    }

    private void Update()
    {
        if (test)
        {
            ApplyDamage(5); // Ejemplo: aplica 20 de daño
            test = false;
        }
    }

    public void ApplyDamage(int totalDamage)
    {
        TriggerDamage();

        if (currentArmor > 0)
        {
            int healthDamage = Mathf.CeilToInt(totalDamage * distributionFactor); // 1/5 del daño total a la vida
            int armorDamage = totalDamage - healthDamage;           // 4/5 del daño total a la armadura

            currentHealth = Mathf.Max(0, currentHealth - healthDamage);
            currentArmor = Mathf.Max(0, currentArmor - armorDamage);
        }
        else
        {
            // Si la armadura es 0, todo el daño va a la vida
            currentHealth = Mathf.Max(0, currentHealth - totalDamage);
        }

        UpdateStatus(currentHealth, healthText);
        UpdateStatus(currentArmor, armorText);
        UpdateHealthFactor(); // Actualizar el factor de salud en el animador
    }

    private void UpdateStatus(int value, TextMeshProUGUI text)
    {
        string valueString = value.ToString();
        List<string> sprites = new List<string>();

        if (valueString.Length < 3)
        {
            sprites.Add("<sprite=71>");
        }
        else
        {
            int hundreds = int.Parse(valueString[0].ToString());
            sprites.Add($"<sprite={fontIndex[hundreds]}>");
        }

        if (valueString.Length < 2)
        {
            sprites.Add("<sprite=71>");
        }
        else
        {
            int tens = int.Parse(valueString[valueString.Length - 2].ToString());
            sprites.Add($"<sprite={fontIndex[tens]}>");
        }

        int units = int.Parse(valueString[valueString.Length - 1].ToString());
        sprites.Add($"<sprite={fontIndex[units]}>");
        sprites.Add("<sprite=10>");

        text.text = string.Join("", sprites);
    }

    private void UpdateHealthFactor()
    {
        float healthFactor = (float)currentHealth / maxHealth;
        faceAnimator.SetFloat("HealthFactor", healthFactor);
    }

    private void TriggerDamage()
    {
        audioSource.PlayOneShot(audioSource.clip);
        faceAnimator.SetFloat("Direction", Random.Range(0, 2));
        faceAnimator.SetTrigger("Damage");
    }
}
