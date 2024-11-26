using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        public string objectName;

        public GameObject perfab;

        public int count;
    }

    public static ObjectPoolManager instance;

    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    private string objectName;

    // 오브젝트들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트 풀에서 오브젝트를 새로 생성할때 사용 할 딕셔너리
    private Dictionary<string, GameObject> objectDic = new Dictionary<string, GameObject>();

    //public int defaultCapacity = 0;
    //public int maxPoolSize = 0;
    //public GameObject mapBoxPrefab;

    public IObjectPool<GameObject> Pool { get; private set; }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(instance);
        }

        Init();
    }

    private void Init()
    {
       //Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
        
        //for(int i = 0; i < defaultCapacity; i++) // 미리 생성해두는 오브젝트에 컴포넌트
        //{
        //    WallScript boxMap = CreatePooledItem().GetComponent< WallScript>();
        //    boxMap.Pool.Release(boxMap.gameObject);
        //}

        IsReady = false;

        for(int i = 0; i < objectInfos.Length; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject
                , true, objectInfos[i].count, objectInfos[i].count);

            if (objectDic.ContainsKey(objectInfos[i].objectName))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트", objectInfos[i].objectName);
                return;
            }

            objectDic.Add(objectInfos[i].objectName, objectInfos[i].perfab);
            objectPoolDic.Add(objectInfos[i].objectName, pool);

            for(int j = 0; j < objectInfos[i].count; j++)
            {
                objectName = objectInfos[i].objectName;
                PoolAble poolAble = CreatePooledItem().GetComponent<PoolAble>();
                poolAble.Pool.Release(poolAble.gameObject);
            }
        }
        Debug.LogFormat("오브젝트 풀링 준비 완");
        IsReady = true;

    }

    private GameObject CreatePooledItem()
    {
        GameObject poolMap = Instantiate(objectDic[objectName]);
        poolMap.GetComponent<PoolAble>().Pool = objectPoolDic[objectName];
        return poolMap;
    }

    private void OnTakeFromPool(GameObject poolMap)
    {
        poolMap.SetActive(true);
    }

    private void OnReturnToPool(GameObject poolMap)
    {
        poolMap.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject poolMap)
    {
        Destroy(poolMap);
    }

    public GameObject GetMapBox(string name)
    {
        objectName = name;

        if (objectDic.ContainsKey(objectName) == false)
        {
            Debug.LogFormat("{0} 등록되지 않은 오브젝트", name);
            return null;
        }

        return objectPoolDic[name].Get();
    }
}
