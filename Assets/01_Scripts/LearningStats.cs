using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Muestra en pantalla lo que el cerebro va aprendiendo.
// Sirve para demostrar que el aprendizaje realmente esta ocurriendo.
public class LearningStats : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshProUGUI statsText;
    public Image bestColorImage;

    void Update()
    {
        if (CellBrain.instance == null)
        {
            return;
        }

        CellBrain brain = CellBrain.instance;

        int bestColor = brain.BestIndex(brain.colorScore);
        int bestSize = brain.BestIndex(brain.sizeScore);

        // Cuadrito que muestra el color que mejor le esta funcionando.
        if (bestColorImage != null)
        {
            bestColorImage.color = brain.GetColor(bestColor);
        }

        if (statsText != null)
        {
            statsText.text =
                "APRENDIZAJE\n" +
                "Exploracion: " + Mathf.Round(brain.explorationRate * 100f) + "%\n" +
                "Mejor color: gen " + bestColor + "\n" +
                "Mejor tamano: " + brain.GetSize(bestSize).ToString("0.00") + "\n" +
                "Ronda anterior: " + brain.lastSurvived + " de " + brain.lastBorn + " sobrevivieron";
        }
    }
}
