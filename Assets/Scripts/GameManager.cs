using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    

    //Involver
    public Player[] players;
    public CardsManager cardsManager;
    public Dealer dealer;
    public UserPanel userPanel;

    public int step = 0;//Indicating the GameStep;
    public bool inStep =false;

    // Use this for initialization
    void Start () {
        continueGame(0);
    }
	
	// Update is called once per frame
	void Update () {
        if(inStep)
        {
            return;
        }
        switch(step)
        {
            case 1:
                {
                    //玩家选择筹码
                    inStep = true;
                    //Debug.Log("enableChipButton();");
                    userPanel.enableChipButton();
                    break;
                }
            case 2:
                {
                    //第一轮发牌，blind+seen = 2 player+dealer
                    inStep = true;
                    StartCoroutine(firstTwoRoundProcessing());
                    break;
                }
            case 3:
                {
                    generalRoundProcessing(1);
                    break;
                }
            case 4:
                {
                    generalRoundProcessing(0);
                    break;
                }
            case 5:
                {
                    generalRoundProcessing(1);
                    break;
                }
            case 6:
                {
                    generalRoundProcessing(0);
                    break;
                }
            case 7:
                {
                    generalRoundProcessing(1);
                    break;
                }
            case 8:
                {
                    generalRoundProcessing(0);
                    break;
                }
            case -1:
                {
                    inStep = true;
                    dealer.revealMyCard();
                    if (players[0].isOver21()) players[0].loseGame();
                    else if(dealer.isOver21()) players[0].winGame(1.0f);
                    else Debug.Log("Fatal error: Over 21 Function");
                    StartCoroutine(resetGame());
                    break;
                }

            default:
                {
                    inStep = true;
                    dealer.revealMyCard();
                    int res=dealer.decideOutcome(players[0].mycards);
                    if (res == 1) players[0].loseGame();
                    else if (res == -1) players[0].winGame(1.0f);
                    else if (res == -2) players[0].winGame(1.5f);
                    else Debug.Log("Fatal Error: DecideOutcome Funtion");
                    StartCoroutine(resetGame());
                    break;
                }
        }
	}

    /// <summary>
    /// The first two rounds to distribute cards for both
    /// </summary>
    /// <returns></returns>
    IEnumerator firstTwoRoundProcessing()
    {
        // 模拟初始化
        Card card1=cardsManager.sendCard(1, 0,true);
        players[0].receieveCard(card1);
        yield return new WaitForSeconds(2);
        Card card2 = cardsManager.sendCard(1, 1, true);
        players[0].receieveCard(card2);
        yield return new WaitForSeconds(2);
        Card card3 = cardsManager.sendCard(0, 0, false);
        dealer.receieveCard(card3);
        yield return new WaitForSeconds(1);
        Card card4 = cardsManager.sendCard(0, 1, true);
        dealer.receieveCard(card4);
        yield return new WaitForSeconds(2);
        continueGame(0);
    }

    /// <summary>
    /// The last three rounds to distribute cards for both
    /// </summary>
    /// <param name="who"></param>
    void generalRoundProcessing(int who)
    {
        Involver inv;
        if (who == 0)
            inv = dealer;
        else
        {
            inv = players[who - 1];
        }
        inStep = true;
        if (inv.isStop())
        {
            continueGame(who);
        }
        else
        {
            if(who==0)
            {
                StartCoroutine(distributeRoundRemain(0, dealer.hasHowManyCards()));
            }
            else
            {
                userPanel.enableCardButton();
            }
        }
    }

    /// <summary>
    /// The remaining round to disttibute cards for player
    /// </summary>
    /// <param name="who"></param>
    /// <param name="no"></param>
    /// <returns></returns>
    IEnumerator distributeRoundRemain(int who,int no)
    {
        // 模拟初始化
        userPanel.disableAllButton();
        Card card1 = cardsManager.sendCard(who, no, true);
        if(who==0)
        {
            dealer.receieveCard(card1);
        }
        else
            players[0].receieveCard(card1);
        yield return new WaitForSeconds(2);
        continueGame(who);
    }

    /// <summary>
    /// Use to reset a new game
    /// </summary>
    /// <returns></returns>
    IEnumerator resetGame()
    {
        //Judge Game here
        //Win or lose
        //Give player jackpot
        yield return new WaitForSeconds(5);
        players[0].discardAllCards();
        dealer.discardAllCards();
        inStep = false;
        step = 0;
        continueGame(0);
    }

    /// <summary>
    /// to push on the game in logic
    /// use who to exam ove21 situation 
    /// </summary>
    /// <param name="who"></param>
    public void continueGame(int who)
    {
        //cards sum is over 21, step in special case -1
        Involver me = who == 0 ? (Involver)dealer : (Involver)players[who - 1];
        if (me.isOver21())
        {
            userPanel.disableAllButton();
            step=-1;
            inStep = false;
            return;
        }
        //normal situation  
        userPanel.disableAllButton();
        step++;
        inStep = false;
    }

    /// <summary>
    /// Locgical handler when player presses more/nomore button
    /// </summary>
    /// <param name="go"></param>
    public void wantMoreCardHandler(bool go)
    {
        if (go)
        {
            StartCoroutine(distributeRoundRemain(1, players[0].hasHowManyCards()));
        }
        else
        {
            players[0].toStop();
            continueGame(1);
        }
    }

    /// <summary>
    /// Locgical handler when player presses addchip(?) button
    /// </summary>
    /// <param name="chip"></param>
    public void addChipHandler(int chip)
    {
        if (players[0].addChip(chip))
            continueGame(1);
        else
        {
            userPanel.balanceNotEnough();
        }
    }
}
