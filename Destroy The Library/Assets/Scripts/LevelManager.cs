using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    /// <summary>
    /// Loads a new scene.
    /// </summary>
    /// <param name="sceneIndex">The scene index of the new scene</param>
    public static void Load(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Loads the scene with the next sceneIndex.
    /// </summary>
    public static void LoadNextScene()
    {
        Load(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }
}
