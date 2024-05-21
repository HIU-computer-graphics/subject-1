using UnityEngine;
using DG.Tweening;

public class PlayerHand : Hand
{
    public bool stand = false; // 플레이어 스탠드 여부

    override public void ResetHand()
    {
        base.ResetHand(); // 기본 클래스의 ResetHand 호출
        stand = false; // 스탠드 상태 초기화
    }

    protected override void UpdateNextPosition()
    {
        float offset = (tableCards.Count == 1) ? 0.10f : 0;
        nextTransformPosition = nextTransformPosition - new Vector3(cardXShift + offset, -0.0001f, offset); // 다음 카드 위치 설정
    }

    public void Stand()
    {
        stand = true; // 스탠드 상태 설정
    }

    public bool CanHit()
    {
        return currentScore > 0 && !IsEnded(); // 히트 가능 여부 확인
    }

    public bool IsEnded()
    {
        return tableCards.Count >= 3 || stand; // 핸드 종료 여부 확인
    }

    public bool HasPush(int anotherScore)
    {
        return IsEnded() && currentScore == anotherScore; // 푸시(무승부) 여부 확인
    }

    public bool HasLost(int dealerScore, bool dealerBlackjack)
    {
        return dealerScore > currentScore && dealerScore < 22; // 패배 여부 확인
    }

    public bool IsNatural8()
    {
        return tableCards.Count == 2 &&
               tableCards[0].GetComponentInChildren<Card>().cardData.GetValue() +
               tableCards[1].GetComponentInChildren<Card>().cardData.GetValue() == 8; // 내추럴 8 여부 확인
    }

    public bool IsNatural9()
    {
        return tableCards.Count == 2 &&
               tableCards[0].GetComponentInChildren<Card>().cardData.GetValue() +
               tableCards[1].GetComponentInChildren<Card>().cardData.GetValue() == 9; // 내추럴 9 여부 확인
    }

    public int TakeValueFromCard(int index)
    {
        return tableCards[index].GetComponentInChildren<Card>().cardData.GetValue(); // 지정된 인덱스의 카드 값 가져오기
    }

    public bool HasAnAce()
    {
        return tableCards[0].GetComponentInChildren<Card>().cardData.rank == CardData.Rank.Ace ||
               tableCards[1].GetComponentInChildren<Card>().cardData.rank == CardData.Rank.Ace; // 에이스 보유 여부 확인
    }
}
