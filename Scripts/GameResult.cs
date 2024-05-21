public class GameResult
{
    private int playerResult; // 플레이어 결과
    private int bankerResult; // 뱅커 결과

    public GameResult(int player, int banker)
    {
        playerResult = player; // 플레이어 결과 초기화
        bankerResult = banker; // 뱅커 결과 초기화
    }

    public void UpdateResult(int player, int banker)
    {
        playerResult = player; // 플레이어 결과 업데이트
        bankerResult = banker; // 뱅커 결과 업데이트
    }

    public bool IsTie()
    {
        return playerResult == bankerResult; // 무승부 여부 확인
    }

    public bool PlayerWon()
    {
        return playerResult > bankerResult; // 플레이어 승리 여부 확인
    }
}
