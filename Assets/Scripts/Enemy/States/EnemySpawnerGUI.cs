using UnityEngine;
using System.Collections; // Asegúrate de incluir esta línea para usar corutinas

public class EnemySpawnerGUI : MonoBehaviour
{
    private bool showGUI = false; // Controla la visibilidad de la GUI
    private string maxEnemiesText; // Texto para el campo de entrada de máximo de enemigos

    public GameObject enemyType1Prefab; // Prefab del primer tipo de enemigo
    public GameObject enemyType2Prefab; // Prefab del segundo tipo de enemigo

    public float spawnInterval = 1f; // Tiempo entre cada spawn
    private bool spawnerActive = false; // Controla si el spawner está activo
    public int maxEnemies = 20; // Máximo número de enemigos a generar
    public int enemiesPerSpawn = 4; // Número de enemigos a generar por cada tipo en cada ciclo

    private void Start()
    {
        // Inicializa el campo de texto con el número máximo de enemigos
        maxEnemiesText = maxEnemies.ToString();
    }

    void Update()
    {
        // Abrir/cerrar la GUI con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            showGUI = !showGUI; // Alternar la visibilidad de la GUI
            if (showGUI)
            {
                ActivateSpawner(); // Activa el spawner al abrir la GUI
            }
        }
        
        // Aquí puedes implementar cualquier lógica adicional necesaria cuando la GUI está activa
    }

    void OnGUI()
    {
        if (showGUI)
        {
            // Panel de Configuración
            GUI.Box(new Rect(10, 10, 250, 150), "Configuración del Spawner");

            // Sección para editar el número máximo de enemigos
            GUI.Label(new Rect(20, 40, 200, 20), "Máximo de Enemigos:");
            maxEnemiesText = GUI.TextField(new Rect(150, 40, 70, 20), maxEnemiesText);
            int.TryParse(maxEnemiesText, out maxEnemies); // Actualiza maxEnemies basado en el input

            // Botón para activar el spawner
            if (GUI.Button(new Rect(20, 70, 200, 30), "Iniciar Spawneo"))
            {
                ActivateSpawner();
            }

            // Botón para detener el spawner
            if (GUI.Button(new Rect(20, 110, 200, 30), "Detener Spawneo"))
            {
                DeactivateSpawner();
            }
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el juego si la GUI no está activa
        }
    }

    // Método para activar el spawner
    public void ActivateSpawner()
    {
        spawnerActive = true;
        StartCoroutine(SpawnEnemies()); // Inicia la corutina para el spawneo
    }

    // Método para desactivar el spawner
    public void DeactivateSpawner()
    {
        spawnerActive = false;
    }

    // Corrutina para spawn de enemigos
    private IEnumerator SpawnEnemies()
    {
        int enemiesSpawned = 0;

        // Mientras el spawner esté activo y no se haya alcanzado el máximo
        while (spawnerActive && enemiesSpawned < maxEnemies)
        {
            // Calcular cuántos enemigos generar de cada tipo
            int remainingEnemies = maxEnemies - enemiesSpawned;
            int enemiesType1 = Random.Range(0, remainingEnemies + 1); // Generar un número aleatorio para el tipo 1
            int enemiesType2 = remainingEnemies - enemiesType1; // El resto será el tipo 2

            // Genera enemigos del tipo 1
            for (int i = 0; i < enemiesType1 && enemiesSpawned < maxEnemies; i++)
            {
                SpawnEnemyAtSpawnerPosition(enemyType1Prefab);
                enemiesSpawned++;
            }

            // Genera enemigos del tipo 2
            for (int i = 0; i < enemiesType2 && enemiesSpawned < maxEnemies; i++)
            {
                SpawnEnemyAtSpawnerPosition(enemyType2Prefab);
                enemiesSpawned++;
            }

            // Espera el tiempo de spawn entre cada ciclo
            yield return new WaitForSeconds(spawnInterval);
        }

        // Desactiva el spawner al finalizar
        spawnerActive = false;
    }

    // Método para instanciar el enemigo en la posición del spawner
    private void SpawnEnemyAtSpawnerPosition(GameObject enemyPrefab)
    {
        // Usa la posición del spawner para instanciar el enemigo
        Vector3 spawnPos = transform.position; // Usa la posición del spawner

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true); // Asegúrate de que el enemigo esté activo
    }
}
