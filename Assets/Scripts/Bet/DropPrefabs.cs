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
    public float dropHeight = 5f; // Prefabs가 떨어질 높이


    //3개의 position을 만드는데 1, 2, 3 포지션이 순서대로 tie , player, banker이고 각각의 떨어질 위치에 지정 및 떨어지는 오브젝트에 collider 를 추가해야함


    private List<GameObject> spawnedPrefabs = new List<GameObject>(); // 생성된 프리팹을 저장할 리스트
    private DataManager dm;

    void Start()
    {
        dm = FindObjectOfType<DataManager>();
    }


    private int beforeBetAmountOnPlayer;
    private int beforeBetAmountOnBanker;
    private int beforeBetAmountOnTie;
    //tie , banker , player에 프리팹을 생성하고 떨어뜨리는 함수
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

    // 생성된 프리팹들을 전부 사라지게 하는 함수 로직에서 돈을 변환해서 playerMoney 로 변환해줄때 사용해서 없애줄 필요가있음
    public void ClearAllPrefabs()
    {
        beforeBetAmountOnBanker = beforeBetAmountOnPlayer = beforeBetAmountOnTie = 0;
        foreach (GameObject prefab in spawnedPrefabs) {
            Destroy(prefab);
        }
        spawnedPrefabs.Clear();
    }
}