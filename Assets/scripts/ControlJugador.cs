using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControlJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadCaminar = 3.5f;
    public float velocidadCorrer = 6.5f;
    public float velocidadRotacion = 12f;
    public float fuerzaSalto = 7.5f;
    public float gravedad = -20f;

    [Header("Combate y Salud")]
    public int vida = 100;
    public int danioGolpe = 25;
    public float alcanceGolpe = 2.2f;
    public GameObject balaPrefab;
    public Transform puntoDisparo;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocidadVertical;
    private bool estaMuerto = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (controller.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

        if (estaMuerto) return;
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(balaPrefab, puntoDisparo.position, transform.rotation);
        }

        Mover();
        AccionesExtra();
    }

    void Mover()
    {
        float h = Input.GetAxis("Horizontal"); // A y D
        float v = Input.GetAxis("Vertical");   // W y S

        Transform cam = Camera.main.transform;
        Vector3 adelante = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 derecha = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;
        Vector3 direccion = (adelante * v + derecha * h).normalized;

        bool corriendo = Input.GetKey(KeyCode.LeftShift) && direccion.magnitude > 0.1f;
        float velocidadActual = corriendo ? velocidadCorrer : velocidadCaminar;

        if (direccion.magnitude >= 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
            controller.Move(direccion * velocidadActual * Time.deltaTime);
        }

        animator.SetFloat("Velocidad", direccion.magnitude);
        animator.SetBool("Corriendo", corriendo);
    }

    void AccionesExtra()
    {
        // 1. Saltar (Barra espaciadora)
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocidadVertical.y = fuerzaSalto;
            animator.SetTrigger("Saltar");
        }

        // 2. Rodar / Esquivar (Tecla C)
        if (Input.GetKeyDown(KeyCode.C) && controller.isGrounded)
        {
            animator.SetTrigger("Rodar");
            controller.Move(transform.forward * 1.5f);
        }

        // 3. Golpear / Cross Punch (Clic izquierdo o tecla F)
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Atacar");
            GolpearEnemigo();
        }

        // 4. Presionar botón / Interactuar (Tecla E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Interactuar");
            Debug.Log("Jugador presionó botón / interactuó");
        }

        // 5. Morir manualmente para probar (Tecla K)
        if (Input.GetKeyDown(KeyCode.K))
        {
            Morir();
        }
    }

    void GolpearEnemigo()
    {
        ControlEnemigo enemigo = FindFirstObjectByType<ControlEnemigo>();
        if (enemigo != null)
        {
            float distancia = Vector3.Distance(transform.position, enemigo.transform.position);
            if (distancia <= alcanceGolpe)
            {
                enemigo.RecibirDanio(danioGolpe);
            }
        }
    }

    public void RecibirDanio(int cantidad)
    {
        if (estaMuerto) return;
        vida -= cantidad;
        Debug.Log("Vida del Jugador: " + vida);

        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;
        animator.SetFloat("Velocidad", 0f);
        animator.SetTrigger("Morir");
    }
}