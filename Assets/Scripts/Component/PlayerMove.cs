using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Role _player;
    private CharacterController2D _cc2d;

    public void SetPlayer(Role player) { _player = player; }

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
            _player.SetFaceToLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            _player.SetFaceToRight();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _player.SetMoveState(true);
        }
        else
        {
            _player.SetMoveState(false);
        }
        
        if (dir.y == 0)
        {
            dir.y = -2;
        }

        _cc2d.Move(dir * (5 * Time.deltaTime));
    }
}