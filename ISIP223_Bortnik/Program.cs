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