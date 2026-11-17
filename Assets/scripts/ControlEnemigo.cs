using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    public int vida = 100;
    public float velocidad = 2.8f;
    public float rangoDeteccion = 12f;
    public float distanciaAtaque = 1.9f;
    public float tiempoEntreAtaques = 2f;
    public int danioAtaque = 15;

    private CharacterController controller;
    private Animator animator;
    private ControlJugador jugador;
    private Vector3 velocidadVertical;
    private float temporizadorAtaque;
    private bool estaMuerto = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        jugador = FindFirstObjectByType<ControlJugador>();
    }

    void Update()
    {
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
        velocidadVertical.y += -20f * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

        if (estaMuerto || jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.transform.position);

        if (distancia <= rangoDeteccion && distancia > distanciaAtaque)
        {
            Vector3 direccion = (jugador.transform.position - transform.position);
            direccion.y = 0;
            direccion.Normalize();

            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, 8f * Time.deltaTime);

            controller.Move(direccion * velocidad * Time.deltaTime);
            animator.SetFloat("Velocidad", 1f);
        }
        else if (distancia <= distanciaAtaque)
        {
            animator.SetFloat("Velocidad", 0f);
            temporizadorAtaque += Time.deltaTime;

            if (temporizadorAtaque >= tiempoEntreAtaques)
            {
                temporizadorAtaque = 0f;
                animator.SetTrigger("Atacar");
                jugador.RecibirDanio(danioAtaque);
            }
        }
        else
        {
            animator.SetFloat("Velocidad", 0f);
        }
    }

    public void RecibirDanio(int cantidad)
    {
        if (estaMuerto) return;
        vida -= cantidad;
        Debug.Log("Vida del Enemigo: " + vida);

        if (vida <= 0)
        {
            estaMuerto = true;
            animator.SetFloat("Velocidad", 0f);
            animator.SetTrigger("Morir");
        }
    }
}