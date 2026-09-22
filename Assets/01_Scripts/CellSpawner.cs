using UnityEngine;
using System.Collections.Generic;

// Crea las celulas de cada ronda y las destruye cuando la ronda termina.
public class CellSpawner : MonoBehaviour
{
    [Header("Variables")]
    public int cellsPerRound = 12;
    public int maxCells = 20;
    public float spawnRangeX = 7.5f;
    public float spawnRangeY = 3.5f;

    [Header("Referencias")]
    public GameObject cellPrefab;

    public List<Cell> cells = new List<Cell>();

    public void SpawnRound()
    {
        cells.Clear();

        // OPTIMIZACION: nunca se pasa del limite de celulas en pantalla.
        int amount = cellsPerRound;

        if (amount > maxCells)
        {
            amount = maxCells;
        }

        for (int i = 0; i < amount; i++)
        {
            float x = Random.Range(-spawnRangeX, spawnRangeX);
            float y = Random.Range(-spawnRangeY, spawnRangeY);

            Cell c = Instantiate(cellPrefab, new Vector3(x, y, 0f), transform.rotation).GetComponent<Cell>();
            cells.Add(c);
        }
    }

    // Las celulas que siguen vivas reportan que sobrevivieron y desaparecen.
    public void EndRound()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i] != null)
            {
                cells[i].EndRound();
            }
        }

        cells.Clear();
    }
}
