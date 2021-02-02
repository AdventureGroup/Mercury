using UnityEngine;

public class ScreenCanvas : MonoBehaviour
{
    public Transform LeftUpPivot;
    public Transform RightDownPivot;

    public Canvas BaseCanvas { get; private set; }

    private void Awake() { BaseCanvas = GetComponent<Canvas>(); }
}