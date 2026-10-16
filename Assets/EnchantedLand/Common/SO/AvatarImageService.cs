using System;
using System.Collections.Generic;
using EnchantedLand.Common.Race;
using UnityEngine;
using UnityEngine.Serialization;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "AvatarImageService", menuName = "Configs/Game/SO Avatar Image Service ", order = 1)]
    public class AvatarImageService : ScriptableObject
    {
        [SerializeField] private  List<Avatar> _avatarList = null;
        [SerializeField] private  List<EquipmentsImage> _weaponsList = null;
        [SerializeField] private  List<EquipmentsImage> _armorsList = null;
        [SerializeField] private  List<Avatar> _enemyes = null;
        
        public Avatar GeAvatar(int type)
        {
            for (var i = 0; i < _avatarList.Count; i++)
            {
                if (_avatarList[i].Id == type)
                {
                    return _avatarList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>Avatar Image: Avatar not found!!!</color>");
            return null;
        }
        
        public EquipmentsImage GeWeapon(int type)
        {
            for (var i = 0; i < _weaponsList.Count; i++)
            {
                if (_weaponsList[i].Id == type)
                {
                    return _weaponsList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>Weapons Image: Weapon not found!!!</color>");
            return null;
        }
        
        public EquipmentsImage GetArmor(int type)
        {
            for (var i = 0; i < _armorsList.Count; i++)
            {
                if (_armorsList[i].Id == type)
                {
                    return _armorsList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>Armors Image: Armor not found!!!</color>");
            return null;
        }
        
        public Avatar GetEnemy(int type)
        {
            for (var i = 0; i < _enemyes.Count; i++)
            {
                if (_enemyes[i].Id == type)
                {
                    return _enemyes[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>Enemy Image: Armor not found!!!</color>");
            return null;
        }
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