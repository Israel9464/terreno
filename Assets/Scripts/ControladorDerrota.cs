using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorDerrota : MonoBehaviour
{
    public string escenaJuego = "SampleScene";

    void Start()
    {
        // Asegura que el tiempo corra normal y libera el cursor para poder dar clic
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(escenaJuego);
    }
}