using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPrefabs : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab5;
    public GameObject prefab10;
    public GameObject prefab50;
    public Vector3 position1;
    public Vector3 position2;
    public Vector3 position3;
    public float dropHeight = 5f; // Prefabs�� ������ ����


    //3���� position�� ����µ� 1, 2, 3 �������� ������� tie , player, banker�̰� ������ ������ ��ġ�� ���� �� �������� ������Ʈ�� collider �� �߰��ؾ���


    private List<GameObject> spawnedPrefabs = new List<GameObject>(); // ������ �������� ������ ����Ʈ
    private DataManager dm;

    void Start()
    {
        dm = FindObjectOfType<DataManager>();
    }


    private int beforeBetAmountOnPlayer;
    private int beforeBetAmountOnBanker;
    private int beforeBetAmountOnTie;
    //tie , banker , player�� �������� �����ϰ� ����߸��� �Լ�
    public void DropAtPosition1_tie()
    {
        if (beforeBetAmountOnTie == dm.BetAmountOnTie)
            return;
        StartCoroutine(DropPrefabsCoroutine(position1, dm.Chip));
        beforeBetAmountOnTie = dm.BetAmountOnTie;
    }


    public void DropAtPosition2_banker()
    {
        if (beforeBetAmountOnBanker == dm.BetAmountOnBanker)
            return;
        StartCoroutine(DropPrefabsCoroutine(position2, dm.Chip));
        beforeBetAmountOnBanker = dm.BetAmountOnBanker;
    }


    public void DropAtPosition3_player()
    {
        if (beforeBetAmountOnPlayer == dm.BetAmountOnPlayer)
            return;
        StartCoroutine(DropPrefabsCoroutine(position3, dm.Chip));
        beforeBetAmountOnPlayer = dm.BetAmountOnPlayer;
    }

    private IEnumerator DropPrefabsCoroutine(Vector3 position, int num50, int num10, int num5, int num1)
    {
        for (int i = 0; i < num50; i++) {
            SpawnPrefab(prefab50, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num10; i++) {
            SpawnPrefab(prefab10, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num5; i++) {
            SpawnPrefab(prefab5, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num1; i++) {
            SpawnPrefab(prefab1, position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator DropPrefabsCoroutine(Vector3 position, int betAmount)
    {
        int num50 = betAmount / 50;
        betAmount %= 50;
        int num10 = betAmount / 10;
        betAmount %= 10;
        int num5 = betAmount / 5;
        betAmount %= 5;
        int num1 = betAmount;

        for (int i = 0; i < num50; i++) {
            SpawnPrefab(prefab50, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num10; i++) {
            SpawnPrefab(prefab10, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num5; i++) {
            SpawnPrefab(prefab5, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num1; i++) {
            SpawnPrefab(prefab1, position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnPrefab(GameObject prefab, Vector3 position)
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.position = position + Vector3.up * dropHeight;
        var rigid = instance.GetComponent<Rigidbody>();
        spawnedPrefabs.Add(instance);
    }

    // ������ �����յ��� ���� ������� �ϴ� �Լ� �������� ���� ��ȯ�ؼ� playerMoney �� ��ȯ���ٶ� ����ؼ� ������ �ʿ䰡����
    public void ClearAllPrefabs()
    {
        beforeBetAmountOnBanker = beforeBetAmountOnPlayer = beforeBetAmountOnTie = 0;
        foreach (GameObject prefab in spawnedPrefabs) {
            Destroy(prefab);
        }
        spawnedPrefabs.Clear();
    }
}