using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorMision : MonoBehaviour
{
    public static ControladorMision instancia;

    [Header("Configuración")]
    public int totemsNecesarios = 5;
    private int totemsRecogidos = 0;

    [Header("Aparición Aleatoria")]
    public GameObject totemPrefab;          // Arrastra aquí el Prefab de Owl Statue
    public Transform[] puntosAparicion;     // Lista de posibles lugares (pon 10 o 15)

    [Header("Conexiones")]
    public GameObject zonaEscape;
    public Perseguidor enemigo;
    public TextMeshProUGUI textoMision;

    void Awake()
    {
        instancia = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (zonaEscape != null) zonaEscape.SetActive(false);
        GenerarTotemsAlAzar();
        ActualizarTexto();
    }

    void GenerarTotemsAlAzar()
    {
        if (totemPrefab == null || puntosAparicion.Length == 0) return;

        // Copiamos los puntos a una lista temporal para ir tachando los que ya se usaron
        List<Transform> puntosDisponibles = new List<Transform>(puntosAparicion);
        int cantidadAGenerar = Mathf.Min(totemsNecesarios, puntosDisponibles.Count);

        for (int i = 0; i < cantidadAGenerar; i++)
        {
            int indiceAzar = Random.Range(0, puntosDisponibles.Count);
            Transform puntoElegido = puntosDisponibles[indiceAzar];

            Instantiate(totemPrefab, puntoElegido.position, puntoElegido.rotation);

            // Quitamos ese punto de la lista para que no salgan dos tótems en el mismo lugar
            puntosDisponibles.RemoveAt(indiceAzar);
        }
    }

    public void SumarTotem()
    {
        totemsRecogidos++;

        if (enemigo != null)
        {
            enemigo.velocidad += 0.35f;
        }

        if (totemsRecogidos >= totemsNecesarios)
        {
            if (zonaEscape != null) zonaEscape.SetActive(true);
            if (textoMision != null) textoMision.text = "¡Tótems completos! Ve hacia la luz para escapar";
        }
        else
        {
            ActualizarTexto();
        }
    }

    void ActualizarTexto()
    {
        if (textoMision != null)
        {
            textoMision.text = "Tótems: " + totemsRecogidos + " / " + totemsNecesarios;
        }
    }

    public void GanarJuego()
    {
        SceneManager.LoadScene("Victoria");
    }
}