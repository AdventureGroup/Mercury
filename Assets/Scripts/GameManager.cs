using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private ResourceDatabase _db;
    private Camera _mainCamera;
    [SerializeField]private World _activeWorld;

    /// <summary>
    /// 数据
    /// </summary>
    public ResourceDatabase Db => _db;

    /// <summary>
    /// 主摄像机
    /// </summary>
    public Camera MainCamera => _mainCamera;

    /// <summary>
    /// 活动中世界，请先调用HasValue检查有没有活动中世界，再调用Value获取世界
    /// </summary>
    public MayNull<World> ActiveWorld => new MayNull<World>(_activeWorld);

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }

        _db = Resources.Load<ResourceDatabase>(nameof(ResourceDatabase)) ?? throw new ArgumentException();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        LoadWorld(0);
    }

    /// <summary>
    /// 加载世界
    /// </summary>
    public void LoadWorld(GameObject worldObject)
    {
        if (_activeWorld != null) throw new InvalidOperationException("存在活动中世界");
        var world = Instantiate(worldObject);
        _activeWorld = world.GetComponent<World>();
        if (_activeWorld == null)
        {
            Destroy(world);
            throw new ArgumentException(nameof(worldObject));
        }

        _activeWorld.OnLoad();
    }

    /// <summary>
    /// 根据数据中的世界列表加载世界
    /// </summary>
    public void LoadWorld(int index) { LoadWorld(Db.Worlds[index].gameObject); }

    /// <summary>
    /// 卸载活动中世界
    /// </summary>
    public void UnloadWorld()
    {
        if (_activeWorld == null) throw new InvalidOperationException("没有活动中世界");
        _activeWorld.OnUnload();
        Destroy(_activeWorld.gameObject);
        _activeWorld = null;
    }
}