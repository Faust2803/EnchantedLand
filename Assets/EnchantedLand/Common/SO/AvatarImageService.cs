using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "AvatarImageService", menuName = "Configs/Game/SO Avatar Image Service ", order = 1)]
    public class AvatarImageService : ScriptableObject
    {
        [Header("Avatars")]
        [SerializeField] private List<Avatar> _avatarList = new();

        [Header("Equipment")]
        [SerializeField] private List<EquipmentsImage> _weaponsList = new();
        [SerializeField] private List<EquipmentsImage> _armorsList = new();
        [SerializeField] private List<EquipmentsImage> _potions = new();
        [SerializeField] private List<EquipmentsImage> _otherEquipment = new();

        [Header("Enemies")]
        [SerializeField] private List<Avatar> _enemyes = new();

        private Dictionary<int, Avatar> _avatars;
        private Dictionary<int, EquipmentsImage> _weapons;
        private Dictionary<int, EquipmentsImage> _armors;
        private Dictionary<int, EquipmentsImage> _potionsLookup;
        private Dictionary<int, EquipmentsImage> _otherEquipmentLookup;
        private Dictionary<int, Avatar> _enemies;

        private void OnEnable()
        {
            RebuildLookups();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateIds(_avatarList);
            UpdateIds(_weaponsList);
            UpdateIds(_armorsList);
            UpdateIds(_potions);
            UpdateIds(_otherEquipment);
            UpdateIds(_enemyes);
            
            RebuildLookups();
        }
#endif
        
        private void UpdateIds<T>(List<T> items) where T : class
        {
            if (items == null) return;

            for (var i = 0; i < items.Count; i++)
            {
                if (items[i] is Avatar avatar)
                    avatar.Id = i;
                else if (items[i] is EquipmentsImage equipment)
                    equipment.Id = i;
            }
        }

        private void RebuildLookups()
        {
            _avatars = BuildLookup(_avatarList, x => x.Id, "Avatar");
            _weapons = BuildLookup(_weaponsList, x => x.Id, "Weapon");
            _armors = BuildLookup(_armorsList, x => x.Id, "Armor");
            _potionsLookup = BuildLookup(_potions, x => x.Id, "Potion");
            _otherEquipmentLookup = BuildLookup(_otherEquipment, x => x.Id, "Other equipment");
            _enemies = BuildLookup(_enemyes, x => x.Id, "Enemy");
        }

        private Dictionary<int, T> BuildLookup<T>(List<T> items, Func<T, int> getId, string category) where T : class
        {
            var result = new Dictionary<int, T>();
            if (items == null) return result;
            foreach (var item in items)
            {
                if (item == null) continue;
                var id = getId(item);
                if (!result.TryAdd(id, item))
                {
                    Debug.LogWarning($"{category}: duplicate ID {id}.");
                }
            }
            return result;
        }

        private T GetById<T>(Dictionary<int, T> lookup, int id, string category) where T : class
        {
            if (lookup != null && lookup.TryGetValue(id, out var item)) return item;
            Debug.LogWarning($"{category}: item with ID {id} not found.");
            return null;
        }
        
        public Avatar GeAvatar(int id) => GetById(_avatars, id, "Avatar");
        public EquipmentsImage GeWeapon(int id) => GetById(_weapons, id, "Weapon");
        public EquipmentsImage GetArmor(int id) => GetById(_armors, id, "Armor");
        public EquipmentsImage GetOtherEquipment(int id) => GetById(_otherEquipmentLookup, id, "Other equipment");
        public EquipmentsImage GetPotions(int id) => GetById(_potionsLookup, id, "Potion");
        public Avatar GetEnemy(int id) => GetById(_enemies, id, "Enemy");
    }
    
    [Serializable]
    public class Avatar
    {
        public int Id;
        public Sprite Image;
    }
    
    [Serializable]
    public class EquipmentsImage
    {
        public int Id;
        public Sprite PreviewImage;
        public Sprite StorageImage;
        public Sprite EquipedImage;
    }
}