using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dealer : Involver {

    // Use this for initialization
    void Start()
    {
        isStopped = false;
        //Debug.Log("--------------------------(21 - 10) / 10=" + (21 - 10) / 10);
        //Debug.Log("--------------------------(21 - 11) / 10=" + (21 - 11) / 10);
        //Debug.Log("--------------------------(21 - 12) / 10=" + (21 - 12) / 10);
    }


    /// <summary>
    /// ai judge step
    /// </summary>
    /// <returns></returns>
    public override bool isStop()
    {
        if (isStopped) return true;
        else
        {
            int score=0;
            int aceNum=0;
            for (int i =0; i <mycards.Count; i++)
            {
                if(mycards[i].no==1)
                {
                    aceNum++;
                }
                else if(mycards[i].no <=10)
                {
                    score += mycards[i].no;
                }
                else
                {
                    score += 10;
                }
            }
            if(aceNum==0&&score< 17)
            {
                return false;
            }
            else if (aceNum == 0 && score >= 17)
            {
                return true;
            }
            else if (aceNum == 1 && score==10&& mycards.Count==2)
            {
                return true;
            }
            else if (score+aceNum < 17)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
    }
    
    /// <summary>
    /// return 1 dealer win
    /// return 0 55kai
    /// return -1 player win
    /// return -2 player win 1.5 times the jackpot 
    /// </summary>
    /// <param name="playerCards"></param>
    /// <returns></returns>
    public int decideOutcome(List<Card> playerCards)
    {
        int mySum=0;
        int playerSum=0;
        int myCardsNum= mycards.Count;
        int playerCardsNum= playerCards.Count;
        int myAceNum=0;
        int playerAceNum=0;

        for (int i = mycards.Count - 1; i >= 0; i--)
        {
            if(mycards[i].no<10)
                mySum += mycards[i].no;
            else
                mySum += 10;
            if (mycards[i].no ==1)
            {
                myAceNum++;
            }
        }
        for (int i = playerCards.Count - 1; i >= 0; i--)
        {
            if (playerCards[i].no < 10)
                playerSum += playerCards[i].no;
            else
                playerSum += 10;
            if (playerCards[i].no == 1)
            {
                playerAceNum++;
            }
        }
        Debug.Log("mySum:" + mySum + "---" + "playerSum:" + playerSum + "---" + "myCardsNum:" + myCardsNum + "---" + "playerCardsNum:" + playerCardsNum + "---" + "myAceNum:" + myAceNum + "---" + "playerAceNum:" + playerAceNum);
        if (mySum > 21 || playerSum > 21)
        {
            Debug.Log("Exception: logical mistake when decide who wins");
            return 1;
        }
        //five small situation 
        else if (myCardsNum == 5 && playerCardsNum == 5)
        {
            if (mySum < playerSum) return 1;
            else if (mySum > playerSum) return -2;
            else return 1;
        }
        else if (myCardsNum == 5) return 1;
        else if (playerCardsNum == 5) return -2;
        //blackJackSituation
        else if (myCardsNum == 2 && myAceNum == 1 && mySum == 11 && playerCardsNum == 2 && playerAceNum == 1 && playerSum == 11)
        {
            return 1;
        }
        else if (myCardsNum == 2 && myAceNum == 1 && mySum == 11) return 1;
        else if (playerCardsNum == 2 && playerAceNum == 1 && playerSum == 11) return -2;
        //for the rest situation, compare the cards total num, bigger win
        else
        {
            int myN = (21 - mySum) / 10;
            int playerN = (21 - playerSum) / 10;
            myN = myN > myAceNum ? myAceNum : myN;
            playerN = playerN > playerAceNum ? playerAceNum : playerN;

            mySum += myN * 10;
            playerSum += playerN * 10;
            //Debug.Log("mySum:" + mySum + "---" + "playerSum:" + playerSum + "---" + "myCardsNum:" + myCardsNum + "---" + "playerCardsNum:" + playerCardsNum + "---" + "myAceNum:" + myAceNum + "---" + "playerAceNum:" + playerAceNum);
            if (mySum>21||playerSum>21)
            {
                Debug.Log("Exception: logical mistake when adding up to maxSum no more than 21");
                return 0;
            }
            else
            {
                return mySum >= playerSum ? 1 : -1;
            }
        }
    }

    /// <summary>
    /// For dealer, his first card is unreachable to players
    /// When revealing the result
    /// The function gonna reverse it and show the result to everyone
    /// </summary>
    public void revealMyCard()
    {
        cm.revealCard(0, mycards[0].gameObject);
    }
}
