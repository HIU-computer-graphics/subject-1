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
        // Rigidbody ������Ʈ�� ������
        rb = GetComponent<Rigidbody>();


        boxCollider = GetComponent<BoxCollider>();

        // Collider�� ũ�⸦ ������Ʈ�� ũ��� ���߱�
      //  boxCollider.size = GetComponent<Renderer>().bounds.size;

        // �ʱ� �ӵ��� ����
        rb.velocity = Vector3.down * initialSpeed;

        // �߷��� Ȱ��ȭ
        rb.useGravity = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� �߻����� �� ������ ���� �߷��� ��Ȱ��ȭ
        rb.useGravity = false;

        // �浹 �� �ӵ��� 0���� ����
        rb.velocity = Vector3.zero;
        // ��������  ��������
        rb.isKinematic = true;

    }
}
