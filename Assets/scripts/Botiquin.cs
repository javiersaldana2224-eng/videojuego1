using UnityEngine;

public class Botiquin : MonoBehaviour
{
    public int curacion = 25;
    public float distanciaRecoger = 2.5f;
    private ControlJugador jugador;
    private Renderer malla;

    void Start()
    {
        jugador = FindFirstObjectByType<ControlJugador>();
        malla = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (jugador == null) return;

        Vector3 centroBotiquin = malla != null ? malla.bounds.center : transform.position;
        float distancia = Vector3.Distance(centroBotiquin, jugador.transform.position);

        if (distancia <= distanciaRecoger && Input.GetKeyDown(KeyCode.E))
        {
            jugador.vida = Mathf.Min(jugador.vida + curacion, 100);
            Debug.Log("Botiquín recogido. Vida actual: " + jugador.vida);
            Destroy(gameObject);
        }
    }
}