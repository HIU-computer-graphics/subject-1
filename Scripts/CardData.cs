using UnityEngine;

[System.Serializable]
public class CardData
{
    public enum Suit { Hearts, Diamonds, Spades, Clubs } // 카드 슈트 열거형
    public enum Rank { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King } // 카드 랭크 열거형

    public Suit suit; // 카드 슈트
    public Rank rank; // 카드 랭크

    public CardData(Suit suit, Rank rank)
    {
        this.suit = suit; // 슈트 초기화
        this.rank = rank; // 랭크 초기화
    }

    public string DebugName()
    {
        return rank.ToString() + " of " + suit.ToString() + ", " + GetValue(); // 디버그용 카드 이름 반환
    }

    public int GetValue()
    {
        if ((int)rank < 9)
            return (int)rank + 1; // 랭크가 Ace ~ Nine인 경우 값 반환
        return 0; // 랭크가 Ten, Jack, Queen, King인 경우 값 0 반환
    }

    public string GetAssetName()
    {
        return rank.ToString().ToLower() + suit.ToString().ToLower(); // 카드 에셋 이름 반환
    }

    public Mesh GetMesh()
    {
        string meshName = GetAssetName();
        return Resources.Load<Mesh>("Cards/" + meshName); // 카드 메쉬 로드 및 반환
    }
}
