using UnityEngine;
using TMPro;

// Controla las rondas de 10 segundos, el puntaje y la interfaz.
public class GameManager : MonoBehaviour
{
    [Header("Variables")]
    public static GameManager instance;
    public float roundTime = 10f;
    public int round = 1;
    public int score = 0;
    float timer = 0f;

    [Header("Referencias")]
    public CellSpawner spawner;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        timer = roundTime;
        spawner.SpawnRound();
        UpdateUI();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndRound();
        }

        UpdateUI();
    }

    // Cierra la ronda: las sobrevivientes aprenden, el cerebro se actualiza
    // y arranca la siguiente generacion de celulas.
    void EndRound()
    {
        spawner.EndRound();
        CellBrain.instance.NextRound();

        round++;
        timer = roundTime;

        spawner.SpawnRound();
    }

    public void AddScore()
    {
        score++;
    }

    void UpdateUI()
    {
        int seconds = Mathf.CeilToInt(timer);

        if (seconds < 0)
        {
            seconds = 0;
        }

        if (timeText != null)
        {
            timeText.text = "Tiempo: " + seconds;
        }

        if (scoreText != null)
        {
            scoreText.text = "Eliminadas: " + score;
        }

        if (roundText != null)
        {
            roundText.text = "Ronda: " + round;
        }
    }
}
