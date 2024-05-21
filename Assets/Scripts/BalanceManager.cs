using UnityEngine;
using TMPro;

public class BalanceManager : MonoBehaviour {

    [SerializeField]
    private float balance = 0;
    public TMP_Text balanceText;

    private static BalanceManager _Instance;

    void Start()
    {
        _Instance = this;
        balanceText.text = balance.ToString("F2");
    }

    public static void ChangeBalance(float value)
    {
        _Instance.balance += value;
        _Instance.balanceText.text = _Instance.balance.ToString("F2");
    }

    public void SetBalanceAt(float value)
    {
        //DOTween.To(() => balance, x => balance = x, value, 1).SetEase(Ease.InOutSine).OnUpdate(() => { balanceText.text = balance.ToString("0"); });
    }

    public static double GetBalance()
    {
        return _Instance.balance;
    }

}
