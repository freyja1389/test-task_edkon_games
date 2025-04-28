using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdCon.MiniGameTemplate
{
    public class CustomizationScene : MonoBehaviour
    {
        public void OnLoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
