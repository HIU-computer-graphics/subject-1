using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class FallingObject : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider boxCollider;
    public float initialSpeed = 10f;

    void Start()
    {
        // Rigidbody 컴포넌트를 가져옴
        rb = GetComponent<Rigidbody>();


        boxCollider = GetComponent<BoxCollider>();

        // Collider의 크기를 오브젝트의 크기와 맞추기
      //  boxCollider.size = GetComponent<Renderer>().bounds.size;

        // 초기 속도를 설정
        rb.velocity = Vector3.down * initialSpeed;

        // 중력을 활성화
        rb.useGravity = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌이 발생했을 때 중지를 위해 중력을 비활성화
        rb.useGravity = false;

        // 충돌 시 속도를 0으로 설정
        rb.velocity = Vector3.zero;
        // 떨어진후  완전정지
        rb.isKinematic = true;

    }
}
