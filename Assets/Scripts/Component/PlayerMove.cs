using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Role _player;
    private CharacterController2D _cc2d;
    private bool _canFallDown = true;

    public void SetPlayer(Role player) { _player = player; }

    private void Start()
    {
        _cc2d = GetComponent<CharacterController2D>() ?? throw new ArgumentException();
        RegisterInputAction();
    }

    private void RegisterInputAction()
    {
        var input = GameManager.Instance.Input;
        input.AddDelegation(InputType.Left.ToString(), GoLeftPressAction);
        input.AddDelegation(InputType.Left.ToString(), GoReleaseAction, true);
        input.AddDelegation(InputType.Right.ToString(), GoRightPressAction);
        input.AddDelegation(InputType.Right.ToString(), GoReleaseAction, true);
        input.AddDelegation(InputType.Jump.ToString(), JumpAction);
        input.AddDelegation(InputType.Down.ToString(), DownAction);
    }

    private void GoLeftPressAction(in KeyInputEventArgs args)
    {
        _player.SetFaceToLeft();
        _player.SetMoveState(true);
        _cc2d.Move(new Vector2(-1 * (5 * Time.deltaTime), 0));
        _canFallDown = true;
    }

    private void GoReleaseAction(in KeyInputEventArgs args) { _player.SetMoveState(false); }

    private void GoRightPressAction(in KeyInputEventArgs args)
    {
        _player.SetFaceToRight();
        _player.SetMoveState(true);
        _cc2d.Move(new Vector2(1 * (5 * Time.deltaTime), 0));
        _canFallDown = true;
    }

    private void JumpAction(in KeyInputEventArgs args)
    {
        _cc2d.Move(new Vector2(0, 1 * (5 * Time.deltaTime)));
        _cc2d.IsIgnorePlatform = true;
        _canFallDown = false;
    }

    private void DownAction(in KeyInputEventArgs args)
    {
        _cc2d.IsIgnorePlatform = true;
        _canFallDown = true;
    }

    private void Update()
    {
        if (_canFallDown)
        {
            _cc2d.Move(new Vector2(0, -2 * (5 * Time.deltaTime)));
        }

        _canFallDown = true;
    }
}