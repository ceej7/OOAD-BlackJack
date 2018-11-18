using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Involver : MonoBehaviour {
    public bool isStopped; //need more cards or not
    public List<Card> mycards; // my cards list

    //For cards[0] lifting and reversing
    public Transform pos1;
    public Transform revealPos1;
    public Transform liftPos1;

    /// <summary>
    /// return the step if stopped
    /// </summary>
    /// <returns></returns>
    public abstract bool isStop();


    /// <summary>
    /// Abstrct decide to stop
    /// </summary>
    public void toStop()
    {
        isStopped = true;
    }

    public int hasHowManyCards()
    {
        return mycards.Count;
    }

    public bool isOver21()
    {
        int sum = 0;
        for (int i = mycards.Count - 1; i >= 0; i--)
        {
            if(mycards[i].no<= 10)
                sum += mycards[i].no;
            else
            {
                sum += 10;
            }
        }
        Debug.Log(this.ToString()+" Logging: isOver21 Function: " + sum);
        if(sum>21)
        {
            return true;
        }
        return false;
    }
 
    /// <summary>
    /// Get a new Card
    /// </summary>
    /// <param name="myCard"></param>
    public void receieveCard(Card myCard)
    {
        if (mycards.Count == 5) return;
        mycards.Add(myCard);
    }


    public void discardAllCards()
    {
        while (mycards.Count != 0)
        {
            Card card = mycards[0];
            card.gameObject.SetActive(false);
            mycards.RemoveAt(0);
        }
        isStopped = false;
    }

}
