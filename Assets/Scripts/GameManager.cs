using System;
using UnityEngine;
//TODO :
// 1、减淡GKD *************************************
//2、弓箭手技能安排好，然后去skill那边写
//3、 playmove里面的输入修改一下，
public enum InputType
{
    Up,
    Down,
    Left,
    Right,
    Skill1,
    Skill2,
    UseItem,
    Ultimate,
    Dash,
    NormalAttack,
    Jump
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private ResourceDatabase _db;
    private Camera _mainCamera;
    [SerializeField] private World _activeWorld;
    [SerializeField] private ScreenCanvas _screenCanvas;
    private InputManager _input;

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

    public ScreenCanvas MainCanvas => _screenCanvas;

    public InputManager Input => _input;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }

        _db = Resources.Load<ResourceDatabase>(nameof(ResourceDatabase)) ?? throw new ArgumentException();
        _input = new InputManager();
        RegisterKeyInput();
    }

    private void Start() { _mainCamera = Camera.main; }

    private void Update() { _input.Update(); }

    /// <summary>
    /// 加载世界
    /// </summary>
    public void LoadWorld(GameObject worldObject)
    {
        if (_activeWorld != null) throw new InvalidOperationException("存在活动中世界");
        var world = Instantiate(worldObject);
        if (!world.TryGetComponent(out _activeWorld))
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

    private void RegisterKeyInput()
    {
        _input.AddBinding(InputType.Up.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.UpArrow));
        _input.AddBinding(InputType.Down.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.DownArrow));
        _input.AddBinding(InputType.Left.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.LeftArrow));
        _input.AddBinding(InputType.Right.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.RightArrow));

        _input.AddBinding(InputType.NormalAttack.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.X));
        _input.AddBinding(InputType.Jump.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.C));
        _input.AddBinding(InputType.Dash.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.Z));

        _input.AddBinding(InputType.Skill1.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.A));
        _input.AddBinding(InputType.Skill2.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.S));
        _input.AddBinding(InputType.UseItem.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.D));
        _input.AddBinding(InputType.Ultimate.ToString(), KeyBindingInfo.CreateKeyboard(KeyCode.F));
    }
}