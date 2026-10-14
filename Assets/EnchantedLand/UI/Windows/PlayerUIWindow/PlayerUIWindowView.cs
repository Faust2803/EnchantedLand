using System.Collections.Generic;
using EnchantedLand.Common.Scripts;
using EnchantedLand.UI.PlauerUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class PlayerUIWindowView : BaseView
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private ActiveEquipmentUI _activeEquipmentUI;
        
        public Storage Storage => _storage;
        public ActiveEquipmentUI ActiveEquipmentUI => _activeEquipmentUI;
        
        protected override BaseMediator CreateMediator()
        {
            _mediator = new PlayerUIMediator();
            return _mediator;
        }
    }
}