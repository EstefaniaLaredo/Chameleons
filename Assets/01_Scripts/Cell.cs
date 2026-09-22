using UnityEngine;

// Una celula. Al nacer le pide su color y su tamano al cerebro,
// y al morir o al terminar la ronda le informa como le fue.
public class Cell : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 0.4f;
    public float limitX = 8f;
    public float limitY = 4f;

    int colorIndex;
    int sizeIndex;
    bool reported = false;
    Vector3 direction;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Le pide al cerebro que genes usar en esta ronda.
        colorIndex = CellBrain.instance.ChooseColor();
        sizeIndex = CellBrain.instance.ChooseSize();

        sr.color = CellBrain.instance.GetColor(colorIndex);

        float size = CellBrain.instance.GetSize(sizeIndex);
        transform.localScale = new Vector3(size, size, 1f);

        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    }

    void Update()
    {
        transform.position = transform.position + direction * speed * Time.deltaTime;

        // Rebota al llegar al borde para no salirse de la pantalla.
        if (transform.position.x < -limitX || transform.position.x > limitX)
        {
            direction.x = -direction.x;
        }

        if (transform.position.y < -limitY || transform.position.y > limitY)
        {
            direction.y = -direction.y;
        }
    }

    // El jugador hizo clic encima: la celula muere y reporta el fracaso.
    void OnMouseDown()
    {
        Report(false);
        GameManager.instance.AddScore();
        Destroy(gameObject);
    }

    // Termino la ronda y la celula sigue viva: reporta el exito.
    public void EndRound()
    {
        Report(true);
        Destroy(gameObject);
    }

    void Report(bool alive)
    {
        if (reported)
        {
            return;
        }

        reported = true;
        CellBrain.instance.Learn(colorIndex, sizeIndex, alive);
    }
}
