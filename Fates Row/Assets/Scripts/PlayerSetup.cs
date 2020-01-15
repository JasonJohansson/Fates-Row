using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    public GameObject PlayerUi;
    private GameObject Can;
    private Camera Cam;

    [SerializeField]
    private GameObject Attack;
    [SerializeField]
    private GameObject Jump;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else {
            Can = Instantiate(PlayerUi);
            DontDestroyOnLoad(Can);
            GetComponent<PlayerUnit>().joy = Can.GetComponentInChildren<FixedJoystick>();
            Can.GetComponentInChildren<Button>().onClick.AddListener(GetComponent<PlayerUnit>().isJumping);
            Attack = Can.transform.Find("AttackButton").gameObject;
            Attack.GetComponent<Button>().onClick.AddListener(GetComponent<PlayerAttack>().performAttack);
            NewScene();
        }
}

    private void OnDestroy()
    {
        if (isLocalPlayer) {
            Destroy(Can);
            if(Cam != null)
                 Cam.enabled = true;
        }

    }

    public void NewScene() {
        Cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        Cam.enabled = false;
    }
    public GameObject GetCanvas()
    {
        return Can;
    }
}
