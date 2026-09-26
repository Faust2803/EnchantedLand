using System.Collections.Generic;
using Common.Scripts.Network;
using Managers;
using UI.Panels;
using UnityEngine;
using Zenject;

namespace Tanks.Lobby.Scripts
{
    public class LobbyScenesManager : MonoBehaviour
    {
        [Inject] private UiManager UiManager;

        void Start()
        {


            UiManager.OpenPanel(
                PanelType.LobbyMainPanel,
                new LobbyMainPanelData
                {
                    
                });

        }

    }
}
