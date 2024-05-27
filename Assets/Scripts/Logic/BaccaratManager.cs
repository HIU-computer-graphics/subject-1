using System;
using System.Collections.Generic;
using UnityEngine;

// 카드 값을 정의하는 enum
public enum Card
{
    Ace = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

public enum CardSuit
{
    Hearts = 1,
    Diamonds = 2,
    Clubs = 3,
    Spades = 4
}

// BaccaratManager 클래스
public class BaccaratManager : MonoBehaviour
{
    private DataManager dm; // DataManager 인스턴스를 참조할 변수

    private List<ValueTuple<Card, CardSuit>> deck; // 카드 덱
    private List<ValueTuple<Card, CardSuit>> playerHand; // 플레이어의 패
    private List<ValueTuple<Card, CardSuit>> bankerHand; // 뱅커의 패

    public List<ValueTuple<Card, CardSuit>> GetCardDeck() { return deck; }
    public List<ValueTuple<Card, CardSuit>> GetPlayerHand() { return playerHand; }
    public List<ValueTuple<Card, CardSuit>> GetBankerHand() { return bankerHand; }

    void Start()
    {
        // DataManager 인스턴스 참조
        dm = FindObjectOfType<DataManager>();
    }

    // 카드 덱을 초기화하는 함수
    public void InitializeDeck()
    {
        deck = new List<ValueTuple<Card, CardSuit>>();
        // 4개의 슈트 (하트, 다이아몬드, 클럽, 스페이드)
        for (int i = 0; i < 4; i++) {
            CardSuit suit = (CardSuit)(i + 1);
            // 각 슈트에는 1부터 13까지의 카드 값
            for (Card j = Card.Ace; j <= Card.King; j++) {
                deck.Add((j, suit));
            }
        }
    }

    // 카드 덱을 셔플하는 함수
    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++) {
            ValueTuple<Card, CardSuit> temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // 플레이어와 뱅커에게 초기 카드를 분배하는 함수
    public void DealInitialCards()
    {
        playerHand = new List<ValueTuple<Card, CardSuit>> { deck[0], deck[2] };
        bankerHand = new List<ValueTuple<Card, CardSuit>> { deck[1], deck[3] };
        deck.RemoveRange(0, 4); // 분배된 카드 덱에서 제거
    }

    // 손패의 값을 계산하는 함수 (바카라 룰에 따라)
    public int CalculateHandValue(List<ValueTuple<Card, CardSuit>> hand)
    {
        int value = 0;
        foreach (ValueTuple<Card, CardSuit> card in hand) {
            int cardValue = (int)card.Item1;
            if (cardValue > 9)
                value += 0; // 얼굴 카드와 10은 0으로 계산
            else
                value += cardValue;
        }
        return value % 10; // 바카라는 10으로 나눈 나머지를 사용
    }

    // 추가 카드를 분배하는 함수 (바카라 룰에 따라)
    public void DealAdditionalCards()
    {
        int playerValue = CalculateHandValue(playerHand);
        int bankerValue = CalculateHandValue(bankerHand);

        // 플레이어와 뱅커의 첫 두 장 카드의 합이 8 또는 9이면 추가 카드 없이 승패 결정
        if (playerValue >= 8 || bankerValue >= 8) {
            return;
        }

        // 플레이어 추가 카드 룰
        if (playerValue <= 5) {
            playerHand.Add(deck[0]);
            deck.RemoveAt(0);
        }

        // 뱅커 추가 카드 룰
        bankerValue = CalculateHandValue(bankerHand); // 플레이어가 카드 추가한 후 뱅커 값 재계산
        if (playerHand.Count == 3) // 플레이어가 세 번째 카드를 받은 경우
        {
            int playerThirdCardValue = (int)playerHand[2].Item1;
            if (bankerValue <= 2) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            } else if (bankerValue == 3 && playerThirdCardValue != 8) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            } else if (bankerValue == 4 && playerThirdCardValue >= 2 && playerThirdCardValue <= 7) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            } else if (bankerValue == 5 && playerThirdCardValue >= 4 && playerThirdCardValue <= 7) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            } else if (bankerValue == 6 && playerThirdCardValue >= 6 && playerThirdCardValue <= 7) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            }
        } else // 플레이어가 세 번째 카드를 받지 않은 경우
          {
            if (bankerValue <= 5) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            }
        }
    }

    // 승리자를 결정하고 베팅 결과를 처리하는 함수
    public Winner DetermineWinner()
    {
        int playerValue = CalculateHandValue(playerHand);
        int bankerValue = CalculateHandValue(bankerHand);

        Winner ret;
        if (playerValue > bankerValue) {
            ret = Winner.Player;
            dm.PlayerMoney += (dm.BetAmountOnPlayer * 2f);
        } else if (playerValue < bankerValue) {
            ret = Winner.Banker;
            dm.PlayerMoney += (dm.BetAmountOnBanker * 1.95f);
        } else {
            ret = Winner.Tie;
            dm.PlayerMoney += (dm.BetAmountOnTie * 8f);
        }

        return ret;
    }
}

public enum Winner
{
    Player = 1,
    Banker = 2,
    Tie = 3
}