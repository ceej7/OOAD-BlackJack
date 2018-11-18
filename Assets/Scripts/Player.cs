using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : Involver {

    public int no;
    public bool isConnected;
    public int balance;
    public int jackpot;
    public GameManager gm;
    public PlayerConn myConn;

	// Use this for initialization
	void Start () {
        isStopped = false;
        isConnected = false;
    }
	
	// Update is called once per frame
	void Update () {
        //Promise the balance and jackpot text up-2-date
        //userPanel.updateUserAccount(jackpot, balance);

    }

    public override bool isStop()
    {
        return isStopped;
    }

    public void winGame(float times)
    {
        myConn.RpcwinDisplay(jackpot + (int)(jackpot * times));
        changeAccount(balance + (int)(jackpot * times) + jackpot, 0);
    }
    
    public void loseGame()
    {
        myConn.RpcloseDisplay();
        changeAccount(balance, 0);
    }



    public void addChip(int chip)
    {
        if ((balance - chip) < 0)
            return ;
        changeAccount(balance - chip, jackpot + chip);
        gm.continueGame(no);
    }

    public void bindConnection(PlayerConn conn)
    {
        myConn = conn;
        conn.player = this;
        isConnected = true;
        changeAccount(10000, 0);
    }

    public void changeAccount(int bal,int jack)
    {
        balance = bal;
        jackpot = jack;
        myConn.RpcupdateAccount(bal, jack);
    }

    public void wantAntoherCard()
    {
        myConn.RpcWantAnotherCard();
    }

    public void moreCards(bool more)
    {
        if (more)
        {
            gm.sendAnotherCard(no, hasHowManyCards());
        }
        else
        {
            isStopped = true;
            gm.continueGame(no);
        }
    }
}
