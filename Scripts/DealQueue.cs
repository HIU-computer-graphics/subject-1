using UnityEngine;
using System.Collections.Generic;

public enum Flip { FlipUp, Horizontal_FlipUp, FlipDown, Reveal } // 카드 flip 타입 열거형

public static class DealQueue
{
    private static List<GameObject> processList = new List<GameObject>(); // 처리할 카드 리스트
    public static bool processing = false; // 처리 중인지 여부

    public delegate void voidDelegate(); // void 반환형 delegate
    public static event voidDelegate OnFinishedDealing; // 딜 완료 이벤트

    public static void DealCard(GameObject card)
    {
        processList.Add(card); // 카드 리스트에 추가
        processing = true; // 처리 상태 설정
    }

    public static void ProcessCard()
    {
        if (processList.Count > 0)
            processList.RemoveAt(0); // 첫 번째 카드 제거
        else
            return;

        if (processList.Count == 0)
        {
            processing = false; // 처리 상태 해제
            OnFinishedDealing?.Invoke(); // 딜 완료 이벤트 호출 (null 검사 추가)
        }
    }
}
