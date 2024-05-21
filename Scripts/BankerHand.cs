using UnityEngine;

public class BankerHand : Hand
{
    public bool stand = false; // 뱅커가 스탠드했는지 여부

    override public void ResetHand()
    {
        base.ResetHand(); // 기본 클래스의 ResetHand 호출
        stand = false; // 스탠드 상태 초기화
    }

    public void Stand()
    {
        stand = true; // 스탠드 상태 설정
    }

    public bool CanHit()
    {
        return currentScore > 0 && !IsEnded(); // 히트할 수 있는지 여부 확인
    }

    public bool IsEnded()
    {
        return currentScore > 20 || stand; // 게임이 종료되었는지 여부 확인
    }

    public bool HasPush(int anotherScore)
    {
        return IsEnded() && currentScore == anotherScore; // 푸시(무승부)인지 여부 확인
    }

    public bool HasLost(int dealerScore, bool dealerBlackjack)
    {
        return dealerScore > currentScore && dealerScore < 22; // 패배 여부 확인
    }

    public bool IsNatural8()
    {
        return tableCards.Count == 2 &&
               tableCards[0].GetComponent<Card>().cardData.GetValue() +
               tableCards[1].GetComponent<Card>().cardData.GetValue() == 8; // 내추럴 8인지 여부 확인
    }

    public bool IsNatural9()
    {
        return tableCards.Count == 2 &&
               tableCards[0].GetComponent<Card>().cardData.GetValue() +
               tableCards[1].GetComponent<Card>().cardData.GetValue() == 9; // 내추럴 9인지 여부 확인
    }

    public bool HasAnAce()
    {
        return tableCards[0].GetComponent<Card>().cardData.rank == CardData.Rank.Ace ||
               tableCards[1].GetComponent<Card>().cardData.rank == CardData.Rank.Ace; // 에이스를 보유하고 있는지 여부 확인
    }
}
