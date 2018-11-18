using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class GameManager : NetworkBehaviour {

    //Involver
    public Dealer dealer;
    public Player[] players;
    public CardsManager cardsManager;

    //GameStep Issues
    public int step;
    public bool inStep;

    // Use this for initialization
    void Start() {
        if (!isServer) return;
        step = 0;
        inStep = false;
    }

    // Update is called once per frame
    void Update() {
        if (!isServer) return;
        if (inStep)
        {
            return;
        }
       
       switch(step)
        {
            case 0:
                {
                    //Wait for ready
                    int inNum = 0;
                    int readyNum=0;

                    for(int i=0;i<players.Length;i++)
                    {
                        if(players[i].isConnected)
                        {
                            inNum++;
                        }
                        if(players[i].isConnected&&players[i].myConn.isReady)
                        {
                            readyNum++;
                        }
                    }
                    //Debug.Log("inNum:"+ inNum+ " readyNum:"+readyNum);
                    if(readyNum==inNum&&inNum!=0)
                    {
                        continueGame(0);
                    }
                    break;
                }
            case 1:
                {
                    inStep = true;
                    if (players[0].isConnected && players[0].myConn.isReady)
                    {
                        players[0].myConn.RpctoAddChip();
                    }
                    else
                    {
                        continueGame(1);
                    }
                    break;
                }
            case 2:
                {
                    inStep = true;
                    if (players[1].isConnected && players[1].myConn.isReady)
                    {
                        players[1].myConn.RpctoAddChip();
                    }
                    else
                    {
                        continueGame(2);
                    }
                    break;
                }
            case 3:
                {
                    inStep = true;
                    if (players[2].isConnected && players[2].myConn.isReady)
                    {
                        players[2].myConn.RpctoAddChip();
                    }
                    else
                    {
                        continueGame(3);
                    }
                    break;
                }
            case 4:
                {
                    inStep = true;
                    StartCoroutine(firstTwoRoundProcessing());
                    break;
                }
            case 5:
                {
                    if (players[0].isConnected && players[0].myConn.isReady)
                        generalRoundProcessing(1);
                    else
                        step++;
                    break;
                }
            case 6:
                {
                    if (players[1].isConnected && players[1].myConn.isReady)
                        generalRoundProcessing(2);
                    else
                        step++;
                    break;
                }
            case 7:
                {
                    if (players[2].isConnected && players[2].myConn.isReady)
                        generalRoundProcessing(3);
                    else
                        step++;
                    break;
                }
            case 8:
                {
                    generalRoundProcessing(0);
                    break;
                }
            case 9:
                {
                    if (players[0].isConnected && players[0].myConn.isReady)
                        generalRoundProcessing(1);
                    else
                        step++;
                    break;
                }
            case 10:
                {
                    if (players[1].isConnected && players[1].myConn.isReady)
                        generalRoundProcessing(2);
                    else
                        step++;
                    break;
                }
            case 11:
                {
                    if (players[2].isConnected && players[2].myConn.isReady)
                        generalRoundProcessing(3);
                    else
                        step++;
                    break;
                }
            case 12:
                {
                    generalRoundProcessing(0);
                    break;
                }
            case 13:
                {
                    if (players[0].isConnected && players[0].myConn.isReady)
                        generalRoundProcessing(1);
                    else
                        step++;
                    break;
                }
            case 14:
                {
                    if (players[1].isConnected && players[1].myConn.isReady)
                        generalRoundProcessing(2);
                    else
                        step++;
                    break;
                }
            case 15:
                {
                    if (players[2].isConnected && players[2].myConn.isReady)
                        generalRoundProcessing(3);
                    else
                        step++;
                    break;
                }
            case 16:
                {
                    generalRoundProcessing(0);
                    break;
                }
            case -1:
                {
                    inStep = true;
                    dealer.revealMyCard();
                    StartCoroutine(resetGame());
                    break;
                }
            case -2:
                {
                    //庄家自爆
                    inStep = true;
                    dealer.revealMyCard();
                    for (int i = 0; i < 3; i++)
                    {
                        if ((players[i].isConnected && players[i].myConn.isReady&&!players[i].isOver21()))
                        {
                            players[i].winGame(1.0f);
                        }
                    }
                    StartCoroutine(resetGame());
                    break;
                }
            default:
                {
                    //正常判断结果，排除爆掉的人
                    inStep = true;
                    dealer.revealMyCard();
                    for(int i=0;i<3;i++)
                    {
                        if((players[i].isConnected && players[i].myConn.isReady)&&!players[i].isOver21())
                        {
                            int res = dealer.decideOutcome(players[i].mycards);
                            if (res == 1) players[i].loseGame();
                            else if (res == -1) players[i].winGame(1.0f);
                            else if (res == -2) players[i].winGame(1.5f);
                            else Debug.Log("Fatal Error: DecideOutcome Funtion");
                        }
                    }
                    StartCoroutine(resetGame());
                    break;
                }

        }



        
    }

    /// <summary>
    /// make a player registered in this game
    /// Indeed, only after this function is called
    /// can the player attend this game online
    /// </summary>
    /// <returns></returns>
    public int registerInGameManager(PlayerConn me){
        if (!isServer) return 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isConnected)
            {
                players[i].bindConnection(me);
                players[i].myConn.RpctoWaitForReady();
                me.RpcsetCamera(i + 1);
                return i + 1;
            }
        }
        return 0;
    }



    /// <summary>
    /// The first two rounds to distribute cards for both
    /// </summary>
    /// <returns></returns>
    IEnumerator firstTwoRoundProcessing()
    {
        //模拟初始化
        for(int i=0;i<4;i++)
        {
            if(i<3)
            {
                if((players[i].isConnected && players[i].myConn.isReady))
                {
                    Card card = cardsManager.sendCard(i + 1, 0);
                    players[i].receieveCard(card);
                     yield return new WaitForSeconds(2);
                }
                

            }
            else
            {
                Card card = cardsManager.sendCard(0, 0);
                dealer.receieveCard(card);
                yield return new WaitForSeconds(2);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (i < 3)
            {
                if ((players[i].isConnected && players[i].myConn.isReady))
                {
                    Card card = cardsManager.sendCard(i + 1,1);
                    players[i].receieveCard(card);
                    yield return new WaitForSeconds(2);
                }


            }
            else
            {
                Card card = cardsManager.sendCard(0, 1);
                dealer.receieveCard(card);
                yield return new WaitForSeconds(2);
            }
        }
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
            if (who == 0)
            {
                StartCoroutine(distributeRoundRemain(0, dealer.hasHowManyCards()));
            }
            else
            {
                players[who - 1].wantAntoherCard();
            }
        }
    }

    /// <summary>
    /// The remaining round to disttibute cards for player
    /// </summary>
    /// <param name="who"></param>
    /// <param name="no"></param>
    /// <returns></returns>
    public IEnumerator distributeRoundRemain(int who, int no)
    {
        Card card = cardsManager.sendCard(who, no);
        if (who == 0)
        {
            dealer.receieveCard(card);
        }
        else
            players[who-1].receieveCard(card);
        yield return new WaitForSeconds(2);
        continueGame(who);
    }
    public void sendAnotherCard(int who, int no)
    {
        StartCoroutine(distributeRoundRemain(who, no));
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
        for(int i=0;i<3;i++)
        {
            players[i].discardAllCards();
            if(players[i].isConnected && players[i].myConn.isReady)
            {
                players[i].myConn.RpctoWaitForReady();
            }

        }
        dealer.discardAllCards();
        inStep = false;
        step = 0;
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
            me.isStopped = true;
            if(who==0)
            {
                step = -2;
                inStep = false;
                return;
            }
            else
            {
                ((Player)me).loseGame();
            }
        }


        bool isAllOver21 = true;
        for(int i=0;i<3;i++)
        {
            if(players[i].isConnected&& players[i].myConn.isReady&& !players[i].isOver21())
            {
                isAllOver21 = false;
                break;
            }
        }
        if   (isAllOver21)
        {
            step = -1;
            inStep = false;
            return;
        }

        step++;
        inStep = false;
    }



}
