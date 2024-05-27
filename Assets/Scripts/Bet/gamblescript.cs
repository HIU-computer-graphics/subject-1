using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamblescript : MonoBehaviour
{
    private DataManager dm;
    public GameObject Restart;

    public GameObject[] objectsToInactive;

    void Start()
    {
        Restart.SetActive(false);
        dm = FindObjectOfType<DataManager>();
    }

    void Update()
    {
        Restart_Script();
    }

    // 버튼스크립트를 위한 돈 추가 함수 
    public void addtie()
    {
        if (dm.GetStatus() == GameStatus.Dealing)
            return;

        int money = dm.Chip;
        dm.BetAmountOnTie += money;
        dm.PlayerMoney -= money;
        if (dm.PlayerMoney < 0) {
            dm.BetAmountOnTie -= money;
            dm.PlayerMoney += money;

        }
    }

    public void addPlayer()
    {
        if (dm.GetStatus() == GameStatus.Dealing)
            return;

        int money = dm.Chip;
        dm.BetAmountOnPlayer += money;
        dm.PlayerMoney -= money;
        if (dm.PlayerMoney < 0) {
            dm.BetAmountOnPlayer -= money;
            dm.PlayerMoney += money;

        }
    }

    public void addbanker()
    {
        if (dm.GetStatus() == GameStatus.Dealing)
            return;

        int money = dm.Chip;
        dm.BetAmountOnBanker += money;
        dm.PlayerMoney -= money;
        if (dm.PlayerMoney < 0) {
            dm.BetAmountOnBanker -= money;
            dm.PlayerMoney += money;

        }
    }

    // clear 건돈을 모두 뺀후 player money에 추가 
    public void ClearMoney()
    {
        if (dm.GetStatus() == GameStatus.Dealing)
            return;

        dm.PlayerMoney += dm.BetAmountOnBanker;
        dm.PlayerMoney += dm.BetAmountOnPlayer;
        dm.PlayerMoney += dm.BetAmountOnTie;

        dm.BetAmountOnBanker = 0;
        dm.BetAmountOnPlayer = 0;
        dm.BetAmountOnTie = 0;
    }

    public void BetTurnEnd()
    {
        if (dm.GetStatus() == GameStatus.Dealing)
            return;
        dm.SetStatus(GameStatus.Dealing);
        for (int i = 0; i < objectsToInactive.Length; i++) {
            objectsToInactive[i].SetActive(false);
        }
    }

    public void BetTurnStart()
    {
        for (int i = 0; i < objectsToInactive.Length; i++) {
            objectsToInactive[i].SetActive(true);
        }
    }

    public void Restart_Script()
    {

        if (dm.GetStatus() == GameStatus.Dealing)
            return;

        if (dm.PlayerMoney == 0 && dm.BetAmountOnPlayer == 0 && dm.BetAmountOnBanker == 0 && dm.BetAmountOnTie == 0) {
            Restart.SetActive(true);
        }
    }

    public void Restart_Button()
    {
        if (dm.GetStatus() == GameStatus.Dealing)
            return;
        dm.PlayerMoney = 100;
        Restart.SetActive(false);
    }
}