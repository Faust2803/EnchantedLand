using System;
using System.Text;
using Common.Scripts.Utils;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts.Potions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EnchantedLand.Common.Scripts
{
    public class StorageItem : MonoBehaviour
    { 
        [SerializeField] private int _ID;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private Image _image;
        [SerializeField] private DoubleClickButton _button;
        
        [Header("Tooltip")]
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        
        protected IIdentifiable _itemData;
        public bool ItemData => _itemData != null;
        public Action<int, IIdentifiable> OnDoubleClick;
        public int ID => _ID;

        public virtual void AddItem(IIdentifiable itemData)
        {
            _itemData = itemData;
            _counter.text = itemData.Count.ToString();
            _image.sprite = itemData.StorageImage;
            _button.OnDoubleClick += OnButtonClicked;
            SetActive(true);
            _counter.gameObject.SetActive(itemData.Count > 1);
            SetTooltipInfo();
        }

        public virtual void RemoveItem()
        {
            _itemData = null;
            _button.OnDoubleClick -= OnButtonClicked;
            SetActive(false);
        }

        private void SetActive(bool value)
        {
            _counter.gameObject.SetActive(value);
            _button.gameObject.SetActive(value);
        }

        private void OnButtonClicked()
        {
            OnDoubleClick?.Invoke(_ID, _itemData);
        }


        private void SetTooltipInfo()
        {
            _title.text = _itemData.Title;
            //_description.text = _itemData.Description;
            var builder = new StringBuilder();
            switch (_itemData.Type)
            {
                case ItemType.Armor:
                    var armor  = _itemData as Armor.Armor;
                    var nch = CharacteristicsServise.Characteristics(armor.NeededCharacteristics);
                    if (nch.Length > 0)
                    {
                        builder.Append($"НЕОБХОДИМЫЕ ХАРАКТЕРИСТИКИ\n{nch.ToString()}");
                    }
                    var ich = CharacteristicsServise.Characteristics(armor.InfluenceCharacteristics);
                    if (ich.Length > 0)
                    {
                        builder.Append($"ВЛИЯЕТ НА\n{ich.ToString()}");
                    }
                    var res = CharacteristicsServise.Resistance(armor.Resistance);
                    if (res.Length > 0)
                    {
                        builder.Append($"СОПРОТИВЛЕНИЕ\n{res.ToString()}");
                    }
                    var ski = CharacteristicsServise.Skills(armor.Skills);
                    if (ski.Length > 0)
                    {
                        builder.Append($"НАВЫКИ\n{ski.ToString()}");
                    }
                    builder.Append($"ПРОЧНОСТЬ: {armor.MaxStrength}/{armor.CurrentStrength}");
                    break;
                
                case ItemType.Weapon:
                    var weapon  = _itemData as Weapon.Weapon;
                    
                    if (weapon.Projectile)
                    {
                        builder.Append($"\nТИП ОРУЖИЯ:\n Снаряд, {weapon.Count} штук\n\n");
                    }
                    else
                    {
                        builder.Append($"\nТИП ОРУЖИЯ: \n");
                        builder.Append(weapon.TwoHands ? $"Двуручное " : $"Одноручное ");
                        if (weapon.Distance > 2)
                        {
                            builder.Append($", дистанционное\n\n");
                        }
                        else
                        {
                            builder.Append(weapon.Distance == 1 ? $", ближнего действия\n\n" : $", длинное\n\n");
                        }
                        
                    }
                    
                    var wnch = CharacteristicsServise.Characteristics(weapon.NeededCharacteristics);
                    if (wnch.Length > 0)
                    {
                        builder.Append($"НЕОБХОДИМЫЕ ХАРАКТЕРИСТИКИ\n{wnch.ToString()}");
                    }
                    var wich = CharacteristicsServise.Characteristics(weapon.InfluenceCharacteristics);
                    if (wich.Length > 0)
                    {
                        builder.Append($"ВЛИЯЕТ НА\n{wich.ToString()}");
                    }
                    var wski = CharacteristicsServise.Skills(weapon.Skills);
                    if (wski.Length > 0)
                    {
                        builder.Append($"НАВЫКИ\n{wski.ToString()}");
                    }
                    var dam = CharacteristicsServise.Damage(weapon.Damage);
                    if (dam.Length > 0)
                    {
                        builder.Append($"УРОН\n{dam.ToString()}");
                    }
                    builder.Append($"ПРОЧНОСТЬ: {weapon.MaxStrength}/{weapon.CurrentStrength}");
                    break;
                case ItemType.Potions:
                    var potion  = _itemData as Potions.Potions;
                    if (potion.SubType == PotionsType.HP)
                    {
                        builder.AppendLine($"Использование прибавляет {potion.Impact} HP, но не больше максимума");
                    }
                    else
                    {
                        builder.AppendLine($"Использование прибавляет {potion.Impact} указанной характеристики до следующего хода");
                    }
                    break;
                case ItemType.Other:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _description.text = builder.ToString();
        }
        
    }
}