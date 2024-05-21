using UnityEngine;
using DG.Tweening;
using TMPro;

public class BalanceManager : MonoBehaviour
{
    [SerializeField]
    private float balance = 0; // 초기 잔액
    public TMP_Text balanceText; // 잔액을 표시할 텍스트 객체
    private static BalanceManager _Instance; // BalanceManager의 정적 인스턴스

    void Start()
    {
        _Instance = this; // 인스턴스 할당
        balanceText.text = balance.ToString("F2"); // 잔액 텍스트 설정
    }

    public static void ChangeBalance(float value)
    {
        if (_Instance == null)
        {
            Debug.LogError("BalanceManager instance is null!");
            return;
        }

        _Instance.balance += value; // 잔액 업데이트
        _Instance.balanceText.text = _Instance.balance.ToString("F2"); // 잔액 텍스트 업데이트
    }

    public void SetBalanceAt(float value)
    {
        DOTween.To(() => balance, x => balance = x, value, 1).SetEase(Ease.InOutSine).OnUpdate(() =>
        {
            balanceText.text = balance.ToString("0"); // 애니메이션과 함께 잔액 텍스트 업데이트
        });
    }

    public static double GetBalance()
    {
        if (_Instance == null)
        {
            Debug.LogError("BalanceManager instance is null!");
            return 0;
        }

        return _Instance.balance; // 현재 잔액 반환
    }
}
