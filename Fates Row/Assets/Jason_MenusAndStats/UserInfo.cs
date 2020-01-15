using UnityEngine.UI;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public Text username;
    public Text logout;
    public Button logoutButton;
    // Start is called before the first frame update
    void Start()
    {
        if (AccountManager.isLoggedIn)
        {
            username.text = AccountManager.LoggedInUserName;
            logout.text = "Log Out";
            
        }
        else
        {
            username.text = "";
            logout.text = "";
            logoutButton.interactable = false;
        }
    }

    public void LogOut()
    {
        AccountManager.instance.LogOut();
        Destroy(AccountManager.instance);
    }
}
