using UnityEngine;
using UnityEngine.SceneManagement;

public class ARMUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadUIAdditive()
    {
        if (SceneManager.GetSceneByName("UILayer").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("UIlayer", LoadSceneMode.Additive);
        } else
        {
            SceneManager.UnloadSceneAsync("UILayer");
        }
    }
    public void OffloadUI()
    {
        SceneManager.UnloadSceneAsync(2, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    
}
