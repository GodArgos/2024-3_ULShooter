using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Agrega esta línea para usar listas

public class EnemySpawnerGUI : MonoBehaviour
{
    private bool showGUI = false; // Controla la visibilidad de la GUI
    private string maxEnemiesText; // Texto para el campo de entrada de máximo de enemigos

    public GameObject enemyType1Prefab; // Prefab del primer tipo de enemigo
    public GameObject enemyType2Prefab; // Prefab del segundo tipo de enemigo

    public float spawnInterval = 1f; // Tiempo entre cada spawn
    private bool spawnerActive = false; // Controla si el spawner está activo
    public int maxEnemies; // Máximo número de enemigos a generar
    public int suma;
    public int ene1;
    public int ene2;
    private List<GameObject> activeEnemies = new List<GameObject>(); // Lista para rastrear enemigos activos
    
    private float hordeInterval = 60f; // Tiempo inicial entre hordas
    private float minHordeInterval = 10f; // Tiempo mínimo entre hordas

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
                // Pausar el juego cuando la GUI está abierta
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Reanudar el juego cuando la GUI está cerrada
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
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
    }

    // Método para activar el spawner
    public void ActivateSpawner()
    {
        DeactivateSpawner(); // Detener el spawner si ya estaba activo
        DestroyExistingEnemies(); // Eliminar enemigos existentes
        spawnerActive = true;
        StartCoroutine(HordeSpawner()); // Inicia el sistema de hordas
    }

    // Método para desactivar el spawner
    public void DeactivateSpawner()
    {
        spawnerActive = false;
    }

    private IEnumerator HordeSpawner()
    {
        while (spawnerActive)
        {
            yield return new WaitForSeconds(hordeInterval);
            

            // Reducir el intervalo entre hordas hasta el límite mínimo
            if (hordeInterval > minHordeInterval)
            {
                Debug.Log("Intervalo reducido"+hordeInterval);
                hordeInterval -= 5f;
                hordeInterval = Mathf.Max(hordeInterval, minHordeInterval);

            }

            if (spawnerActive){
                StartCoroutine(SpawnEnemies());
                Debug.Log("Horda iniciada");
            };
        }
    }

    // Corrutina para spawn de enemigos
    private IEnumerator SpawnEnemies()
{
    // Generar enemigos solo una vez al iniciar el spawner
    int enemiesType1 = Random.Range(1, maxEnemies); // Genera entre 1 y maxEnemies enemigos de tipo 1
    int enemiesType2 = Random.Range(1, maxEnemies); // Genera entre 1 y maxEnemies enemigos de tipo 2
    suma = enemiesType1 + enemiesType2;

    Debug.Log("Enemigos 1: " + enemiesType1);
    Debug.Log("Enemigos 2: " + enemiesType2);

    // Controlar la cantidad total de enemigos generados
    for (int i = 0; i < enemiesType1; i++)
    {
        if (!spawnerActive) break; // Salir si el spawner se desactiva
        GameObject enemy = SpawnEnemyAtSpawnerPosition(enemyType1Prefab);
        activeEnemies.Add(enemy);
        ene1++; // Aumentar el contador de enemigos tipo 1
        yield return new WaitForSeconds(spawnInterval); // Esperar entre spawns
    }

    for (int i = 0; i < enemiesType2; i++)
    {
        if (!spawnerActive) break; // Salir si el spawner se desactiva
        GameObject enemy = SpawnEnemyAtSpawnerPosition(enemyType2Prefab);
        activeEnemies.Add(enemy);
        ene2++; // Aumentar el contador de enemigos tipo 2
        yield return new WaitForSeconds(spawnInterval); // Esperar entre spawns
    }

    // Desactiva el spawner al finalizar
    //spawnerActive = false;
}

    // Método para instanciar el enemigo en la posición del spawner
    private GameObject SpawnEnemyAtSpawnerPosition(GameObject enemyPrefab)
    {
        // Usa la posición del spawner para instanciar el enemigo
        Vector3 spawnPos = transform.position;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true); // Asegúrate de que el enemigo esté activo
        return newEnemy; // Devuelve el enemigo creado
    }

    // Método para destruir enemigos existentes
    private void DestroyExistingEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy); // Destruye el enemigo
        }
        activeEnemies.Clear(); // Limpia la lista
    }
}
