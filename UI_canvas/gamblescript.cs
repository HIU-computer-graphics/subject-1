using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ������ ������� �޾ƿͼ� �׿����� ���� ���� Ȥ�� ������Ŵ 
public class gamblescript : MonoBehaviour
{

    public int a = 0; // ���� ����� �Լ� 
    // Start is called before the first frame update
    void Start()
    {
        // ���� ���� 0���� �ʱ�ȭ�ϰ� ���� 
        PlayerPrefs.SetInt("player", 0);
        PlayerPrefs.SetInt("banker", 0);
        PlayerPrefs.SetInt("tie", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //������ ����� �޾ƿ� 
        if( a == 0)
        {

        }
        else if( a == 1) // tie
        {
            tiewin(); // ������ �� ��� ���� 
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


    // ��Ŀ tie �÷��̾� �¸��� �������� �Լ� 
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


    // ��ư��ũ��Ʈ�� ���� �� �߰� �Լ� 
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


    // clear �ǵ��� ��� ���� player money�� �߰�  ���ۿ� ������ ���� �����ؾ���

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
