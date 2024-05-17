using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 게임의 결과값을 받아와서 그에따라 돈을 지불 혹은 삭제시킴 
public class gamblescript : MonoBehaviour
{

    public int a = 0; // 게임 결과값 함수 
    // Start is called before the first frame update
    void Start()
    {
        // 각각 돈을 0으로 초기화하고 시작 
        PlayerPrefs.SetInt("player", 0);
        PlayerPrefs.SetInt("banker", 0);
        PlayerPrefs.SetInt("tie", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //게임의 결과를 받아옴 
        if( a == 0)
        {

        }
        else if( a == 1) // tie
        {
            tiewin(); // 돈지급 후 모두 비우기 
            PlayerPrefs.SetInt("player", 0);
            PlayerPrefs.SetInt("banker", 0);
            PlayerPrefs.SetInt("tie", 0);
            a = 0;
        }
        else if ( a == 2) // player win
        {
            Playerwin();
            PlayerPrefs.SetInt("player", 0);
            PlayerPrefs.SetInt("banker", 0);
            PlayerPrefs.SetInt("tie", 0);
            a = 0;
        }   
        else if(a == 3) // banker win
        {
            bankerwin();
            PlayerPrefs.SetInt("player", 0);
            PlayerPrefs.SetInt("banker", 0);
            PlayerPrefs.SetInt("tie", 0);
            a = 0;
        }
    }


    // 뱅커 tie 플레이어 승리시 수당지급 함수 
    private void bankerwin()
    {
        int money, plymoney;
        money =PlayerPrefs.GetInt("banker");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", plymoney+money*2);
    }

    private void Playerwin()
    {
        int money, plymoney;
        money = PlayerPrefs.GetInt("player");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", plymoney + money * 2);
    }

    private void tiewin()
    {
        int money, plymoney;
        money = PlayerPrefs.GetInt("tie");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", plymoney + money * 8);
    }


    // 버튼스크립트를 위한 돈 추가 함수 
    public void addtie()
    {
        int money, plymoney, addmoney;
         money =    PlayerPrefs.GetInt("chip");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", plymoney - money);
        addmoney = PlayerPrefs.GetInt("tie");
        PlayerPrefs.SetInt("tie", addmoney + money);
    }
    
    public void addPlayer() {
        int money, plymoney, addmoney;
        money = PlayerPrefs.GetInt("chip");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", plymoney - money);
        addmoney = PlayerPrefs.GetInt("player");
        PlayerPrefs.SetInt("player", addmoney + money);
    }

    public void addbanker()
    {
        int money, plymoney, addmoney;
        money = PlayerPrefs.GetInt("chip");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", plymoney - money);
        addmoney = PlayerPrefs.GetInt("banker");
        PlayerPrefs.SetInt("banker", addmoney + money);
    }


    // clear 건돈을 모두 뺀후 player money에 추가  동작에 문제가 있음 수정해야함

    public void ClearMoney()
    {
        int clear1 , clear2 ,clear3 , plymoney;

        clear1 = PlayerPrefs.GetInt("banker");
        clear2 = PlayerPrefs.GetInt("player");
        clear3 = PlayerPrefs.GetInt("tie");
        plymoney = PlayerPrefs.GetInt("playermoney");
        PlayerPrefs.SetInt("playermoney", (plymoney +clear1+clear2+clear3));
        PlayerPrefs.SetInt("player", 0);
        PlayerPrefs.SetInt("banker", 0);
        PlayerPrefs.SetInt("tie", 0);
    }
}
