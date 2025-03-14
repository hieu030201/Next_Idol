using System.Collections.Generic;
using UnityEngine;

public class YunObjectPool : MonoBehaviour
{
    public static YunObjectPool Instance;

    private readonly Dictionary<GameObject, Queue<GameObject>> _poolDictionary = new ();
    private readonly Dictionary<GameObject, bool> _expandSetting = new ();
    private readonly Dictionary<GameObject, Transform> _parentDictionary = new ();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Tạo pool cho một prefab với tùy chọn mở rộng và đặt parent Transform
    /// </summary>
    public void CreatePool(GameObject prefab, int initialSize, Transform parent = null, bool allowExpand = true)
    {
        if (_poolDictionary.ContainsKey(prefab)) return;
        _poolDictionary[prefab] = new Queue<GameObject>();
        _expandSetting[prefab] = allowExpand;
        _parentDictionary[prefab] = parent; // Lưu parent của prefab

        for (var i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(prefab, parent); // Đặt parent khi Instantiate
            obj.SetActive(false);
            _poolDictionary[prefab].Enqueue(obj);
        }
    }

    /// <summary>
    /// Lấy object từ pool hoặc xử lý theo chế độ cho phép mở rộng hay không
    /// </summary>
    public GameObject GetObject(GameObject prefab, Vector3? position = null, Quaternion? rotation = null)
    {
        if (!_poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("THIS PREFAB POOL IS NOT CREATED: " + prefab.name);
            return null;
        }

        var spawnPosition = position ?? Vector3.zero;
        var spawnRotation = rotation ?? Quaternion.identity;

        if (_poolDictionary[prefab].Count > 0)
        {
            // Debug.LogWarning("REUSED OBJECT IN POOL: " + prefab.name);
            var obj = _poolDictionary[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            obj.SetActive(true);
            return obj;
        }
        if (_expandSetting[prefab]) // Nếu cho phép mở rộng, tạo mới
        {
            // Debug.LogWarning("CREATE NEW OBJECT IN POOL: " + prefab.name);
            var obj = Instantiate(prefab, spawnPosition, spawnRotation, _parentDictionary[prefab]);
            return obj;
        }
        else // Nếu không cho phép mở rộng, lấy object đầu pool (FIFO)
        {
            var obj = _poolDictionary[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            obj.SetActive(true);
            _poolDictionary[prefab].Enqueue(obj); // Đưa object này về cuối hàng đợi
            return obj;
        }
    }

    /// <summary>
    /// Trả object về pool để tái sử dụng
    /// </summary>
    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);

        if (!_poolDictionary.ContainsKey(prefab))
        {
            _poolDictionary[prefab] = new Queue<GameObject>();
        }

        _poolDictionary[prefab].Enqueue(obj);
    }
}
