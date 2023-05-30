using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void LoadAdditiveScene(int level)
    {
        SceneManager.LoadScene(level,LoadSceneMode.Additive);
    }


}