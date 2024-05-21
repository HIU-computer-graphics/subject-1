using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public UIStates uiStates; // UI 상태 관리자 참조
    [HideInInspector]
    public static float totalBet = 0; // 총 베팅 금액
    [HideInInspector]
    public float totalWinnings = 0; // 총 당첨금
    [HideInInspector]
    public bool endingGame = false; // 게임 종료 여부
    [HideInInspector]
    public float startingBet = 0; // 시작 베팅 금액

    public PlayerHand hand; // 플레이어 핸드 참조
    public BankerHand bankerHand; // 뱅커 핸드 참조
    [HideInInspector]
    public Hand currentHand; // 현재 핸드 참조

    public Hand[] playerHands; // 플레이어 핸드 배열
    [HideInInspector]
    public List<ChipStack> betStacks; // 베팅 칩 스택 리스트
    public bool canDeal = true; // 딜 가능 여부

    private void Start()
    {
        playerHands = new Hand[] { hand, bankerHand }; // 플레이어 핸드 초기화
        betStacks = new List<ChipStack>(); // 베팅 칩 스택 리스트 초기화
    }

    public void AwardWinnings()
    {
        // 주석 처리된 코드: 사이드 베팅에 따른 당첨금 계산

        uiStates.OnStateChange(); // UI 상태 변경 트리거
    }

    public void ClearBet()
    {
        totalBet = 0; // 총 베팅 금액 초기화
        betStacks.Clear(); // 베팅 칩 스택 리스트 초기화
        uiStates.OnStateChange(); // UI 상태 변경 트리거
    }

    public GameObject DealCard(CardData card, int id)
    {
        if (IsPlacingBet())
        {
            startingBet = totalBet; // 시작 베팅 금액 설정
        }

        return playerHands[id].DealCard(card); // 카드 딜
    }

    public void ResetScore()
    {
        hand.ResetScore(); // 플레이어 점수 리셋
        bankerHand.ResetScore(); // 뱅커 점수 리셋
    }

    public void ResetStartBet()
    {
        startingBet = 0; // 시작 베팅 금액 초기화
    }

    public void ResetTable()
    {
        hand.ResetHand(); // 플레이어 핸드 리셋
        bankerHand.ResetHand(); // 뱅커 핸드 리셋

        currentHand = hand; // 현재 핸드를 플레이어 핸드로 설정
        endingGame = false; // 게임 종료 상태 해제
        uiStates.Reset(); // UI 상태 리셋
    }

    public bool IsIdle()
    {
        return hand.currentScore == 0 && bankerHand.currentScore == 0 && totalBet == 0; // 유휴 상태 여부 확인
    }

    public bool IsPlacingBet()
    {
        return totalBet > 0; // 베팅 중인지 여부 확인
    }

    public bool BothHandsEnded()
    {
        return hand.IsEnded() && bankerHand.IsEnded(); // 양쪽 핸드가 종료되었는지 여부 확인
    }

    public bool IsPlayerEnded()
    {
        return hand.IsEnded(); // 플레이어 핸드가 종료되었는지 여부 확인
    }

    public bool IsBankerEnded()
    {
        return bankerHand.IsEnded(); // 뱅커 핸드가 종료되었는지 여부 확인
    }

    public void StandOnPlayertHand()
    {
        hand.Stand(); // 플레이어 스탠드
    }

    public void StandOnBankertHand()
    {
        bankerHand.Stand(); // 뱅커 스탠드
    }

    public void NextHand()
    {
        currentHand = bankerHand; // 다음 핸드를 뱅커 핸드로 설정
    }

    public void ResetHand()
    {
        currentHand = hand; // 현재 핸드를 플레이어 핸드로 리셋
    }
}
