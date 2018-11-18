    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConn : NetworkBehaviour{
    public UserPanel userPanel;
    public Camera cam;
    public GameManager gm;
    public Player player;

    [SyncVar]
    public int connNum=0;

    public bool isReady;
	// Use this for initialization
	void Start () {
        if(isLocalPlayer)
        {
            isReady = false;
            //Debug.Log("PlayerConn is initializing on Client, UI and Cam");
            userPanel.gameObject.SetActive(true);
            cam.gameObject.SetActive(true);
            CmdRegisterInServer();
        }

     
	}

	[ClientRpc]
    public void RpcsetCamera(int i)
    {
        if (!isLocalPlayer) return;
        if (i==1)
        {
            cam.transform.localPosition = new Vector3(0, 18.54f, -10.82f);
            cam.transform.localRotation = Quaternion.Euler(65.453f, 0, 0);
        }
        if (i == 2)
        {
            cam.transform.localPosition = new Vector3(-17.23f, 20.42f, -2.6f);
            cam.transform.localRotation = Quaternion.Euler(65.453f, 54.676f, 4.859f);
        }
        if (i == 3)
        {
            cam.transform.localPosition = new Vector3(16.24f, 20.37f, -3.78f);
            cam.transform.localRotation = Quaternion.Euler(65.453f, -45.373f, 4.859f);
        }

    }


    /// <summary>
    /// From Client2Server
    /// Register myself in Server and bind with player
    /// </summary>
    [Command]
    public void CmdRegisterInServer()
    {
        if (!isServer) return;
        //Debug.Log("PlayerConn-Server is binding with Gamemanager");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gm==null)
        {
            //Debug.Log("PlayerConn-Server cannot find GM");
            return;
        }

        //here should add number exceeding exception  handler
        connNum = gm.registerInGameManager(this);
        Debug.Log("My connNum: " + connNum);
    }


    /// <summary>
    /// Ready BT Handler
    /// then tell the Server that i am ready
    /// From Client2Server
    /// </summary>
    public void ReadyToPlayHandler()
    {
        if (!isLocalPlayer) return;
        //Debug.Log("ReadyToPlayHandler");
        userPanel.disableAllButton();
        CmdReadyToPlay();
    }
    [Command]
    public void CmdReadyToPlay()
    {
        isReady = true;
        
    }

    /// <summary>
    /// Enable to interact with rhe Chip BTs
    /// From Server2Client
    /// </summary>
    [ClientRpc]
    public void RpctoAddChip()
    {
        if (!isLocalPlayer) return;
        userPanel.enableChipButton();
    }

    /// <summary>
    /// Chip BTs Handler 
    /// Then tell how many chip does user push in
    /// From Client2Server
    /// </summary>
    public void AddChipHandler(int chip)
    {
        if (!isLocalPlayer) return;
        CmdAddChip(chip);
        userPanel.disableAllButton();
    }
    [Command]
    public void CmdAddChip(int chip)
    {
        player.addChip(chip);
    }

    /// <summary>
    /// Update the acount info in ui;
    /// From Server2Client 
    /// </summary>
    /// <param name="bal"></param>
    /// <param name="jack"></param>
    [ClientRpc]
    public void RpcupdateAccount(int bal,int jack)
    {
        if (!isLocalPlayer) return;
        userPanel.updateUserAccount(jack,bal);
    }


    /// <summary>
    /// Enable on client more/not button
    /// </summary>
    [ClientRpc]
    public void RpcWantAnotherCard()
    {
        if (!isLocalPlayer) return;
        userPanel.enableCardButton();
    }
    

    /// <summary>
    /// Handler for more and stop BTs
    /// want more cards or just stop getting cards
    /// </summary>
    /// <param name="more"></param>
    public void moreCardsHandler(bool more)
    {
        if (!isLocalPlayer) return;
        CmdmoreCards(more);
        userPanel.disableAllButton();
    }
    [Command]
    public void CmdmoreCards(bool more)
    {
        player.moreCards(more);
    }

    [ClientRpc]
    public void RpcwinDisplay(int i)
    {
        userPanel.winDisplay(i, 5);
    }

    [ClientRpc]
    public void RpcloseDisplay()
    {
        userPanel.loseDisplay(5);
    }

    [ClientRpc]
    public void RpctoWaitForReady()
    {
        isReady = false;
        userPanel.enableReadyButton();
    }
}
