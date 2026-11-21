using UnityEngine;

public class EfectoMiedo : MonoBehaviour
{
    public Transform enemigo;          // Arrastra aquí al perseguidor
    public float distanciaSusto = 15f; // Distancia a la que empieza el efecto
    public float distanciaMinima = 2f; // Distancia máxima de peligro

    [Header("Configuración de Cámara")]
    public float visionNormal = 60f;   // FOV normal (o tamaño de cámara en 2D)
    public float visionCerca = 30f;    // FOV cerrado cuando está pegado a ti

    private Camera camara;

    void Start()
    {
        camara = GetComponent<Camera>();
        
        // Activa niebla ambiental si es un bosque 3D para darle más atmósfera
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
    }

    void Update()
    {
        if (enemigo == null) return;

        // Calculamos qué tan cerca está el enemigo
        float distancia = Vector3.Distance(transform.position, enemigo.position);

        // Convertimos esa distancia en un valor de 0 (lejos) a 1 (muy cerca)
        float nivelMiedo = 1f - Mathf.InverseLerp(distanciaMinima, distanciaSusto, distancia);

        // Reducimos el campo de visión suavemente (funciona en cámaras 3D y 2D)
        if (camara.orthographic)
        {
            camara.orthographicSize = Mathf.Lerp(visionNormal, visionCerca, nivelMiedo);
        }
        else
        {
            camara.fieldOfView = Mathf.Lerp(visionNormal, visionCerca, nivelMiedo);
        }

        // Aumentamos la densidad de la niebla conforme se acerca (de 0.02 a 0.25)
        RenderSettings.fogDensity = Mathf.Lerp(0.02f, 0.25f, nivelMiedo);
    }
}
