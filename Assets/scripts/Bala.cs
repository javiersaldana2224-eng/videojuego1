using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 30f;
    public float danio = 25f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * velocidad, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Evita que la bala choque contigo mismo al salir del arma
        if (collision.gameObject.GetComponentInParent<ControlJugador>() != null) return;

        // Si golpea al enemigo, le baja vida
        ControlEnemigo enemigo = collision.gameObject.GetComponentInParent<ControlEnemigo>();
        if (enemigo != null)
        {
            enemigo.SendMessage("RecibirDanio", danio, SendMessageOptions.DontRequireReceiver);
        }

        Destroy(gameObject);
    }
}