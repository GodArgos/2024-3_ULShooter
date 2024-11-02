using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de importar el espacio de nombres para UI
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyType1Prefab; // Prefab del primer tipo de enemigo
    public GameObject enemyType2Prefab; // Prefab del segundo tipo de enemigo
    public GameObject uiCanvas; // Referencia al Canvas que quieres mostrar/ocultar
    public TMP_InputField maxEnemiesInputField; // Referencia al InputField para el número máximo de enemigos

    public float spawnInterval = 1f;     // Tiempo entre cada spawn
    private bool spawnerActive = false;  // Controla si el spawner está activo
    public int maxEnemies;          // Máximo número de enemigos a generar
    public int enemiesPerSpawn = 1;      // Número de enemigos a generar por cada tipo en cada ciclo

    // Rango de posición para spawnear enemigos (ajusta estos valores según tu escenario)
    public float spawnRange = 5f; // Rango de distancia alrededor del spawner

    private void Start()
    {
        // Desactivar el Canvas al inicio
        uiCanvas.SetActive(false);
        
        // Establecer el valor inicial del InputField
        maxEnemiesInputField.text = maxEnemies.ToString();

        // Inicia el spawner de inmediato para pruebas
        ActivateSpawner();
    }

    private void Update()
    {
        // Escuchar si se presiona la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleCanvas(); // Llama a la función para mostrar/ocultar el Canvas
        }

        // Actualiza el máximo de enemigos basado en el valor del InputField
        if (int.TryParse(maxEnemiesInputField.text, out int newMaxEnemies))
        {
            maxEnemies = Mathf.Max(0, newMaxEnemies); // Asegura que maxEnemies no sea negativo
        }
    }

    // Método para activar el spawner
    public void ActivateSpawner()
    {
        spawnerActive = true;
        StartCoroutine(SpawnEnemies());
    }

    // Corrutina para spawn de enemigos
    private IEnumerator SpawnEnemies()
    {
        int enemiesSpawned = 0;

        // Mientras el spawner esté activo y no se haya alcanzado el máximo
        while (spawnerActive && enemiesSpawned < maxEnemies)
        {
            // Calcula cuántos enemigos de cada tipo spawnear
            int remainingEnemies = maxEnemies - enemiesSpawned;
            int enemiesToSpawnType1 = Random.Range(0, Mathf.Min(enemiesPerSpawn, remainingEnemies) + 1);
            int enemiesToSpawnType2 = remainingEnemies - enemiesToSpawnType1;

            // Asegúrate de que no superen el máximo de enemigos
            enemiesToSpawnType2 = Mathf.Clamp(enemiesToSpawnType2, 0, enemiesPerSpawn);

            // Genera enemigos del tipo 1
            for (int i = 0; i < enemiesToSpawnType1 && enemiesSpawned < maxEnemies; i++)
            {
                SpawnEnemyAtRandomPosition(enemyType1Prefab);
                enemiesSpawned++;
            }

            // Genera enemigos del tipo 2
            for (int i = 0; i < enemiesToSpawnType2 && enemiesSpawned < maxEnemies; i++)
            {
                SpawnEnemyAtRandomPosition(enemyType2Prefab);
                enemiesSpawned++;
            }

            // Espera el tiempo de spawn entre cada ciclo
            yield return new WaitForSeconds(spawnInterval);
        }

        // Desactiva el spawner al finalizar
        spawnerActive = false;
    }

    // Método para instanciar el enemigo en una posición aleatoria
    private void SpawnEnemyAtRandomPosition(GameObject enemyPrefab)
    {
        // Generar una posición aleatoria alrededor del spawner
        Vector3 randomOffset = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
        Vector3 spawnPos = transform.position + randomOffset; // Calcula la posición de spawn

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        
        // Asegúrate de que el enemigo esté activo
        newEnemy.SetActive(true); // Asegúrate de que el enemigo esté activo
    }

    // Método para alternar la visibilidad del Canvas
    private void ToggleCanvas()
    {
        bool isCanvasActive = !uiCanvas.activeSelf;
        uiCanvas.SetActive(isCanvasActive);

        // Configura el cursor para habilitar el uso de la UI
        if (isCanvasActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
