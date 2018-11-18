using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : Involver {
    public int balance;
    public int jackpot;
    public GameManager gm;
    public UserPanel userPanel;

	// Use this for initialization
	void Start () {
        balance = 10000;
        jackpot = 0;
        isStopped = false ;
    }
	
	// Update is called once per frame
	void Update () {
        //Promise the balance and jackpot text up-2-date
        userPanel.updateUserAccount(jackpot, balance);

    }

    public override bool isStop()
    {
        return isStopped;
    }

    public bool addChip(int chip)
    {
        if ((balance - chip) < 0)
            return false;
        balance = balance - chip;
        jackpot = jackpot + chip;
        return true;
        
    }

    public void winGame(float times)
    {
        userPanel.winDisplay(jackpot + (int)(jackpot * times),5);
        balance += (int)(jackpot * times)+jackpot;
        jackpot = 0;
    }
    
    public void loseGame()
    {
        userPanel.loseDisplay(5);
        jackpot = 0;
    }

}
