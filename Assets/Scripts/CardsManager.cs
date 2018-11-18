using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour {

    public GameObject[] cardsStack;
    public List<int> cardsSequence;
    public Transform spawnPos;
    public Transform liftedPos;

    public Transform[] playerPosPreset;
    public Transform[] dealerPosPreset;

    //Use this for initialization
    void Start () {
        reshuffle();

        //StartCoroutine(distribute()); //Animation test
    }

    /// <summary>
    /// To Generate and reshuffle the cards in the cards stack
    /// CardsStack store the static cards prefab
    /// CardsSequence indicates the sequence in the stack 
    /// </summary>
    public void reshuffle()
    {
        cardsSequence = new List<int>();
        for (int i = 0; i <= 51; i++)
        {
            cardsSequence.Add(i);
        }

        for (int i = 51; i >= 0; i--)
        {
            int swapNum = Random.Range(0, i);
            int tmp = cardsSequence[swapNum];
            cardsSequence[swapNum] = cardsSequence[i];
            cardsSequence[i] = tmp;
        }
    }

    /// <summary>
    /// Send Card to somebody
    /// gamblerNum=0 ->dealer
    /// gamblerNum>=1 ->player
    /// for extension
    /// </summary>
    /// <param name="gamblerNum"></param>
    /// <param name="cardNum"></param>
    /// <param name="toRev"></param>
    public Card sendCard(int gamblerNum,int cardNum,bool toRev)
    {
        switch(gamblerNum)
        {
            case 0:
                {
                    return distributeCard(dealerPosPreset[cardNum], toRev);
                    //break;
                }
                
            case 1:
                {
                    return distributeCard(playerPosPreset[cardNum], toRev);
                    //break;
                }
            default:
                return null;
        }
    }

    /// <summary>
    /// Trigger behaviour in 3D Scene
    /// Distribute the card on the top of the stack
    /// To the target position
    /// </summary>
    public Card distributeCard(Transform target,bool toRev)
    {
        if (cardsSequence.Count == 0) reshuffle();
        int objIndex = cardsSequence[0];
        cardsSequence.RemoveAt(0);

        //Spawn the card
        GameObject go = Instantiate(cardsStack[objIndex], spawnPos.position, spawnPos.rotation);
        go.transform.parent = spawnPos;
        go.transform.localScale = new Vector3(1, 1, 1);

        //Move to location
        go.GetComponent<Card>().movetoTarget(liftedPos, target, toRev);
        return go.GetComponent<Card>();
    }
}
