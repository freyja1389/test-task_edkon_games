using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdCon.MiniGameTemplate
{
    /// <summary>
    /// The main.
    /// </summary>
    public class Main : MonoBehaviour
    {
        public void OnLoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
