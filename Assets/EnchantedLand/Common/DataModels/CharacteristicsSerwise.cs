using System.Collections.Generic;
using System.Text;
using EnchantedLand.Common.Damage;

namespace EnchantedLand.Common.DataModels
{
    public static class CharacteristicsServise
    {
        public static StringBuilder Characteristics(Dictionary<CharacteristicsID, int> characteristics, bool all = false)
        {
            var builder = new StringBuilder();
            if (characteristics[CharacteristicsID.Strength] > 0 || all)
            {
                builder.AppendLine("Сила: " + characteristics[CharacteristicsID.Strength]);
            }
            if (characteristics[CharacteristicsID.Agility] > 0 || all)
            {
                builder.AppendLine("Ловкость: " + characteristics[CharacteristicsID.Agility]);
            }
            if (characteristics[CharacteristicsID.Endurance] > 0 || all)
            {
                builder.AppendLine("Выносливость: " + characteristics[CharacteristicsID.Endurance]);
            }
            if (characteristics[CharacteristicsID.Intelligence] > 0 || all)
            {
                builder.AppendLine("Интеллект: " + characteristics[CharacteristicsID.Intelligence]);
            }
            if (characteristics[CharacteristicsID.Charisma] > 0 || all)
            {
                builder.AppendLine("Харизма: " + characteristics[CharacteristicsID.Charisma]);
            }
            return builder;
        }
        
        public static StringBuilder Skills(Dictionary<SkillsID, int> skills, bool all = false, Dictionary<SkillsID, int> addSkills = null)
        {
            var builder = new StringBuilder();
            if (skills[SkillsID.Armor] > 0 || all)
            {
                if (addSkills != null)
                {
                    builder.AppendLine("Броня: " + (skills[SkillsID.Armor] + addSkills[SkillsID.Armor]));
                }
                else
                {
                    builder.AppendLine("Броня: " + skills[SkillsID.Armor] );
                }
            }
            if (skills[SkillsID.Accuracy] > 0 || all)
            {
                if (addSkills != null)
                {
                    builder.AppendLine("Точность: " + (skills[SkillsID.Accuracy] + addSkills[SkillsID.Accuracy]));
                }
                else
                {
                    builder.AppendLine("Точность: " + skills[SkillsID.Accuracy]);
                }
            }
            if (skills[SkillsID.Destruction] > 0 || all)
            {
                if (addSkills != null)
                {
                    builder.AppendLine("Урон: " + (skills[SkillsID.Destruction] + addSkills[SkillsID.Destruction]));
                }
                else
                {
                    builder.AppendLine("Урон: " + skills[SkillsID.Destruction]);
                }
                
            }
            if (skills[SkillsID.Initiative] > 0 || all)
            {
                if (addSkills != null)
                {
                    builder.AppendLine("Инициатива: " + (skills[SkillsID.Initiative] + addSkills[SkillsID.Initiative]));
                }
                else
                {
                    builder.AppendLine("Инициатива: " + skills[SkillsID.Initiative]);
                }
            }
            if (skills[SkillsID.Stealth] > 0 || all)
            {
                if (addSkills != null)
                {
                    builder.AppendLine("Скрытность: " + (skills[SkillsID.Initiative] + addSkills[SkillsID.Initiative]));
                }
                else
                {
                    builder.AppendLine("Скрытность: " + skills[SkillsID.Stealth]);
                    
                }
            }
            return builder;
        }
        
        public static StringBuilder Resistance(Dictionary<DamageType, float> resistance, bool all = false, float allResistance =0)
        {
            var builder = new StringBuilder();
            if (resistance.ContainsKey(DamageType.Physical)  && (resistance[DamageType.Physical] > 0 || all))
            {
                builder.AppendLine("Физическое: " + (resistance[DamageType.Physical] +allResistance));
            }
            if (resistance.ContainsKey(DamageType.Fire)  && (resistance[DamageType.Fire] > 0 || all))
            {
                builder.AppendLine("Огонь: " + (resistance[DamageType.Fire]+allResistance));
            }
            if (resistance.ContainsKey(DamageType.Ice)  && (resistance[DamageType.Ice] > 0 || all))
            {
                builder.AppendLine("Лед: " + (resistance[DamageType.Ice]+allResistance));
            }
            if (resistance.ContainsKey(DamageType.Acid)  && (resistance[DamageType.Acid] > 0 || all))
            {
                builder.AppendLine("Кислота: " + (resistance[DamageType.Acid]+allResistance));
            }
            if (resistance.ContainsKey(DamageType.Electricity)  && (resistance[DamageType.Electricity] > 0 || all))
            {
                builder.AppendLine("Электричество: " + (resistance[DamageType.Electricity]+allResistance));
            }
            if (resistance.ContainsKey(DamageType.Paralysis)  && (resistance[DamageType.Paralysis] > 0 || all))
            {
                builder.AppendLine("Оглушение: " + (resistance[DamageType.Paralysis]+allResistance));
            }
            if (resistance.ContainsKey(DamageType.Death)  && (resistance[DamageType.Death] > 0 || all))
            {
                builder.AppendLine("Смерть: " + (resistance[DamageType.Death]+allResistance));
            }
            return builder;
        }

        public static StringBuilder Damage(List<Damage.Damage> damage )
        {
            var builder = new StringBuilder();
            for (var i = 0; i < damage.Count; i++)
            {
                builder.AppendLine($"{damage[i].Title}");
                if (damage[i].Type ==  DamageType.Physical)
                {
                    builder.AppendLine($"   Урон Физический {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Acid)
                {
                    builder.AppendLine($"   Урон Кислота {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Acid)
                {
                    builder.AppendLine($"   Урон Кислота {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Death)
                {
                    builder.AppendLine($"   Урон Смерть");
                }
                if (damage[i].Type ==  DamageType.Electricity)
                {
                    builder.AppendLine($"   Урон Электричество {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Fire)
                {
                    builder.AppendLine($"   Урон Огонь {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Ice)
                {
                    builder.AppendLine($"   Урон Лед {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Paralysis)
                {
                    builder.AppendLine($"   Урон Оглушение {damage[i].Min} - {damage[i].Max}");
                }
                if (damage[i].Type ==  DamageType.Оther)
                {
                    builder.AppendLine($"   Урон Специальный {damage[i].Min} - {damage[i].Max}");
                }
                
                if (damage[i].Opposition > 0)
                {
                    builder.AppendLine($"   Противодействие: {damage[i].Opposition}");
                }
                
                if (damage[i].Radius > 0 )
                {
                    builder.AppendLine($"   Массовый урон радиус поражени: {damage[i].Radius}м. ");
                }
                
            }
            return builder;
        }
    }
}