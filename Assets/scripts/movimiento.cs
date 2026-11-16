using UnityEngine;

public class movimiento : MonoBehaviour
{
    public GameObject jugador;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ROTACIONES

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 nuevaPosicion = jugador.transform.position;
            nuevaPosicion.y = 0.5f;
            jugador.transform.Rotate(0, nuevaPosicion.y, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 nuevaPosicion = jugador.transform.position;
            nuevaPosicion.y = 0.5f;
            jugador.transform.Rotate(0, -nuevaPosicion.y, 0);
        }
        //TRASLADARSE

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 nuevaPosicion = jugador.transform.position;
            nuevaPosicion.z = 0.03f;
            jugador.transform.Translate(0, 0, nuevaPosicion.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 nuevaPosicion = jugador.transform.position;
            nuevaPosicion.z = 0.03f;
            jugador.transform.Translate(0, 0, -nuevaPosicion.z);
        }
    }
}
