using System.Collections;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyType1Prefab;
    public GameObject enemyType2Prefab;
    public GameObject uiCanvas; 
    public TMP_InputField maxEnemiesInputField;

    public float spawnInterval = 1f;     
    private bool spawnerActive = false;  
    public int maxEnemies;          
    public int enemiesPerSpawn = 1;     

    public float spawnRange = 5f;

    private void Start()
    {
        uiCanvas.SetActive(false);
        maxEnemiesInputField.text = maxEnemies.ToString();
        ActivateSpawner();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleCanvas();
        }

        if (int.TryParse(maxEnemiesInputField.text, out int newMaxEnemies))
        {
            maxEnemies = Mathf.Max(0, newMaxEnemies); 
        }
    }

    public void ActivateSpawner()
    {
        spawnerActive = true;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        int enemiesSpawned = 0;

        while (spawnerActive && enemiesSpawned < maxEnemies)
        {
            int remainingEnemies = maxEnemies - enemiesSpawned;
            int enemiesToSpawnType1 = Random.Range(0, Mathf.Min(enemiesPerSpawn, remainingEnemies) + 1);
            int enemiesToSpawnType2 = remainingEnemies - enemiesToSpawnType1;

            enemiesToSpawnType2 = Mathf.Clamp(enemiesToSpawnType2, 0, enemiesPerSpawn);

            for (int i = 0; i < enemiesToSpawnType1 && enemiesSpawned < maxEnemies; i++)
            {
                SpawnEnemyAtRandomPosition(enemyType1Prefab);
                enemiesSpawned++;
            }

            for (int i = 0; i < enemiesToSpawnType2 && enemiesSpawned < maxEnemies; i++)
            {
                SpawnEnemyAtRandomPosition(enemyType2Prefab);
                enemiesSpawned++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        spawnerActive = false;
    }

    private void SpawnEnemyAtRandomPosition(GameObject enemyPrefab)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
        Vector3 spawnPos = transform.position + randomOffset;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true); 
    }

    private void ToggleCanvas()
    {
        bool isCanvasActive = !uiCanvas.activeSelf;
        uiCanvas.SetActive(isCanvasActive);

        if (isCanvasActive)
        {
            // Pausar el juego
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Reanudar el juego
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
