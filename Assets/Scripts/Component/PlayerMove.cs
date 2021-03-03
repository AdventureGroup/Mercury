using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Role _player;
    private CharacterController2D _cc2d;
    private bool _canFallDown = true;
    private float DeltaMoveX = 0;
    private float DeltaMoveY = 0;
    private float NowMoveX = 0;
    private float NowMoveY = 0;
    public int DoubleJump = 2;
    public float JumpWait = 0.5f;
    public float DashCoolDown = 0;
    public float DashTime;

    private bool Dash;

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
        //input.AddDelegation(InputType.Jump.ToString(), JumpAction);
        input.AddDelegation(InputType.Down.ToString(), DownAction);
    }

    private void GoLeftPressAction(in KeyInputEventArgs args)
    {
        _player.SetFaceToLeft();
        _player.SetMoveState(true);
        _canFallDown = true;
        //NowMoveX = -5 * (1 + _player.Value.SpeedMov);
        _cc2d.Move(new Vector2(-1 * (3 * ( 1 + _player.Value.SpeedMov) * Time.deltaTime), 0));
    }

    private void GoReleaseAction(in KeyInputEventArgs args) { _player.SetMoveState(false); }

    private void GoRightPressAction(in KeyInputEventArgs args)
    {
        _player.SetFaceToRight();
        _player.SetMoveState(true);
        //NowMoveX = 5 * (1 + _player.Value.SpeedMov);
        _cc2d.Move(new Vector2(1 * (3 * (1 + _player.Value.SpeedMov) * Time.deltaTime), 0));
        _canFallDown = true;
    }

    private void JumpAction(in KeyInputEventArgs args)
    {
        if (DoubleJump == 0 || JumpWait >= 0)
            return;
        NowMoveY = 10;
        DeltaMoveY = -20;
        DoubleJump --;
        JumpWait = 0.15f;
        //_cc2d.Move(new Vector2(0, 1 * (5 * Time.deltaTime)));
        //_cc2d.IsIgnorePlatform = true;
        //_canFallDown = false;
    }
    private void DashAction(in KeyInputEventArgs args)
    {
        if (DashCoolDown > 0)
            return;
        Dash = true;
        DashTime = 0.15f;
        DashCoolDown = 1;
    }
    private void DownAction(in KeyInputEventArgs args)
    {
        _cc2d.IsIgnorePlatform = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            JumpAction(new KeyInputEventArgs());
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DashAction(new KeyInputEventArgs());
        }
        //这里以上都要修改
        DashCoolDown -= 1 * Time.deltaTime;
        JumpWait -= 1 * Time.deltaTime;

        if (Dash)
        {
            DashTime -= Time.deltaTime;
            _cc2d.Move(new Vector2(_player.GetFaceTo() * (15 * (1 + _player.Value.SpeedMov) * Time.deltaTime), 0));
            if (DashTime <= 0)
            {
                Dash = false;
                DashCoolDown = 1;
            }
        }

        _cc2d.Move(new Vector2(NowMoveX * Time.deltaTime, NowMoveY * Time.deltaTime));
        NowMoveY += DeltaMoveY * Time.deltaTime;
        NowMoveX += DeltaMoveX * Time.deltaTime;
        /*
        if (_canFallDown)
        {
            _cc2d.Move(new Vector2(0, -2 * (5 * Time.deltaTime)));
        }
        */
        if (_cc2d.IsGrounded && JumpWait <= 0)
        {
            _cc2d._GravityEffect = false;
            DeltaMoveY = 0;
            DoubleJump = 2;
        }
        _canFallDown = true;
    }
}