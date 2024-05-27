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
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
    }



    //tie , banker , player에 프리팹을 생성하고 떨어뜨리는 함수
    public void DropAtPosition1_tie()
    {
        if (dm.currentStatus == GameStatus.Dealing)
            return;
        int amount = (int)dm.BetAmountOnTie;
        int num50 = amount / 50;
        amount %= 50;
        int num10 = amount / 10;
        amount %= 10;
        int num5 = amount / 5;
        amount %= 5;
        int num1 = amount;

        StartCoroutine(DropPrefabsCoroutine(position1, num50, num10, num5, num1));
    }

    
    public void DropAtPosition2_banker()
    {
        if (dm.currentStatus == GameStatus.Dealing)
            return;
        int amount = (int)dm.BetAmountOnBanker;
        int num50 = amount / 50;
        amount %= 50;
        int num10 = amount / 10;
        amount %= 10;
        int num5 = amount / 5;
        amount %= 5;
        int num1 = amount;

        StartCoroutine(DropPrefabsCoroutine(position1, num50, num10, num5, num1));
    }


    public void DropAtPosition3_player()
    {
        if (dm.currentStatus == GameStatus.Dealing)
            return;
        int amount = (int)dm.BetAmountOnPlayer;
        int num50 = amount / 50;
        amount %= 50;
        int num10 = amount / 10;
        amount %= 10;
        int num5 = amount / 5;
        amount %= 5;
        int num1 = amount;

        StartCoroutine(DropPrefabsCoroutine(position1, num50, num10, num5, num1));
    }

    private IEnumerator DropPrefabsCoroutine(Vector3 position, int num50, int num10, int num5, int num1)
    {
        for (int i = 0; i < num50; i++)
        {
            SpawnPrefab(prefab50, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num10; i++)
        {
            SpawnPrefab(prefab10, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num5; i++)
        {
            SpawnPrefab(prefab5, position);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < num1; i++)
        {
            SpawnPrefab(prefab1, position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnPrefab(GameObject prefab, Vector3 position)
    {
        GameObject instance = Instantiate(prefab, new Vector3(position.x, position.y + dropHeight, position.z), Quaternion.identity);
        spawnedPrefabs.Add(instance);
    }

    // 생성된 프리팹들을 전부 사라지게 하는 함수 로직에서 돈을 변환해서 playerMoney 로 변환해줄때 사용해서 없애줄 필요가있음
    public void ClearAllPrefabs()
    {
        if (dm.currentStatus == GameStatus.Dealing)
            return;
        foreach (GameObject prefab in spawnedPrefabs)
        {
            Destroy(prefab);
        }
        spawnedPrefabs.Clear();
    }
}
