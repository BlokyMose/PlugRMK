using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Search;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Game Controller")]
    public class GameController : MonoBehaviour
    {
        [SerializeField, SearchContext("t:scene")]
        Object scene;

        [SerializeField]
        string sceneName;

        public void LoadScene(string sceneName = "")
        {
            SceneManager.LoadScene(string.IsNullOrEmpty(sceneName) ? this.sceneName : sceneName);
        }
        
        public void Quit()
        {
            Debug.Log("Quitted");
            Application.Quit();
        }

        void OnValidate()
        {
            if (scene != null)
                sceneName = scene.name;
        }

    }
}
