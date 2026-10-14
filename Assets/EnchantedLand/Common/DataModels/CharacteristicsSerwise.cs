using System.Collections.Generic;
using System.Text;
using EnchantedLand.Common.Damage;

namespace EnchantedLand.Common.DataModels
{
    public static class CharacteristicsServise
    {
        public static StringBuilder Characteristics(Characteristics characteristics)
        {
            var builder = new StringBuilder();
            if (characteristics.Strength > 0 )
            {
                builder.AppendLine("Сила: " + characteristics.Strength);
            }
            if (characteristics.Agility > 0 )
            {
                builder.AppendLine("Ловкость: " + characteristics.Agility);
            }
            if (characteristics.Endurance > 0 )
            {
                builder.AppendLine("Выносливость: " + characteristics.Endurance);
            }
            if (characteristics.Intelligence > 0 )
            {
                builder.AppendLine("Интеллект: " + characteristics.Intelligence);
            }
            if (characteristics.Charisma > 0 )
            {
                builder.AppendLine("Харизма: " + characteristics.Charisma);
            }
            return builder;
        }
        
        public static StringBuilder Skills(Skills skills)
        {
            var builder = new StringBuilder();
            if (skills.Armor > 0 )
            {
                builder.AppendLine("Броня: " + skills.Armor);
            }
            if (skills.Accuracy > 0 )
            {
                builder.AppendLine("Точность: " + skills.Accuracy);
            }
            if (skills.Destruction > 0 )
            {
                builder.AppendLine("Урон: " + skills.Destruction);
            }
            if (skills.Initiative > 0 )
            {
                builder.AppendLine("Инициатива: " + skills.Initiative);
            }
            if (skills.Stealth > 0 )
            {
                builder.AppendLine("Скрытность: " + skills.Stealth);
            }
            return builder;
        }
        
        public static StringBuilder Resistance(Dictionary<DamageType, float> resistance)
        {
            var builder = new StringBuilder();
            if (resistance.ContainsKey(DamageType.Physical)  && resistance[DamageType.Physical] > 0 )
            {
                builder.AppendLine("Физическое: " + resistance[DamageType.Physical]);
            }
            if (resistance.ContainsKey(DamageType.Fire)  && resistance[DamageType.Fire] > 0 )
            {
                builder.AppendLine("Огонь: " + resistance[DamageType.Fire]);
            }
            if (resistance.ContainsKey(DamageType.Ice)  && resistance[DamageType.Ice] > 0 )
            {
                builder.AppendLine("Лед: " + resistance[DamageType.Ice]);
            }
            if (resistance.ContainsKey(DamageType.Acid)  && resistance[DamageType.Acid] > 0 )
            {
                builder.AppendLine("Кислота: " + resistance[DamageType.Acid]);
            }
            if (resistance.ContainsKey(DamageType.Electricity)  && resistance[DamageType.Electricity] > 0 )
            {
                builder.AppendLine("Электричество: " + resistance[DamageType.Electricity]);
            }
            if (resistance.ContainsKey(DamageType.Paralysis)  && resistance[DamageType.Paralysis] > 0 )
            {
                builder.AppendLine("Оглушение: " + resistance[DamageType.Paralysis]);
            }
            if (resistance.ContainsKey(DamageType.Death)  && resistance[DamageType.Death] > 0 )
            {
                builder.AppendLine("Смерть: " + resistance[DamageType.Death]);
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