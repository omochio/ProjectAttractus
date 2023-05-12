using UnityEngine.SceneManagement;

public class SceneChanger
{
    public static void SceneChange(string sceneName)
    {
        //string loadingSceneName = "LoadingScene";

        //SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
