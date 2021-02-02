using UnityEngine;

public class ShowPlayerInfo : MonoBehaviour
{
    public GameObject InfoPanel;

    private PanelPlayerInfo _infoPanel;

    public void Show()
    {
        _infoPanel = Instantiate(InfoPanel, GameManager.Instance.MainCanvas.RightDownPivot)
            .GetComponent<PanelPlayerInfo>();
        _infoPanel.SetPlayer(GetComponent<Role>());
    }

    private void OnDestroy()
    {
        if (_infoPanel)
        {
            Destroy(_infoPanel.gameObject);
        }
    }
}