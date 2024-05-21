using UnityEngine;
using System.Collections.Generic;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance { get; private set; } // 싱글턴 인스턴스
    public WinSequence winHandler; // 승리 시퀀스 핸들러 참조
    public static bool betsEnabled = true; // 베팅 가능 여부
    public static int roundCount = -1; // 라운드 수
    public static float totalWin = 0; // 총 당첨금

    public delegate void voidEvent(); // void 반환형 delegate
    public static event voidEvent onResult; // 결과 이벤트

    private List<BetSpace> betSpaces = new List<BetSpace>(); // 베팅 공간 리스트
    public Transform chipTrayPosition; // 칩 트레이 위치 참조

    private static int playerResult = 0, bankerResult = 0; // 플레이어 및 뱅커 결과

    private void Awake()
    {
        Instance = this; // 싱글턴 인스턴스 설정
    }

    public void SetResult(int player, int banker)
    {
        totalWin = 0;
        playerResult = player;
        bankerResult = banker;

        foreach (BetSpace betSpace in betSpaces)
        {
            totalWin += betSpace.ResolveBet(); // 베팅 결과 처리
        }
        print("The total win is: " + totalWin);

        BalanceManager.ChangeBalance(totalWin); // 잔액 변경
        winHandler.ShowResult(totalWin); // 결과 표시
        onResult?.Invoke(); // 결과 이벤트 호출
    }

    public static Vector3 GetChipWinPosition()
    {
        return Instance.chipTrayPosition.position; // 칩 트레이 위치 반환
    }

    public static void RegisterBetSpace(BetSpace betSpace)
    {
        Instance.betSpaces.Add(betSpace); // 베팅 공간 등록
    }

    public static bool IsTie()
    {
        return playerResult == bankerResult; // 무승부 여부 확인
    }

    public static bool PlayerWon()
    {
        return playerResult > bankerResult; // 플레이어 승리 여부 확인
    }
}
