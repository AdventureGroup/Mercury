using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    public GameObject InfoPanel;
    public LivingEntity Follow;
    public Vector3 Offset;

    private PanelAttackInfo _uiIns;

    private void Awake()
    {
        _uiIns = Instantiate(InfoPanel, GameManager.Instance.UICanvas.transform).GetComponent<PanelAttackInfo>();
        Follow.Attacked += OnFollowAttacked;
    }

    private void OnFollowAttacked(LivingEntity from, DamageClass dmg)
    {
        _uiIns.AddInfo(dmg.Type.ToString(), dmg.Damage, dmg.Element.ToString());
    }

    private void Update()
    {
        if (!Follow)
        {
            return;
        }

        var cam = GameManager.Instance.MainCamera;
        _uiIns.transform.position = Follow.transform.position + Offset;
    }

    private void OnDestroy() { Follow.Attacked -= OnFollowAttacked; }
}