using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CardManager : MonoBehaviour
{
    public float delay;
    public BaccaratManager baccaratManager;
    public DataManager dataManager;

    public float cardThickness = 0.001f;
    
    public GameObject cards;

    public Transform playerCardTransform;
    public Transform bankerCardTransform;

    public GameObject playerWin;
    public GameObject bankerWin;
    public GameObject tieWin;

    private GameObject[] clubs;
    private GameObject[] diamonds;
    private GameObject[] hearts;
    private GameObject[] spades;

    private Vector3 camMoveAmount = Vector3.zero;
    void Awake()
    {
        if (delay == 0) {
            delay = 1.0f;
        }
        if (baccaratManager == null) {
            baccaratManager = FindObjectOfType<BaccaratManager>();
        }
        if (dataManager == null) {
            dataManager = FindObjectOfType<DataManager>();
        }

        clubs = new GameObject[13];
        diamonds = new GameObject[13];
        hearts = new GameObject[13];
        spades = new GameObject[13];

        cards.transform.SetParent(transform);
        GameObject[] deck = null;
        for (int i = 0; i < cards.transform.childCount; i++) {
            var child = cards.transform.GetChild(i).gameObject;
            if (child.name.Contains("diamonds")) {
                deck = diamonds;
            } else if (child.name.Contains("hearts")) {
                deck = hearts;
            } else if (child.name.Contains("clubs")) {
                deck = clubs;
            } else if (child.name.Contains("spades")) {
                deck = spades;
            } else {
                continue;
            }

            for (int j = 0; j < child.transform.childCount; j++) {
                var cchild = child.transform.GetChild(j).gameObject;
                var n = cchild.name.Substring(cchild.name.Length - 2);
                int num;
                if (!int.TryParse(n, out num)) {
                    continue;
                }
                deck[num - 1] = cchild;
            }
        }

        baccaratManager.InitializeDeck();
        var cardDeck = baccaratManager.GetCardDeck();
        SetCard(cardDeck);
    }

    public void GameStart()
    {
        StartCoroutine(GameProcess());
    }

    private void SetCard(List<ValueTuple<Card, CardSuit>> cardList)
    {
        GameObject[] deck = null;
        for (int i = 0; i < cardList.Count; i++) {
            var card = cardList[i];
            GetCardDeck(card.Item2, out deck);
            var cardObject = deck[(int)card.Item1 - 1];
            cardObject.transform.SetParent(cards.transform);
            cardObject.transform.localPosition = new Vector3(0, i * cardThickness, 0);
            cardObject.transform.rotation = Quaternion.Euler(180, 0, 0);
            //cardObject.transform.Rotate(0, 0, 180);
        }
    }

    private void SetCard(Transform t, List<(Card, CardSuit)> cards) {
        for (int i = 0; i < cards.Count; i++) {
            var card = cards[i];
            GameObject[] deck;
            GetCardDeck(card.Item2, out deck);

            var cardObject = deck[(int)card.Item1 - 1];
            cardObject.transform.SetParent(t);
            cardObject.transform.localPosition = new Vector3(i * 0.1f, 0, 0);
            cardObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private IEnumerator SetCard(int deckCount, List<(Card, CardSuit)> playerCards, List<(Card, CardSuit)> bankerCards) {
        int playerIdx = 0;
        int bankerIdx = 0;

        while(playerIdx < playerCards.Count || bankerIdx < bankerCards.Count) {
            if (playerIdx < playerCards.Count) {
                var card = playerCards[playerIdx];
                GameObject[] deck;
                GetCardDeck(card.Item2, out deck);

                var cardObject = deck[(int)card.Item1 - 1];
                cardObject.transform.SetParent(playerCardTransform);
                cardObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                cardObject.transform.localPosition = new Vector3(
                    cardObject.transform.localPosition.x,
                    deckCount * cardThickness,
                    cardObject.transform.localPosition.z);
                float t = 0;
                var target = new Vector3(playerIdx * 0.1f, 0.0f, 0);
                while(t < 1) {
                    t += Time.deltaTime / delay;
                    cardObject.transform.localPosition = Vector3.Lerp(cardObject.transform.localPosition, target, t);
                    yield return null;
                }
                cardObject.transform.localPosition = target;
                playerIdx++;
            }

            if (bankerIdx < bankerCards.Count) {
                var card = bankerCards[bankerIdx];
                GameObject[] deck;
                GetCardDeck(card.Item2, out deck);

                var cardObject = deck[(int)card.Item1 - 1];
                cardObject.transform.SetParent(bankerCardTransform);
                cardObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                cardObject.transform.localPosition = new Vector3(
                    cardObject.transform.localPosition.x,
                    deckCount * cardThickness,
                    cardObject.transform.localPosition.z);
                float t = 0;
                var target = new Vector3(bankerIdx * 0.1f, 0.0f, 0);
                while (t < 1) {
                    t += Time.deltaTime / delay;
                    cardObject.transform.localPosition = Vector3.Lerp(cardObject.transform.localPosition, target, t);
                    yield return null;
                }
                cardObject.transform.localPosition = target;
                bankerIdx++;
            }

            if (playerIdx == 2 && bankerIdx == 2 && (playerCards.Count > 2 || bankerCards.Count > 2)) {
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private void GetCardDeck(CardSuit suit, out GameObject[] deck)
    {
        switch (suit) {
            case CardSuit.Clubs:
                deck = clubs;
                break;
            case CardSuit.Diamonds:
                deck = diamonds;
                break;
            case CardSuit.Hearts:
                deck = hearts;
                break;
            case CardSuit.Spades:
                deck = spades;
                break;
            default:
                deck = null;
                break;
        }
    }

    private IEnumerator GameProcess()
    {
        yield return SuffleDeck();

        yield return Deal();

        yield return DetermineWinner();

        yield return EndGame();
    }

    private IEnumerator SuffleDeck()
    {
        baccaratManager.ShuffleDeck();
        var deck = baccaratManager.GetCardDeck();
        SetCard(deck);
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator Deal()
    {
        camMoveAmount = Camera.main.transform.forward * 0.5f;
        var camTarget = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
        yield return StartCoroutine(CameraMove(camTarget));

        baccaratManager.DealInitialCards();
        baccaratManager.DealAdditionalCards();
        var deck = baccaratManager.GetCardDeck();
        SetCard(deck);
        var playerHand = baccaratManager.GetPlayerHand();
        var bankerHand = baccaratManager.GetBankerHand();
        yield return StartCoroutine(SetCard(deck.Count, playerHand, bankerHand));

        yield return new WaitForSeconds(delay);
    }

    private IEnumerator DetermineWinner()
    {
        var winner = baccaratManager.DetermineWinner();
        Vector3 camTarget = Camera.main.transform.position;
        if (winner == Winner.Player) {
            camTarget = Camera.main.transform.position + Camera.main.transform.up * -0.3f;
            yield return StartCoroutine(CameraMove(camTarget));
            camMoveAmount += Camera.main.transform.up * -0.3f;
            playerWin.SetActive(true);
        } else if (winner == Winner.Banker) {
            camTarget = Camera.main.transform.position + Camera.main.transform.up * 0.3f;
            yield return StartCoroutine(CameraMove(camTarget));
            camMoveAmount += Camera.main.transform.up * 0.3f;
            bankerWin.SetActive(true);
        } else if (winner == Winner.Tie) {
            camTarget = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
            yield return StartCoroutine(CameraMove(camTarget));
            camMoveAmount += Camera.main.transform.forward * 0.2f;
            tieWin.SetActive(true);
        }
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator EndGame()
    {
        var camTarget = Camera.main.transform.position - camMoveAmount;
        yield return StartCoroutine(CameraMove(camTarget));
        
        baccaratManager.InitializeDeck();
        var deck = baccaratManager.GetCardDeck();
        SetCard(deck);

        dataManager.SetStatus(GameStatus.Betting);
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator CameraMove(Vector3 target)
    {
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime / delay;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, t);
            yield return null;
        }
        Camera.main.transform.position = target;
    }
}
