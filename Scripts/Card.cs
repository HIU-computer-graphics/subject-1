using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public CardData cardData; // 카드 데이터 참조
    private Animator animator; // 애니메이터 컴포넌트
    private MeshRenderer renderCard; // 메쉬 렌더러 컴포넌트

    private void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        renderCard = GetComponent<MeshRenderer>(); // 메쉬 렌더러 컴포넌트 가져오기
    }

    public void MoveTo(Vector3 target)
    {
        DOTween.Sequence()
            .Append(transform.DOMove(target, 1)) // 타겟 위치로 이동
            .Join(transform.DORotate(new Vector3(0, 0, 180), .5f)) // 회전
            .OnComplete(() => {
                Destroy(gameObject); // 이동 완료 후 게임 오브젝트 파괴
            });
    }
}
