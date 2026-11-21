using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaVictoria : MonoBehaviour
{
    void Start()
    {
        // Libera el mouse que dejó bloqueado el FirstPersonController
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("SampleScene");
    }
}