using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class ChipStack : MonoBehaviour
{
    private Vector3 intiialPosition; // 칩 스택의 초기 위치
    private float value = 0; // 칩 스택의 값
    private List<GameObject> chips; // 칩 게임 오브젝트 리스트

    static float[] CHIP_VALUES = new float[] { 0.05f, 0.1f, 0.25f, 1, 5, 10, 25, 100 }; // 칩 값 배열
    static string[] CHIP_PREFAB_NAMES = new string[] { "chip.05", "chip.1", "chip.25", "chip1", "chip5", "chip10", "chip25", "chip100" }; // 칩 프리팹 이름 배열
    private BetSpace betSpaceParent; // Parent 베팅 공간 참조

    void Start()
    {
        intiialPosition = transform.position; // 초기 위치 저장
    }

    public void Add(float value)
    {
        SetValue(this.value + value); // 값 추가
    }

    public void Clear()
    {
        value = 0; // 값 초기화

        if (chips != null)
        {
            foreach (GameObject chip in chips)
            {
                Destroy(chip); // 모든 칩 게임 오브젝트 파괴
            }
        }

        chips = null; // 칩 리스트 초기화
    }

    public float GetValue()
    {
        return value; // 칩 스택의 값 반환
    }

    public void SetValue(float value)
    {
        Clear(); // 칩 스택 초기화

        if (value <= 0)
        {
            return; // 값이 0 이하일 경우 종료
        }

        this.value = value; // 값 설정
        chips = new List<GameObject>(); // 칩 리스트 초기화

        int currentChipIndex = CHIP_VALUES.Length - 1; // 가장 큰 칩 값부터 시작

        while (value > 0)
        {
            value = Mathf.Round(value * 100f) / 100f;
            float nextValue = value - CHIP_VALUES[currentChipIndex];

            if (nextValue < 0)
            {
                currentChipIndex--;
                if (currentChipIndex < 0)
                {
                    throw new Exception("불가능한 값"); // 잘못된 값 예외 처리
                }
                continue;
            }

            value = nextValue;

            GameObject newChip = Instantiate(Resources.Load<GameObject>(CHIP_PREFAB_NAMES[currentChipIndex]));
            newChip.transform.parent = gameObject.transform; // 칩 Parent 설정
            newChip.transform.localPosition = new Vector3(0, 0.03f * (chips.Count + 1), 0); // 칩 위치 설정

            chips.Add(newChip); // 칩 리스트에 추가
        }
    }

    public float Win(float multiplier)
    {
        float winAmount = value * multiplier; // 당첨 금액 계산
        SetValue(winAmount); // 당첨 금액 설정
        CollectChips(); // 칩 수집
        return winAmount; // 당첨 금액 반환
    }

    public void CollectChips()
    {
        transform.DOMove(ResultManager.GetChipWinPosition(), .4f).SetDelay(1).SetEase(Ease.InSine).OnComplete(ResetStack); // 칩 수집 애니메이션
    }

    public void SetBetSpaceParent(BetSpace parent)
    {
        betSpaceParent = parent; // Parent 베팅 공간 설정
    }

    public void ResetBet()
    {
        //betSpaceParent.ResetLastBet(); // 마지막 베팅 초기화 (주석 처리됨)
    }

    public void ResetStack()
    {
        transform.position = intiialPosition; // 초기 위치로 복원
        Clear(); // 칩 스택 초기화
    }
}
