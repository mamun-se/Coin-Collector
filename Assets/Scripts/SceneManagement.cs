using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadGamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
