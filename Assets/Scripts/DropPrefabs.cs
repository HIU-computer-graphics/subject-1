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
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
    }



    //tie , banker , player�� �������� �����ϰ� ����߸��� �Լ�
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

    // ������ �����յ��� ���� ������� �ϴ� �Լ� �������� ���� ��ȯ�ؼ� playerMoney �� ��ȯ���ٶ� ����ؼ� ������ �ʿ䰡����
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
