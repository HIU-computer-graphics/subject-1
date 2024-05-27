using System;
using System.Collections.Generic;
using UnityEngine;

// ī�� ���� �����ϴ� enum
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

// BaccaratManager Ŭ����
public class BaccaratManager : MonoBehaviour
{
    private DataManager dm; // DataManager �ν��Ͻ��� ������ ����

    private List<ValueTuple<Card, CardSuit>> deck; // ī�� ��
    private List<ValueTuple<Card, CardSuit>> playerHand; // �÷��̾��� ��
    private List<ValueTuple<Card, CardSuit>> bankerHand; // ��Ŀ�� ��

    public List<ValueTuple<Card, CardSuit>> GetCardDeck() { return deck; }
    public List<ValueTuple<Card, CardSuit>> GetPlayerHand() { return playerHand; }
    public List<ValueTuple<Card, CardSuit>> GetBankerHand() { return bankerHand; }

    void Start()
    {
        // DataManager �ν��Ͻ� ����
        dm = FindObjectOfType<DataManager>();
    }

    // ī�� ���� �ʱ�ȭ�ϴ� �Լ�
    public void InitializeDeck()
    {
        deck = new List<ValueTuple<Card, CardSuit>>();
        // 4���� ��Ʈ (��Ʈ, ���̾Ƹ��, Ŭ��, �����̵�)
        for (int i = 0; i < 4; i++) {
            CardSuit suit = (CardSuit)(i + 1);
            // �� ��Ʈ���� 1���� 13������ ī�� ��
            for (Card j = Card.Ace; j <= Card.King; j++) {
                deck.Add((j, suit));
            }
        }
    }

    // ī�� ���� �����ϴ� �Լ�
    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++) {
            ValueTuple<Card, CardSuit> temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    // �÷��̾�� ��Ŀ���� �ʱ� ī�带 �й��ϴ� �Լ�
    public void DealInitialCards()
    {
        playerHand = new List<ValueTuple<Card, CardSuit>> { deck[0], deck[2] };
        bankerHand = new List<ValueTuple<Card, CardSuit>> { deck[1], deck[3] };
        deck.RemoveRange(0, 4); // �й�� ī�� ������ ����
    }

    // ������ ���� ����ϴ� �Լ� (��ī�� �꿡 ����)
    public int CalculateHandValue(List<ValueTuple<Card, CardSuit>> hand)
    {
        int value = 0;
        foreach (ValueTuple<Card, CardSuit> card in hand) {
            int cardValue = (int)card.Item1;
            if (cardValue > 9)
                value += 0; // �� ī��� 10�� 0���� ���
            else
                value += cardValue;
        }
        return value % 10; // ��ī��� 10���� ���� �������� ���
    }

    // �߰� ī�带 �й��ϴ� �Լ� (��ī�� �꿡 ����)
    public void DealAdditionalCards()
    {
        int playerValue = CalculateHandValue(playerHand);
        int bankerValue = CalculateHandValue(bankerHand);

        // �÷��̾�� ��Ŀ�� ù �� �� ī���� ���� 8 �Ǵ� 9�̸� �߰� ī�� ���� ���� ����
        if (playerValue >= 8 || bankerValue >= 8) {
            return;
        }

        // �÷��̾� �߰� ī�� ��
        if (playerValue <= 5) {
            playerHand.Add(deck[0]);
            deck.RemoveAt(0);
        }

        // ��Ŀ �߰� ī�� ��
        bankerValue = CalculateHandValue(bankerHand); // �÷��̾ ī�� �߰��� �� ��Ŀ �� ����
        if (playerHand.Count == 3) // �÷��̾ �� ��° ī�带 ���� ���
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
        } else // �÷��̾ �� ��° ī�带 ���� ���� ���
          {
            if (bankerValue <= 5) {
                bankerHand.Add(deck[0]);
                deck.RemoveAt(0);
            }
        }
    }

    // �¸��ڸ� �����ϰ� ���� ����� ó���ϴ� �Լ�
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