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