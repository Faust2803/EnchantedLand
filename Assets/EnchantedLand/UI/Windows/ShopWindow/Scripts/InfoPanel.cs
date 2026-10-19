using System;
using System.Text;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Scripts.Potions;
using EnchantedLand.Common.Weapon;
using Tanks.Common.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _description;
        [SerializeField] TextMeshProUGUI _prise;
        [SerializeField] TextMeshProUGUI _weight;
        [SerializeField] Button _closeButton;
        [SerializeField] Image _image;
        
        public PlayerManager PlayerManager { get; set; }

       
        public void SetInfo(IIdentifiable info)
        {
            var builder = new StringBuilder();
            switch (info.Type)
            {
                case ItemType.Armor:
                    var armor  = info as Armor;
                    var nch = CharacteristicsServise.Characteristics(armor.NeededCharacteristics);
                    if (nch.Length > 0)
                    {
                        builder.AppendLine($"НЕОБХОДИМЫЕ ХАРАКТЕРИСТИКИ\n{nch.ToString()}");
                    }
                    var ich = CharacteristicsServise.Characteristics(armor.InfluenceCharacteristics);
                    if (ich.Length > 0)
                    {
                        builder.AppendLine($"ВЛИЯЕТ НА\n{ich.ToString()}");
                    }
                    var res = CharacteristicsServise.Resistance(armor.Resistance);
                    if (res.Length > 0)
                    {
                        builder.AppendLine($"СОПРОТИВЛЕНИЕ\n{res.ToString()}");
                    }
                    var ski = CharacteristicsServise.Skills(armor.Skills);
                    if (ski.Length > 0)
                    {
                        builder.AppendLine($"НАВЫКИ\n{ski.ToString()}");
                    }
                    builder.AppendLine($"ПРОЧНОСТЬ: {armor.MaxStrength}/{armor.CurrentStrength}");
                    break;
                
                case ItemType.Weapon:
                    var weapon  = info as Weapon;
                    
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
                        builder.AppendLine($"НЕОБХОДИМЫЕ ХАРАКТЕРИСТИКИ\n{wnch.ToString()}");
                    }
                    var wich = CharacteristicsServise.Characteristics(weapon.InfluenceCharacteristics);
                    if (wich.Length > 0)
                    {
                        builder.AppendLine($"ВЛИЯЕТ НА\n{wich.ToString()}");
                    }
                    var wski = CharacteristicsServise.Skills(weapon.Skills);
                    if (wski.Length > 0)
                    {
                        builder.AppendLine($"НАВЫКИ\n{wski.ToString()}");
                    }
                    var dam = CharacteristicsServise.Damage(weapon.Damage);
                    if (dam.Length > 0)
                    {
                        builder.AppendLine($"УРОН\n{dam.ToString()}");
                    }
                    builder.AppendLine($"ПРОЧНОСТЬ: {weapon.MaxStrength}/{weapon.CurrentStrength}");
                    break;
                case ItemType.Potions:
                    var potion  = info as Potions;
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
            
            _title.text = info.Title;
            _description.text = $"{info.Description}\n\n{builder.ToString()}";
            _prise.text = PlayerManager.CalculatePrice(info.Price)+ "монет";
            _weight.text = (info.Weight) + "кг";
            _image.sprite = info.PreviewImage;
            
            gameObject.SetActive(true);
            
        }
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }
}