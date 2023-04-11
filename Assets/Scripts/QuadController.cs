using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuadController : MonoBehaviour
{
    public QuadsManager.QuadState CurrentState { get; private set; }

    [SerializeField] private Image image;
    private Color32 _redColor;
    private Color32 _greenColor;
    private Color32 _offColor;

    private static readonly QuadsManager.QuadState[] States = { QuadsManager.QuadState.Red, QuadsManager.QuadState.Green, QuadsManager.QuadState.Off };

    public event Action<QuadController> OnClick;

    private void Awake()
    {
        SetState(QuadsManager.QuadState.Off);
    }

    public void Init(Color32 red, Color32 green, Color32 off)
    {
        _redColor = red;
        _greenColor = green;
        _offColor = off;
    }
    
    public void SetState(QuadsManager.QuadState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case QuadsManager.QuadState.Red:
                image.color = _redColor;
                break;
            case QuadsManager.QuadState.Green:
                image.color = _greenColor;
                break;
            case QuadsManager.QuadState.Off:
                image.color = _offColor;
                break;
        }
    }

    public void SetRandomState(bool isOffStateAvailable = true)
    {
        QuadsManager.QuadState[] states;
        if (isOffStateAvailable)
        {
            states = new[] { QuadsManager.QuadState.Red, QuadsManager.QuadState.Green, QuadsManager.QuadState.Off };
        }
        else
        {
            states = new[] { QuadsManager.QuadState.Red, QuadsManager.QuadState.Green };
        }

        var randomIndex = Random.Range(0, states.Length);
        SetState(states[randomIndex]);
    }
    
    public void OnQuadClicked()
    {
        if (CurrentState != QuadsManager.QuadState.Off)
        {
            OnClick?.Invoke(this);
            SetState(QuadsManager.QuadState.Off);
        }
    }
}
