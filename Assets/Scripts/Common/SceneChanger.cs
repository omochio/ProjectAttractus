using UnityEngine.SceneManagement;

public class SceneChanger
{
    public static void SceneChange(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
}
