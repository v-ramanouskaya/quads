using UnityEngine;

public class QuadsManager : MonoBehaviour
{
    public enum QuadState
    {
        Red,
        Green,
        Off
    }
    
    [SerializeField] private Color32 redColor;
    [SerializeField] private Color32 greenColor;
    [SerializeField] private Color32 offColor;
    [SerializeField] private QuadController[] quads;
    [SerializeField] private QuadController targetQuad;

    public event System.Action<bool> UpdateScore;

    public void Awake()
    {
        foreach (var quad in quads)
        {
            quad.Init(redColor, greenColor, offColor);
            quad.OnClick += OnQuadClicked;
        }
        
        targetQuad.Init(redColor, greenColor, offColor);
    }
    
    private void OnQuadClicked(QuadController quad)
    {
        UpdateScore?.Invoke(quad.CurrentState == targetQuad.CurrentState);
    }

    public void ResetQuads()
    {
        foreach (var quad in quads)
        {
            quad.SetState(QuadState.Off);
        }
    }

    public void UpdateQuads()
    {
        foreach (var quad in quads)
        {
            quad.SetRandomState();
        }
        targetQuad.SetRandomState(false);
    }
}
