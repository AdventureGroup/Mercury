using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelDebug : MonoBehaviour
{
    public Text EntityCountText;

    private GameManager _gm;

    private void Start() { _gm = GameManager.Instance; }

    private void FixedUpdate()
    {
        var w = _gm.ActiveWorld;
        if (w.HasValue)
        {
            var world = w.Value;
            EntityCountText.text = world.ActiveEntity.Count.ToString();
        }
        else
        {
            EntityCountText.text = "No world";
        }
    }

    public void OnLoadWorldBtnPress(InputField worldField)
    {
        var worldName = worldField.text;
        try
        {
            _gm.LoadWorld(int.Parse(worldName));
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        worldField.text = string.Empty;
    }

    public void OnUnloadWorldBtnPress()
    {
        try
        {
            _gm.UnloadWorld();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void OnCreateEntityBtnPress(InputField entityField)
    {
        try
        {
            var prefab = _gm.Db.Entities[int.Parse(entityField.text)];
            _gm.ActiveWorld.Value.CreateEntity(prefab.gameObject);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void OnCreatePlayerBtnPress(InputField playerField)
    {
        try
        {
            var prefab = _gm.Db.Entities[int.Parse(playerField.text)];
            var entity = _gm.ActiveWorld.Value.CreateEntity(prefab.gameObject);
            entity.gameObject.AddComponent<TestPlayerMove>();//TODO:实现真正的
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void OnDestroyAllEntityBtnPress()
    {
        var world = _gm.ActiveWorld.Value;
        foreach (var e in world.ActiveEntity.ToArray())
        {
            world.DestroyEntity(e);
        }
    }

    public void OnScriptBtnPress()
    {
        //执行一键脚本的内容在这里
        _gm.LoadWorld(0);
        var world = _gm.ActiveWorld.Value;
        world.CreateEntity(_gm.Db.Entities[1].gameObject);
    }
}