using System;
using UnityEngine;

/// <summary>
/// 仅用于测试运动
/// </summary>
public class TestPlayerMove : MonoBehaviour
{
    private CharacterController2D _cc2d;

    private void Start() { _cc2d = GetComponent<CharacterController2D>() ?? throw new ArgumentException(); }

    private void Update()
    {
        var dir = new Vector2();
        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            _cc2d.IsIgnorePlatform = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            _cc2d.IsIgnorePlatform = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            dir.x += -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        if (dir.y == 0)
        {
            dir.y = -2;
        }

        _cc2d.Move(dir * (5 * Time.deltaTime));
    }
}