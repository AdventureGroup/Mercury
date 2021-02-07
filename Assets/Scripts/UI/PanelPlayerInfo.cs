using UnityEngine;
using UnityEngine.UI;

public class PanelPlayerInfo : MonoBehaviour
{
    public Slider HpSlider;
    public Text HpNow;
    public Text HpMax;

    public Slider MpSlider;
    public Text MpNow;
    public Text MpMax;

    private Role _hold;

    private void Update()
    {
        if (_hold)
        {
            HpSlider.maxValue = _hold.Value.HealthMax;
            HpMax.text = Mathf.Round(_hold.Value.HealthMax).ToString();
            HpSlider.value = _hold.Health;
            HpNow.text = Mathf.Round(_hold.Health).ToString();

            MpSlider.maxValue = _hold.Value.ManaMax;
            MpSlider.value = _hold.Mana;
            MpMax.text = Mathf.Round(_hold.Value.ManaMax).ToString();
            MpNow.text = Mathf.Round(_hold.Mana).ToString();
        }
        else
        {
            Debug.LogWarning("未设置玩家，但玩家信息UI在活动");
        }
    }

    public void SetPlayer(Role player) { _hold = player; }
}