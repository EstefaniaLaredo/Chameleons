using UnityEngine;

// CEREBRO COMPARTIDO POR TODAS LAS CELULAS.
// Guarda un puntaje por cada color y por cada tamano posible.
// Si una celula sobrevive, su color y su tamano suben de puntaje.
// Si el jugador la elimina, bajan. Con el tiempo la poblacion
// termina eligiendo los valores que mejor la camuflan.
public class CellBrain : MonoBehaviour
{
    [Header("Variables")]
    public static CellBrain instance;

    [Header("Limites de color")]
    public float minHue = 0f;
    public float maxHue = 1f;
    public float saturation = 0.7f;
    public float brightness = 0.9f;
    public int colorGenes = 12;

    [Header("Limites de tamano")]
    public float minSize = 0.25f;
    public float maxSize = 1f;
    public int sizeGenes = 5;

    [Header("Aprendizaje")]
    public float explorationRate = 0.9f;
    public float explorationMin = 0.1f;
    public float explorationDecay = 0.12f;
    public float reward = 1f;
    public float penalty = 1f;

    [Header("Resultados")]
    public float[] colorScore;
    public float[] sizeScore;
    public int roundBorn = 0;
    public int roundSurvived = 0;
    public int lastBorn = 0;
    public int lastSurvived = 0;

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

        // La memoria arranca vacia: todos los genes valen lo mismo.
        colorScore = new float[colorGenes];
        sizeScore = new float[sizeGenes];
    }

    // DECISION: con probabilidad explorationRate prueba algo nuevo (explorar),
    // si no, repite el que mejor le ha funcionado hasta ahora (explotar).
    public int ChooseColor()
    {
        if (Random.Range(0f, 1f) < explorationRate)
        {
            return Random.Range(0, colorGenes);
        }

        return BestIndex(colorScore);
    }

    public int ChooseSize()
    {
        if (Random.Range(0f, 1f) < explorationRate)
        {
            return Random.Range(0, sizeGenes);
        }

        return BestIndex(sizeScore);
    }

    // Devuelve la posicion del gen con mayor puntaje.
    public int BestIndex(float[] scores)
    {
        int best = 0;

        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > scores[best])
            {
                best = i;
            }
        }

        return best;
    }

    // Convierte el numero del gen en un color real, dentro de los limites.
    public Color GetColor(int index)
    {
        float hue = Mathf.Lerp(minHue, maxHue, (float)index / (colorGenes - 1));
        return Color.HSVToRGB(hue, saturation, brightness);
    }

    // Convierte el numero del gen en un tamano real, dentro de los limites.
    public float GetSize(int index)
    {
        return Mathf.Lerp(minSize, maxSize, (float)index / (sizeGenes - 1));
    }

    // APRENDIZAJE: cada celula reporta una sola vez por ronda.
    public void Learn(int colorIndex, int sizeIndex, bool alive)
    {
        roundBorn++;

        if (alive)
        {
            colorScore[colorIndex] += reward;
            sizeScore[sizeIndex] += reward;
            roundSurvived++;
        }
        else
        {
            colorScore[colorIndex] -= penalty;
            sizeScore[sizeIndex] -= penalty;
        }
    }

    // Al cerrar la ronda se guardan los resultados y se explora un poco menos,
    // porque ya se tiene mas informacion sobre que funciona.
    public void NextRound()
    {
        lastBorn = roundBorn;
        lastSurvived = roundSurvived;
        roundBorn = 0;
        roundSurvived = 0;

        explorationRate = explorationRate - explorationDecay;

        if (explorationRate < explorationMin)
        {
            explorationRate = explorationMin;
        }
    }
}
