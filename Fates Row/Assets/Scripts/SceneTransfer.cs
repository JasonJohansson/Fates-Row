using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransfer : MonoBehaviour
{ 
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void transfer()
    {
        int i = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }   
}
