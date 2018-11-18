using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserPanel : MonoBehaviour
{
    public Button bt5;
    public Button bt10;
    public Button bt20;
    public Button bt50;
    public Button bt100;
    public Button btMore;
    public Button btStop;

    public Text balaceText;
    public Text jackpotText;


    public Text centralText;
    void Start()
    {

        balaceText.gameObject.SetActive(true);
        jackpotText.gameObject.SetActive(true);
        centralText.gameObject.SetActive(false);
    }

    public void enableChipButton()
    {
        StartCoroutine(enableChipBT(0.3f));
    }
    public void enableCardButton()
    {
        btMore.gameObject.SetActive(true);
        btStop.gameObject.SetActive(true);
    }
    public void disableAllButton()
    {
        bt5.gameObject.SetActive(false);
        bt10.gameObject.SetActive(false);
        bt20.gameObject.SetActive(false);
        bt50.gameObject.SetActive(false);
        bt100.gameObject.SetActive(false);
        btMore.gameObject.SetActive(false);
        btStop.gameObject.SetActive(false);
    }

    public void updateUserAccount(int jackpot, int balance)
    {
        jackpotText.text = "Jackpot: $" + jackpot;
        balaceText.text = "Balance: $" + balance;
    }

    public void winDisplay(int m, int dpSeconds)
    {
        centralText.text = "You won $" + m;
        StartCoroutine(showAndDisappear(centralText.gameObject, dpSeconds));
    }

    public void loseDisplay(int dpSeconds)
    {
        centralText.text = "You lost your deal";
        StartCoroutine(showAndDisappear(centralText.gameObject, dpSeconds));
    }

    public void balanceNotEnough()
    {
        centralText.text = "You do not have enough balance";
        StartCoroutine(showAndDisappear(centralText.gameObject, 4));
    }

    IEnumerator showAndDisappear(GameObject go, int second)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(second);
        go.SetActive(false);
    }
    IEnumerator enableChipBT(float interval)
    {
        bt5.gameObject.SetActive(true);
        yield return new WaitForSeconds(interval);
        bt10.gameObject.SetActive(true);
        yield return new WaitForSeconds(interval);
        bt20.gameObject.SetActive(true);
        yield return new WaitForSeconds(interval);
        bt50.gameObject.SetActive(true);
        yield return new WaitForSeconds(interval);
        bt100.gameObject.SetActive(true);
    }
}