using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public Sprite[] healthSprites;
    public Image healthImage;
    public Text healthText;
    public GameObject can;
    bool invinsible;
    
    // Start is called before the first frame update
    void Start()
    {
        invinsible = false;
        currentHealth = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {

    if (currentHealth <= 0)
        Die();

    if (currentHealth > maxHealth)
        currentHealth = maxHealth;

   // healthImage.sprite = healthSprites[currentHealth];
    }

    public void damagePlayer(int damageAmount)
    {   
        if(invinsible == false)
        {
            Debug.Log("LowerHealth");
            //GetComponentInChildren<Graphic>().GetComponent<Animator>().SetTrigger("Damaged");
            currentHealth = currentHealth - damageAmount;
            if (currentHealth <= 0)
                Die();
            invinsible = true;
            StartCoroutine(invinsibleTimer());
        }

    }

    IEnumerator invinsibleTimer()
    {
        yield return new WaitForSeconds(2f);
        invinsible = false;
    }

    void Die()
    {//restarts the level
     //Application.LoadLevel(Application.loadedLevel);
        transform.position = new Vector2(0f,7f);
        currentHealth = maxHealth;
        SceneManager.LoadScene(3);
        //Debug.Log("player died");
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
