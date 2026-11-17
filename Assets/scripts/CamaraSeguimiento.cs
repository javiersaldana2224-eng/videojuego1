using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 separacion = new Vector3(0f, 2.3f, -4.5f);
    public float suavizado = 10f;
    public float sensibilidadGiro = 3f;

    private float anguloHorizontal = 0f;

    void LateUpdate()
    {
        if (objetivo == null) return;

        if (Input.GetMouseButton(1))
        {
            anguloHorizontal += Input.GetAxis("Mouse X") * sensibilidadGiro;
        }

        Quaternion rotacion = Quaternion.Euler(0f, anguloHorizontal, 0f);
        Vector3 posicionDeseada = objetivo.position + (rotacion * separacion);

        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        transform.LookAt(objetivo.position + Vector3.up * 1.5f);
    }
}