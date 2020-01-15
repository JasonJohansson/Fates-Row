using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class campfire : MonoBehaviour
{
	public string level;

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D Colider)
	{
        Debug.Log("plz");
        if (Colider.gameObject.tag == "Player")
        {
            GameObject.Find("UI Overlay").SendMessage("FinishTimer"); //Jason

            Debug.Log("collision detected");
            // Application.LoadLevel(level);
            SceneManager.LoadScene(level);
        }
	}
}