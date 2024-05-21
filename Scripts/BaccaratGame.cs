using System.Collections;
using UnityEngine;

public class BaccaratGame : MonoBehaviour
{
    public Player player; // 플레이어 객체 참조
    public CardDeck cardDeck; // 카드 덱 참조
    public UIStates uiStates; // UI 상태 관리자 참조
    public ResultManager resultManager; // 결과 관리자 참조
    public TableResetManager tabletReset; // 테이블 리셋 관리자 참조
    public int deckCount = 3; // 덱의 개수
    private float END_GAME_PAUSE = 1.2f; // 게임 종료 후 대기 시간

    [HideInInspector]
    public delegate void OnGameReady();
    public static OnGameReady Done; // 게임 준비 완료 이벤트
    public static bool OnPlay { get; private set; } = false; // 게임 진행 여부

    void Awake()
    {
        cardDeck = new CardDeck(deckCount); // 덱 초기화
    }

    private void OnEnable()
    {
        DealQueue.OnFinishedDealing += FinishedDealing; // 딜 완료 이벤트 구독
        TableResetManager.Done += HardResetTable; // 테이블 리셋 이벤트 구독
    }

    private void OnDisable()
    {
        DealQueue.OnFinishedDealing -= FinishedDealing; // 딜 완료 이벤트 구독 해제
        TableResetManager.Done -= HardResetTable; // 테이블 리셋 이벤트 구독 해제
    }

    void Start()
    {
        ResetTable(); // 게임 시작 시 테이블 리셋
    }

    public void ClearAllPlayerBets()
    {
        player.ClearBet(); // 모든 플레이어 베팅 초기화
    }

    public void Deal()
    {
        resultManager.winHandler.HideResult(); // 결과 화면 숨기기
        if (player.IsPlacingBet())
        {
            ResultManager.betsEnabled = false; // 베팅 비활성화
            StartCoroutine(InitialDeal()); // 초기 딜 시작
        }
    }

    private IEnumerator InitialDeal()
    {
        OnPlay = true; // 게임 진행 상태 설정
        for (int i = 0; i < 2; i++)
        {
            DealHand(0); // 플레이어에게 카드 딜
            yield return new WaitForSeconds(.3f); // 0.3초 대기
            DealHand(1); // 뱅커에게 카드 딜
            yield return new WaitForSeconds(.3f); // 0.3초 대기
        }
        player.canDeal = false; // 플레이어의 딜 가능 상태 비활성화
    }

    private void DealHand(int id) // id: 0은 플레이어, 1은 뱅커
    {
        if (player.playerHands[id].GetNumberOfCards() < 3)
        {
            uiStates.SetEnabled(false); // UI 상태 비활성화
            CardData card = cardDeck.GetCard(); // 카드 덱에서 카드 한 장 추출
            DealQueue.DealCard(player.DealCard(card, id)); // 카드 딜
        }
    }

    void FinishedDealing()
    {
        uiStates.SetEnabled(true); // UI 상태 활성화
        uiStates.OnStateChange(); // UI 상태 변경 트리거
        Debug.Log("DEALING FINISHED"); // 딜 완료 로그 출력
        CheckIfEnded(); // 게임 종료 여부 확인
    }

    private bool playerHas3 = false; // 플레이어가 세 번째 카드를 받았는지 여부

    public void CheckIfEnded()
    {
        if (player.IsPlayerEnded())
        {
            player.NextHand(); // 플레이어 핸드 종료 시 다음 핸드로 이동
        }
        else if (player.IsBankerEnded())
        {
            PlayerIsFinished(); // 뱅커 핸드 종료 시 플레이어 핸드 완료 처리
        }

        // 플레이어 규칙
        if (player.hand.GetCurrentScore() >= 0 && player.hand.GetCurrentScore() < 6 && player.hand.GetNumberOfCards() == 2)
        {
            DealHand(0); // 플레이어에게 카드 딜
            playerHas3 = true; // 플레이어가 세 번째 카드를 받음
            player.hand.Stand(); // 플레이어 스탠드
        }
        else if (player.hand.GetCurrentScore() >= 6 || player.hand.GetNumberOfCards() >= 3)
        {
            player.hand.Stand(); // 플레이어 스탠드
        }

        // 뱅커 규칙
        if ((player.hand.GetCurrentScore() == 6 || player.hand.GetCurrentScore() == 7) && player.bankerHand.GetCurrentScore() < 6 && player.bankerHand.GetNumberOfCards() == 2)
        {
            DealHand(1); // 뱅커에게 카드 딜
            player.bankerHand.Stand(); // 뱅커 스탠드
        }
        else if (playerHas3 && player.bankerHand.GetNumberOfCards() == 2 && player.hand.GetNumberOfCards() == 3 && player.bankerHand.GetCurrentScore() < 7)
        {
            int playerThirdCard = player.hand.TakeValueFromCard(2); // 플레이어의 세 번째 카드 값
            int bankerScore = player.bankerHand.GetCurrentScore(); // 뱅커 점수

            if (bankerScore < 3)
            {
                DealHand(1); // 뱅커에게 카드 딜
                player.bankerHand.Stand(); // 뱅커 스탠드
            }
            else if (bankerScore == 3 && playerThirdCard != 8)
            {
                DealHand(1); // 뱅커에게 카드 딜
                player.bankerHand.Stand(); // 뱅커 스탠드
            }
            else if (bankerScore == 4 && playerThirdCard > 1 && playerThirdCard < 8)
            {
                DealHand(1); // 뱅커에게 카드 딜
                player.bankerHand.Stand(); // 뱅커 스탠드
            }
            else if (bankerScore == 5 && playerThirdCard > 3 && playerThirdCard < 8)
            {
                DealHand(1); // 뱅커에게 카드 딜
                player.bankerHand.Stand(); // 뱅커 스탠드
            }
            else if (bankerScore == 6 && (playerThirdCard == 6 || playerThirdCard == 7))
            {
                DealHand(1); // 뱅커에게 카드 딜
                player.bankerHand.Stand(); // 뱅커 스탠드
            }
            else
            {
                player.bankerHand.Stand(); // 뱅커 스탠드
            }
        }
        else
        {
            player.bankerHand.Stand(); // 뱅커 스탠드
        }

        PlayerIsFinished(); // 플레이어 핸드 완료 처리
    }

    private void PlayerIsFinished()
    {
        if (player.BothHandsEnded())
        {
            StartCoroutine(EndGame()); // 게임 종료 처리 시작
        }
    }

    private IEnumerator EndGame()
    {
        while (DealQueue.processing)
        {
            yield return new WaitForSeconds(.5f); // 딜 큐 처리 중 0.5초 대기
        }

        if (!player.endingGame)
        {
            player.endingGame = true; // 게임 종료 상태 설정
            Debug.Log(player.name + " Is getting their awards..."); // 플레이어 보상 로그 출력
            resultManager.SetResult(player.hand.GetCurrentScore(), player.bankerHand.GetCurrentScore()); // 게임 결과 설정
            uiStates.gameEnded = true; // 게임 종료 상태 설정
            yield return new WaitForSeconds(2); // 2초 대기
            ResetTable(); // 테이블 리셋
        }
    }

    private void ResetTable()
    {
        player.ResetScore(); // 플레이어 점수 리셋
        tabletReset.Cleanup(); // 테이블 정리
        BetHistoryManager._Instance.ResetHistory(); // 베팅 기록 리셋
        Done(); // 게임 준비 완료 이벤트 호출
        ResultManager.betsEnabled = !MenuManager.IsActive; // 베팅 활성화 상태 설정
        Player.totalBet = 0; // 총 베팅 금액 초기화
    }

    public void Rebet()
    {
        resultManager.winHandler.HideResult(); // 결과 화면 숨기기
        uiStates.OnStateChange(); // UI 상태 변경 트리거
        player.canDeal = true; // 플레이어 딜 가능 상태 설정
    }

    private void HardResetTable()
    {
        OnPlay = false; // 게임 진행 상태 해제
        player.ResetTable(); // 플레이어 테이블 리셋
        player.canDeal = true; // 플레이어 딜 가능 상태 설정
    }
}
