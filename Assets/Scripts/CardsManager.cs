using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CardsManager : NetworkBehaviour
{

    public GameObject[] cardsStack;
    public List<int> cardsSequence;
    public Transform spawnPos;
    public Transform liftedPos;

    public Transform dealerPosPreset;
    public Transform[] playerPosPreset;

    //Use this for initialization
    void Start () {
        if (!isServer) return;
        reshuffle();
    }

    /// <summary>
    /// To Generate and reshuffle the cards in the cards stack
    /// CardsStack store the static cards prefab
    /// CardsSequence indicates the sequence in the stack 
    /// </summary>
    public void reshuffle()
    {
        if (!isServer) return;
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

    public void revealCard(int gamblerNum,GameObject cardGO)
    {
        //Provide service precisely for the Dealer
        //In that for now, we players do not actually have any hindden cards
        if(gamblerNum==0)
        {
            cardGO.GetComponent<Card>().movetoTarget(searchInTransform(dealerPosPreset, "liftedPos"), searchInTransform(dealerPosPreset, "revealedPos"), true);
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
    public Card sendCard(int gamblerNum,int cardNum)
    { 

        cardNum++;
        if (!isServer) return null;
        switch (gamblerNum)
        {
            case 0:
                {
                    string name=(cardNum==1)? "pos1Hidden" :("pos"+ cardNum );
                    bool toRev = (cardNum == 1) ?false:true;
                    Transform target= searchInTransform(dealerPosPreset, name);
                    if(target==null)
                    {
                        Debug.Log("Send Card to involver " + gamblerNum + " cardNum " + cardNum + " : searching target failed!");
                        return null;
                    }
                    return distributeCard(target, toRev);
                }

            default:
                {
                    if(gamblerNum>3)
                    {
                        Debug.Log("sendCard(): No such Gambler");
                        return null;
                    }
                    string name = (cardNum == 1) ? "pos1Seen" : ("pos" + cardNum );
                    bool toRev = true;
                    Transform target = searchInTransform(playerPosPreset[gamblerNum-1], name);
                    if (target == null)
                    {
                        Debug.Log("Send Card to involver " + gamblerNum + " cardNum " + cardNum + " : searching target failed!");
                        return null;
                    }
                    return distributeCard(target, toRev);
                }
        }
    }

    /// <summary>
    /// On Server End
    /// Trigger behaviour in 3D Scene
    /// instantiate the card on the top of the stack
    /// Distribute To the target position
    /// Meanwhile snyc on all the client end
    /// Wi
    /// </summary>
    public Card distributeCard(Transform target,bool toRev)
    {
        if (cardsSequence.Count == 0) reshuffle();
        int objIndex = cardsSequence[0];
        cardsSequence.RemoveAt(0);

        //Spawn the card on Server
        GameObject go = Instantiate(cardsStack[objIndex], spawnPos.position, spawnPos.rotation);
        //go.transform.parent = spawnPos;
        //Abroad adn sync to the clients
        NetworkServer.Spawn(go);

        //Move to location
        //We have NetworkTransform on the card
        //As a result, the tansform will automatically be sync in the S-C
        go.GetComponent<Card>().movetoTarget(liftedPos, target, toRev);
        return go.GetComponent<Card>();
    }

    public Transform searchInTransform(Transform f, string name)
    {
        if (!isServer) return null;
        foreach (Transform t in f.GetComponentsInChildren<Transform>())
        {
            if (t.name == name)
            {
                return t;
            }
        }
        return null;
    }

    
}
