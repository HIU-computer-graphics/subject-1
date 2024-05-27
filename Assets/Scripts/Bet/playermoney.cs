using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playermoney : MonoBehaviour
{
    private DataManager dm;
    public Text uiText;  //플레이어의 현재 돈을 표시하기 위한 txt UI
    public Text playerText;
    public Text bankerText;
    public Text tieText;

    void Start()
    {
        dm = FindObjectOfType<DataManager>();
        dm.PlayerMoney = 100;  // 100으로 초기설정 
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