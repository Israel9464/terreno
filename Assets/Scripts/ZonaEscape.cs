using UnityEngine;

public class ZonaEscape : MonoBehaviour
{
    void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            ControladorMision.instancia.GanarJuego();
        }
    }
}
