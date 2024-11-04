using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform m_FirePoint;
    [SerializeField]
    private float m_FireRange = 10f;
    [SerializeField]
    private CharacterController m_CharacterController;
    [SerializeField]
    private float m_Speed = 5.0f;
    [SerializeField]
    private float m_JumpHeight = 2.0f; 
    [SerializeField]
    private GameObject m_FireSphere;

    private Vector3 m_PlayerVelocity = Vector3.zero;

    private bool m_IsGrounded;

    private bool m_OnJump = false;

    public float tiempoActual=0.0f;
    public TMP_Text contadortext;

    public int muertesContador=0;
    public TMP_Text muertestext;

    public GameObject endScreen;

    public TMP_Text screenText; 
    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void EnemigoEliminado(){
        muertesContador+=1;
        muertestext.text=muertesContador.ToString()+" enemigos";
    }
    public void mostrarResumen(){
            int minutes = Mathf.FloorToInt(tiempoActual / 60);
            int seconds = Mathf.FloorToInt(tiempoActual % 60);
            endScreen.SetActive(true);
            screenText.text = "Tiempo de juego: "+minutes.ToString() + " min "+ seconds.ToString() + " sec " +
            "\nEnemigos eliminados: "+muertesContador.ToString();;
       
        
    }
    void Update()
    {
        
        if(Input.GetButtonDown("Fire1")){
            mostrarResumen();
        }
        tiempoActual+=Time.deltaTime;
        ActualizarContador();
        
    }

    void ActualizarContador()
    {
        contadortext.text=Mathf.FloorToInt(tiempoActual).ToString() + " seg";
    }

    public void Move(Vector2 movement)
    {
        m_IsGrounded = m_CharacterController.isGrounded;

        if (m_IsGrounded && m_PlayerVelocity.y < 0)
        {
            m_PlayerVelocity.y = 0f;

        }

        // Calcular el movimiento plano XZ
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = move * m_Speed;

        // Calculamos la nueva velocidad en Y
        if (m_OnJump &&  m_IsGrounded)
        {
            m_PlayerVelocity.y += Mathf.Sqrt(
                -2f * Physics.gravity.y * m_JumpHeight
            );
            m_OnJump = false;
        }

        // Muevo el objeto en referencia al forward de Camara
        move = Camera.main.transform.forward * move.z + 
            Camera.main.transform.right * move.x;
        move.y = 0f;

        // Rotacion
        var angles = Quaternion.RotateTowards(
            transform.rotation,
            Camera.main.transform.rotation,
            300f * Time.deltaTime
        ).eulerAngles;
        angles.x = 0f; // bloqueamos la rotacion
        angles.z = 0f; // bloqueamos la rotacion

        transform.rotation = Quaternion.Euler(angles);

        m_PlayerVelocity.y += Physics.gravity.y * Time.deltaTime;
        move = new Vector3(
            move.x * Time.deltaTime,
            m_PlayerVelocity.y * Time.deltaTime,
            move.z * Time.deltaTime
        );

        m_CharacterController.Move(move);
    }

    public void Jump()
    {
        m_OnJump = true;
    }

    public void Fire()
    {
        // 1. Lanzar un raycast
        RaycastHit hit;
        var collision = Physics.Raycast(
            m_FirePoint.position,
            Camera.main.transform.forward,
            out hit,
            m_FireRange
        );
        if (collision)
        {
            // 2. Donde colisione el raycast, ejecutar un sistema de particulas
            Instantiate(m_FireSphere, hit.point, Quaternion.identity);
            
        }

        
    }
}
