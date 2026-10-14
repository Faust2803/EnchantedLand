using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EnchantedLand.Common.Prefabs.UI.DescriptionScroll
{
    public class DescriptionScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        private void OnEnable()
        {
            _scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}