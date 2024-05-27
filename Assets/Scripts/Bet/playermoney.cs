using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playermoney : MonoBehaviour
{
    private DataManager dm;
    public Text uiText;  //�÷��̾��� ���� ���� ǥ���ϱ� ���� txt UI
    public Text playerText;
    public Text bankerText;
    public Text tieText;

    void Start()
    {
        dm = FindObjectOfType<DataManager>();
        dm.PlayerMoney = 100;  // 100���� �ʱ⼳�� 
    }

    void Update()
    {
        UpDatingTxt();
    }

    private void UpdateUIText(Text a, float value)
    {
        if (a != null) {
            a.text = "Money : " + value.ToString() + "$";
        }
    }

    private void UpdateGameText(Text a, int value)
    {
        if (a != null) {
            a.text = " " + value.ToString() + "$";
        }
    }

    private void UpDatingTxt()
    {
        if (dm.GetStatus() == GameStatus.Dealing) {
            return;
        }
        UpdateUIText(uiText, dm.PlayerMoney);
        UpdateGameText(playerText, dm.BetAmountOnPlayer);
        UpdateGameText(bankerText, dm.BetAmountOnBanker);
        UpdateGameText(tieText, dm.BetAmountOnTie);
    }
}