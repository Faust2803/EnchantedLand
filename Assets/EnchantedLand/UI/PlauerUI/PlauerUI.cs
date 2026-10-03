using SO.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EnchantedLand.UI.PlauerUI
{
    public class PlauerUI : MonoBehaviour
    {
        [FormerlySerializedAs("SlotId")] [SerializeField] private EquipmentSlotID slotSlotID;
        [SerializeField] private Image PlauerUIImage;
    }
}