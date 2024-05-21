using System.Collections.Generic;

[System.Serializable]
public class CardDeck
{
    public List<CardData> deck; // 덱에 포함된 카드 리스트

    public CardDeck(int deckCount)
    {
        Shuffle(deckCount); // 지정된 덱 수만큼 덱을 섞음
    }

    public void Shuffle(int deckCount)
    {
        deck = new List<CardData>(52 * deckCount); // 덱 리스트 초기화
        do
        {
            AddDeck(); // 덱 추가
            deckCount--; // 덱 수 감소
        } while (deckCount > 0);
        RandomiseDeck(); // 덱 무작위 섞기
    }

    public CardData GetCard()
    {
        CardData card = deck[0]; // 덱의 첫 번째 카드 가져오기
        deck.RemoveAt(0); // 덱에서 첫 번째 카드 제거
        return card; // 카드 반환
    }

    private void AddDeck()
    {
        var suits = (CardData.Suit[])System.Enum.GetValues(typeof(CardData.Suit)); // 모든 슈트 가져오기
        var ranks = (CardData.Rank[])System.Enum.GetValues(typeof(CardData.Rank)); // 모든 랭크 가져오기

        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 0; j < ranks.Length; j++)
            {
                deck.Add(new CardData((CardData.Suit)suits[i], (CardData.Rank)ranks[j])); // 각 카드 덱에 추가
            }
        }
    }

    private void RandomiseDeck()
    {
        FisherYatesCardDeckShuffle(deck); // Fisher-Yates 알고리즘으로 덱 섞기
    }

    private static List<CardData> FisherYatesCardDeckShuffle(List<CardData> aList)
    {
        System.Random _random = new System.Random();
        CardData myGO;
        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO; // 요소 교환
        }
        return aList; // 섞인 리스트 반환
    }
}
