using Common.Scripts.GoogleSheets;
using Managers;
using UI.Panels;
using UnityEngine;
using Zenject;

public class LoadScenesManager : MonoBehaviour
{
    [Inject] private UiManager UiManager;
    
    private void Start()
    {
        Debug.Log("Load Scenes Manager");
        UiManager.OpenPanel(PanelType.LoadingPanel);
    }

    
}
