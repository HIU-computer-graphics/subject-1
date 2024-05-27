using UnityEngine;

// NOTE: public variables can be changed to use Property(get/set) of C#
public class DataManager : MonoBehaviour
{
    [SerializeField] private int StartMoney;

    // NOTE: This variable is for initialize. Is it okay with here?
    public float PlayerMoney;

    public int BetAmountOnPlayer;
    public int BetAmountOnBanker;
    public int BetAmountOnTie;
    public int Chip;

    private GameStatus currentStatus;

    public CardManager cardManager;
    public gamblescript gambleScript;
    public DropPrefabs dropPrefabs;

    void Awake()
    {
        PlayerMoney = StartMoney;
        BetAmountOnPlayer = BetAmountOnBanker = BetAmountOnTie = 0;
        SetStatus(GameStatus.Betting);
        Chip = 0;

        if (cardManager == null) {
            cardManager = FindObjectOfType<CardManager>();
        }
        if (gambleScript == null) {
            gambleScript = FindObjectOfType<gamblescript>();
        }
        if (dropPrefabs == null) {
            dropPrefabs = FindObjectOfType<DropPrefabs>();
        }
    }

    public GameStatus GetStatus() { return currentStatus; }

    public void SetStatus(GameStatus status)
    {
        currentStatus = status;

        if (status == GameStatus.Dealing) {
            cardManager.GameStart();
        } else if (status == GameStatus.Betting) {
            BetAmountOnPlayer = BetAmountOnBanker = BetAmountOnTie = 0;
            dropPrefabs.ClearAllPrefabs();
            gambleScript.BetTurnStart();
        }

    }
}

public enum GameStatus
{
    Betting = 1,
    Dealing = 2,
}