using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "SampleScene";   // Nombre exacto de tu escena de juego

    void Start()
    {
        // Por si vuelves al menú desde el juego, libera el mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayCreative()
    {
        GameSettings.CurrentMode = GameMode.Creative;
        SceneManager.LoadScene(gameSceneName);
    }

    public void PlaySurvival()
    {
        GameSettings.CurrentMode = GameMode.Survival;
        SceneManager.LoadScene(gameSceneName);
    }
}