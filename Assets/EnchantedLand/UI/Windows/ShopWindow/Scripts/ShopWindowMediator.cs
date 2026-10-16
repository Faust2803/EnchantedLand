using Common.Scripts.Tutorial;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Scripts.Others;
using EnchantedLand.Common.Scripts.Potions;
using EnchantedLand.Common.Weapon;
using Tanks.Common.Scripts.Player;
using Zenject;

namespace UI.Windows
{
    public class ShopWindowMediator : BaseWindowMediator<ShopWindowView, UIData>
    {
        private PlayerManager _playerManager;
        private ArmorManager _armorManager;
        private WeaponManager _weaponManager;
        private PotionsManager _potionsManager;
        private OthersEquipmentManager _othersEquipmentManager; 
        private TutorialManager _tutorialManager;

        private int _counter;
        private ItemType _activeType;
        private IIdentifiable _activeItem;

        private int CurrentPrice => _playerManager.Data.CalculatePrice(_activeItem.Price);

        private int CurrentItemCount => _activeType switch
        {
            ItemType.Armor => _armorManager.Count,
            ItemType.Weapon => _weaponManager.Count,
            ItemType.Potions => _potionsManager.Count,
            ItemType.Other => _othersEquipmentManager.Count,
            _ => 0
        };

        [Inject]
        private void Construct(
            PlayerManager playerManager,
            ArmorManager armorManager,
            WeaponManager weaponManager,
            PotionsManager potionsManager,
            OthersEquipmentManager othersEquipmentManager,
            TutorialManager tutorialManager)
        {
            _playerManager = playerManager;
            _armorManager = armorManager;
            _weaponManager = weaponManager;
            _potionsManager = potionsManager;
            _othersEquipmentManager = othersEquipmentManager;
            _tutorialManager = tutorialManager;
        }

        protected override void ShowStart()
        {
            base.ShowStart();
            Subscribe();
            _counter = 0;
            _activeType = ItemType.Armor;
            Target.ShopTabToggle[0].isOn = true;
            UpdatePlayerInfo();
            GetShopItem();
            
            

            Target.InfoPanel.PlayerManager = _playerManager;
            Target.InfoPanel.gameObject.SetActive(false);
        }

        protected override void OpenFinish()
        {
            base.OpenFinish();
            if (_tutorialManager.CurrentTutorialIndex == TutorialStep.ByeInShop )
            {
                _uiManager.ForceOpenWindow(WindowType.TutorialWindow);
            }
        }

        private void Subscribe()
        {
            Target.ByeButton.onClick.AddListener(OnBuyButtonClicked);
            Target.LeftButton.onClick.AddListener(PrevItem);
            Target.RightButton.onClick.AddListener(NextItem);
            foreach (var toggle in Target.ShopTabToggle)
                toggle.onValueChanged.AddListener(OnToggleClicked);
            _playerManager.Data.OnStorageChanged += OnStorageChanged;
            Target.InfoButton.onClick.AddListener(OnInfoClick);
            Target.FinishButton.onClick.AddListener(OnFinishClick);
        }

        private void OnFinishClick()
        {
            _uiManager.OpenWindow(WindowType.PlayerUIWindow);
            CloseSelf();
        }

        private void OnInfoClick()
        {
            Target.InfoPanel.SetInfo(_activeItem);
        }

        protected override void CloseStart()
        {
            Unsubscribe();
            base.CloseStart();
        }

        private void Unsubscribe()
        {
            Target.ByeButton.onClick.RemoveListener(OnBuyButtonClicked);
            Target.LeftButton.onClick.RemoveListener(PrevItem);
            Target.RightButton.onClick.RemoveListener(NextItem);
            foreach (var toggle in Target.ShopTabToggle)
                toggle.onValueChanged.RemoveListener(OnToggleClicked);
            _playerManager.Data.OnStorageChanged -= OnStorageChanged;
            Target.InfoButton.onClick.RemoveListener(OnInfoClick);
            Target.FinishButton.onClick.RemoveListener(OnFinishClick);
        }

        private void OnBuyButtonClicked()
        {
            if (_activeItem == null) return;
            if (!TryBuyItem()) return;
            UpdatePlayerInfo();
            _playerManager.Save();
        }

        private bool TryBuyItem()
        {
            if (_playerManager.Data.Currency < CurrentPrice)
            {
                ShowMessage(
                    "Нехватает денег",
                    $"Ты не можешь купить этот предмет. Тебе не хватает {CurrentPrice - _playerManager.Data.Currency} монет.");
                return false;
            }
            if (!_playerManager.Data.AddToStorage(_activeItem))
            {
                ShowMessage(
                    "Невозможно купить",
                    "Ты не можешь положить этот предмет в хранилище!\nХранилище полностью заполнено или не хватает грузоподъёмности.");
                return false;
            }
            return true;
        }

        private void ShowMessage(string title, string description)
        {
            var data = new MessageWindowData
            {
                Title = title,
                Description = description,
                HasCancelButton = false,
                HasOKButton = true,
                OkButtonText = "Продолжить игру"
            };
            _uiManager.ForceOpenWindow(WindowType.MessageWindow, data);
        }

        private void OnStorageChanged(int index, IIdentifiable item, bool isAded)
        {
            if (!isAded)
            {
                var price =  (item.Price / 2 ) - (_playerManager.Data.CalculatePrice(item.Price) - item.Price);
                _playerManager.Data.Currency += price;
                
                ShowMessage(
                    "Предмет продан!",
                    $"Вы продали {item.Title} за {price} монет.");
            }
            else
            {
                _playerManager.Data.Currency -= CurrentPrice;
            }
            UpdatePlayerInfo();
            _playerManager.Save();
        }

        private void NextItem()
        {
            ChangeItem(1);
        }

        private void PrevItem()
        {
            ChangeItem(-1);
        }

        private void ChangeItem(int direction)
        {
            var count = CurrentItemCount;
            if (count == 0) return;
            _counter += direction;

            if (_counter >= count) _counter = 0;
            if (_counter < 0) _counter = count - 1;
            
            GetShopItem();
        }

        private void OnToggleClicked(bool isOn)
        {
            if (!isOn) return;

            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                if (!Target.ShopTabToggle[i].isOn) continue;

                _activeType = (ItemType)i;
                _counter = 0;

                GetShopItem();
                return;
            }
        }

        private void GetShopItem()
        {
            var count = CurrentItemCount;
            var hasOne = false;
            if (count > 0)
            {
                // Ищем следующий предмет, который есть в магазине.
                for (var i = 0; i < count; i++)
                {
                    _counter %= count;
                    IIdentifiable item = GetItem(_counter);
                    if (item != null && item.InShop)
                    {
                        _activeItem = item;
                        ShowItem();
                        hasOne = true; 
                        return;
                    }
                    _counter++;
                }
                _activeItem = null;
            }
            if (!hasOne)
            {
                ShowMessage(
                    "Невозможно купить",
                    "Этот раздел магазина сейчас пуст!\nУ торговца нет товаров данного типа");
                Target.ShopTabToggle[0].isOn = true;
            }
            
        }

        private IIdentifiable GetItem(int index)
        {
            return _activeType switch
            {
                ItemType.Armor => _armorManager.GetByIndex(index),
                ItemType.Weapon => _weaponManager.GetByIndex(index),
                ItemType.Potions => _potionsManager.GetByIndex(index),
                ItemType.Other => _othersEquipmentManager.GetByIndex(index),
                _ => null
            };
        }

        private void ShowItem()
        {
            if (_activeItem == null)
                return;

            Target.TitleText = _activeItem.Title;
            Target.DescriptionText = _activeItem.Description;
            Target.ItemImage = _activeItem.PreviewImage;
            Target.PriceText = CurrentPrice.ToString();
            Target.WeightItemText = _activeItem.Weight.ToString();
        }

        private void UpdatePlayerInfo()
        {
            Target.WeightText = _playerManager.Data.AvailableWeight;
            Target.MoneyText = _playerManager.Data.Currency;
        }
    }
}