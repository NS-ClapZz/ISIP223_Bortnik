using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    // Перечисление для типов врагов
    public enum EnemyType
    {
        Goblin,
        Skeleton,
        Mage
    }

    // Перечисление для действий игрока в бою
    public enum CombatAction
    {
        Attack,
        Defend
    }

    // Базовый класс для всех предметов
    public abstract class Item
    {
        public string Name { get; protected set; }

        public Item(string name)
        {
            Name = name;
        }

        public abstract void Use(Player player);

        public abstract string GetStats();
    }

    // Класс оружия
    public class Weapon : Item
    {
        public int Attack { get; private set; }

        public Weapon(string name, int attack) : base(name)
        {
            Attack = attack;
        }

        public override void Use(Player player)
        {
            player.EquipWeapon(this);
        }

        public override string GetStats()
        {
            return $"{Name} (Атака: {Attack})";
        }
    }

    // Класс доспехов
    public class Armor : Item
    {
        public int Defense { get; private set; }

        public Armor(string name, int defense) : base(name)
        {
            Defense = defense;
        }

        public override void Use(Player player)
        {
            player.EquipArmor(this);
        }

        public override string GetStats()
        {
            return $"{Name} (Защита: {Defense})";
        }
    }

    // Класс лечебного зелья
    public class Potion : Item
    {
        public Potion() : base("Лечебное зелье") { }

        public override void Use(Player player)
        {
            player.Heal();
            Console.WriteLine("Вы выпили лечебное зелье и полностью восстановили здоровье!");
        }

        public override string GetStats()
        {
            return $"{Name} (Полное восстановление здоровья)";
        }
    }

    // Класс игрока
    public class Player
    {
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public bool IsFrozen { get; set; }

        public Player(int maxHP)
        {
            MaxHP = maxHP;
            CurrentHP = maxHP;
            // Стартовое снаряжение
            CurrentWeapon = new Weapon("Ржавый меч", 5);
            CurrentArmor = new Armor("Простая броня", 3);
            IsFrozen = false;
        }

        public int GetAttack()
        {
            return CurrentWeapon?.Attack ?? 0;
        }

        public int GetDefense()
        {
            return CurrentArmor?.Defense ?? 0;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void Heal()
        {
            CurrentHP = MaxHP;
        }

        public bool IsAlive()
        {
            return CurrentHP > 0;
        }

        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            Console.WriteLine($"Вы экипировали: {weapon.GetStats()}");
        }

        public void EquipArmor(Armor armor)
        {
            CurrentArmor = armor;
            Console.WriteLine($"Вы экипировали: {armor.GetStats()}");
        }

        public void DisplayStats()
        {
            Console.WriteLine($"=== ИГРОК ===");
            Console.WriteLine($"Здоровье: {CurrentHP}/{MaxHP}");
            Console.WriteLine($"Оружие: {CurrentWeapon?.GetStats() ?? "Нет"}");
            Console.WriteLine($"Броня: {CurrentArmor?.GetStats() ?? "Нет"}");
            Console.WriteLine($"Общая атака: {GetAttack()}");
            Console.WriteLine($"Общая защита: {GetDefense()}");
            Console.WriteLine("=================");
        }
    }