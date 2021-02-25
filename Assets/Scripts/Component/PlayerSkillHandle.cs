using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandle : MonoBehaviour
{
    private List<Skill> _skillList;

    private void Start()
    {
        _skillList = new List<Skill>(GetComponents<Skill>());
        var input = GameManager.Instance.Input;
        foreach (var skill in _skillList)
        {
            switch (skill.SkillType)
            {
                case SkillType.Normal:
                    input.AddDelegation(InputType.NormalAttack.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.Slot1:
                    input.AddDelegation(InputType.Skill1.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.Slot2:
                    input.AddDelegation(InputType.Skill2.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.Ultimate:
                    input.AddDelegation(InputType.Ultimate.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void OnDestroy()
    {
        var input = GameManager.Instance.Input;
        foreach (var skill in _skillList)
        {
            switch (skill.SkillType)
            {
                case SkillType.Normal:
                    input.RemoveDelegation(InputType.NormalAttack.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.Slot1:
                    input.RemoveDelegation(InputType.Skill1.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.Slot2:
                    input.RemoveDelegation(InputType.Skill2.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.Ultimate:
                    input.RemoveDelegation(InputType.Ultimate.ToString(), skill.PlayerRequestToUse);
                    break;
                case SkillType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}