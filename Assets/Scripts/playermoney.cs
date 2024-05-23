using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playermoney : MonoBehaviour
{

    public Text uiText;  //플레이어의 현재 돈을 표시하기 위한 txt UI
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("playermoney",100);  // 100으로 초기설정 
    }

    // Update is called once per frame
    void Update()
    {
        int plymoney = PlayerPrefs.GetInt("playermoney");
        UpdateUIText(plymoney);
    }


   private void UpdateUIText(int value)
    {
        if (uiText != null)
        {
            uiText.text = "Money : " + value.ToString() +"$";
        }
    }

}


