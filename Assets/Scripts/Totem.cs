using UnityEngine;

public class Totem : MonoBehaviour
{
    void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Player"))
        {
            ControladorMision.instancia.SumarTotem();
            Destroy(gameObject); // Desaparece la estatua al recogerla
        }
    }
}
