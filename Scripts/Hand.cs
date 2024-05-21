using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

[System.Serializable]
public class Hand : MonoBehaviour
{
    protected Transform handPosition; // 핸드 위치
    protected Transform drawPosition; // 카드 드로우 위치

    public GameObject scoreOutput; // 점수 출력 UI 요소
    public GameObject scorePanel; // 점수 패널 UI 요소

    [Range(0.5f, 0.6f)]
    public float cardXShift = 0.52f; // 카드 간격
    public int currentScore = 0; // 현재 점수

    protected List<GameObject> tableCards; // 테이블에 놓인 카드 리스트
    protected Vector3 nextTransformPosition; // 다음 카드 위치

    private void Awake()
    {
        tableCards = new List<GameObject>(); // 카드 리스트 초기화
        handPosition = transform.GetChild(0); // 핸드 위치 설정
    }

    void Start()
    {
        ResetHand(); // 핸드 초기화
        drawPosition = GameObject.Find("/DrawPosition").transform; // 드로우 위치 찾기
        if (scoreOutput == null || scorePanel == null)
            return;
        scoreOutput.SetActive(false); // 점수 출력 숨기기
        scorePanel.SetActive(false); // 점수 패널 숨기기
    }

    public GameObject DealCard(CardData card)
    {
        return InitialiseInShoe(card); // 카드 초기화 및 딜
    }

    public void ResetScore()
    {
        currentScore = 0; // 점수 초기화
        UpdateScoreView(); // 점수 보기 업데이트

        if (scoreOutput == null || scorePanel == null)
            return;
        scoreOutput.SetActive(false); // 점수 출력 숨기기
        scorePanel.SetActive(false); // 점수 패널 숨기기
    }

    public virtual void ResetHand()
    {
        nextTransformPosition = handPosition.transform.position; // 다음 카드 위치 초기화
        tableCards.Clear(); // 테이블 카드 초기화
        ResetScore(); // 점수 초기화
    }

    public bool IsBust()
    {
        return currentScore > 21; // 버스트 여부 확인
    }

    protected GameObject InitialiseInShoe(CardData card)
    {
        GameObject cardObj = Resources.Load<GameObject>("Card"); // 카드 오브젝트 로드
        GameObject newCardGameObject = Instantiate(cardObj, drawPosition.position, drawPosition.rotation) as GameObject; // 카드 인스턴스 생성

        TableResetManager.AddCard(newCardGameObject); // 테이블 리셋 매니저에 카드 추가
        newCardGameObject.GetComponentInChildren<MeshFilter>().mesh = card.GetMesh(); // 카드 메쉬 설정
        LinkCardWithData(newCardGameObject, card); // 카드 데이터 연결
        AddCardToHand(newCardGameObject); // 카드 핸드에 추가

        return newCardGameObject; // 새 카드 오브젝트 반환
    }

    protected void AddCardToHand(GameObject card)
    {
        card.transform.SetParent(handPosition.parent); // 카드 부모 설정
        SetupPosition(card); // 카드 위치 설정
        tableCards.Add(card); // 테이블 카드 리스트에 추가
        CalculateScore(); // 점수 계산
    }

    public bool HasPerfectPair()
    {
        return tableCards[0].GetComponentInChildren<Card>().cardData.suit.Equals(tableCards[1].GetComponentInChildren<Card>().cardData.suit) &&
               tableCards[0].GetComponentInChildren<Card>().cardData.rank.Equals(tableCards[1].GetComponentInChildren<Card>().cardData.rank); // 퍼펙트 페어 여부 확인
    }

    public bool HasPair()
    {
        return tableCards[0].GetComponentInChildren<Card>().cardData.rank.Equals(tableCards[1].GetComponentInChildren<Card>().cardData.rank); // 페어 여부 확인
    }

    public void UpdateScoreView()
    {
        if (scoreOutput == null || scorePanel == null)
            return;

        if (currentScore < 0)
        {
            scoreOutput.SetActive(false); // 점수 출력 숨기기
            scorePanel.SetActive(false); // 점수 패널 숨기기
            return;
        }

        scoreOutput.SetActive(true); // 점수 출력 표시
        scorePanel.SetActive(true); // 점수 패널 표시

        scoreOutput.GetComponent<TMP_Text>().text = currentScore.ToString(); // 점수 텍스트 업데이트
    }

    protected void LinkCardWithData(GameObject cardGameObject, CardData cardData)
    {
        cardGameObject.GetComponentInChildren<Card>().cardData = cardData; // 카드 데이터 연결
    }

    protected virtual void SetupPosition(GameObject cardGameObject)
    {
        Flip flipType = GetFlipType(tableCards.Count > 1); // 플립 타입 설정
        Vector3 flipPosition = Vector3.zero;
        if (flipType == Flip.Horizontal_FlipUp)
            flipPosition = new Vector3(0, 90, 0); // 플립 위치 설정

        Sequence sq = DOTween.Sequence();
        sq
            .Append(cardGameObject.transform.DOMove(drawPosition.GetChild(0).position, .4f).SetEase(Ease.OutSine)) // 드로우 위치로 이동
            .Join(cardGameObject.transform.DORotate(drawPosition.GetChild(0).rotation.eulerAngles, .2f).SetEase(Ease.InSine)) // 회전
            .Append(cardGameObject.transform.DOJump(nextTransformPosition, .15f, 1, .6f).SetEase(Ease.OutSine)) // 점프
            .Join(cardGameObject.transform.DORotate(flipPosition, .2f).SetEase(Ease.OutSine)) // 회전
            .OnComplete(() =>
            {
                DealQueue.ProcessCard(); // 카드 처리
                UpdateScoreView(); // 점수 보기 업데이트
            });

        UpdateNextPosition(); // 다음 카드 위치 업데이트
    }

    protected virtual void UpdateNextPosition()
    {
        float offset = (tableCards.Count == 1) ? 0.10f : 0; // 오프셋 설정
        nextTransformPosition = nextTransformPosition - new Vector3(-cardXShift - offset, -0.0001f, offset); // 다음 카드 위치 설정
    }

    public void CalculateScore()
    {
        currentScore = 0; // 현재 점수 초기화
        Card card;

        foreach (GameObject cardGO in tableCards)
        {
            card = cardGO.GetComponentInChildren<Card>();
            currentScore += card.cardData.GetValue(); // 카드 값 추가
        }
        currentScore %= 10; // 점수 10으로 나눈 나머지
    }

    public int GetCurrentScore()
    {
        return currentScore %= 10; // 현재 점수 반환
    }

    protected virtual Flip GetFlipType(bool isThirdCard)
    {
        if (isThirdCard)
            return Flip.Horizontal_FlipUp; // 세 번째 카드일 경우 가로 플립

        return Flip.FlipUp; // 기본 플립
    }

    public int GetNumberOfCards()
    {
        return tableCards.Count; // 테이블에 있는 카드 개수 반환
    }
}
