using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI kills;
    [SerializeField] private GameObject crosshair;
    private HudManager hudManager;
    private bool active = false;

    private void Start()
    {
        hudManager = FindObjectOfType<HudManager>();
        panel.SetActive(false);
    }

    private void Update()
    {
        if (hudManager.currentHealth <= 0 && !active)
        {
            hudManager.gameObject.SetActive(false);
            crosshair.SetActive(false);
            panel.SetActive(true);
            active = true;
            SetValues();

            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.R) && active)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void SetValues()
    {
        float totalTime = hudManager.lastTime;
        string mins = ((int)totalTime / 60).ToString("00");
        string segs = (totalTime % 60).ToString("00");
        string TimerString = string.Format("{00}:{01}", mins, segs);

        time.text = TimerString;

        kills.text = hudManager.GetKillCount().ToString();
    }

}
