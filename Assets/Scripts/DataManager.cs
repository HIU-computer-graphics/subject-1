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

    public GameStatus currentStatus;

    void Awake()
    {
        PlayerMoney = StartMoney;
        BetAmountOnPlayer = BetAmountOnBanker = BetAmountOnTie = 0;
        currentStatus = GameStatus.Betting;
        Chip = 0;
    }
}

public enum GameStatus
{
    Betting = 1,
    Dealing = 2,
}