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

    // Базовый класс врага
    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int MaxHP { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        public bool IsBoss { get; protected set; }

        protected Random random;

        public Enemy(string name, int maxHP, int attack, int defense, bool isBoss = false)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Attack = attack;
            Defense = defense;
            IsBoss = isBoss;
            random = new Random();
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public bool IsAlive()
        {
            return CurrentHP > 0;
        }

        public abstract int PerformAttack(Player player);

        public virtual void DisplayStats()
        {
            string bossPrefix = IsBoss ? "БОСС - " : "";
            Console.WriteLine($"=== {bossPrefix}{Name.ToUpper()} ===");
            Console.WriteLine($"Здоровье: {CurrentHP}/{MaxHP}");
            Console.WriteLine($"Атака: {Attack}");
            Console.WriteLine($"Защита: {Defense}");
            Console.WriteLine("=================");
        }
    }

    // Класс Гоблина
    public class Goblin : Enemy
    {
        private double critChance;

        public Goblin(bool isBoss = false) : base("Гоблин", 30, 8, 2, isBoss)
        {
            critChance = isBoss ? 0.3 : 0.2; // 20% базовый, 30% у босса
        }

        public override int PerformAttack(Player player)
        {
            bool isCrit = random.NextDouble() < critChance;
            int damage = Attack;

            if (isCrit)
            {
                damage = (int)(damage * 1.5);
                Console.WriteLine($"{Name} наносит критический удар!");
            }

            Console.WriteLine($"{Name} атакует и наносит {damage} урона!");
            return damage;
        }
    }

    // Класс Скелета
    public class Skeleton : Enemy
    {
        public Skeleton(bool isBoss = false) : base("Скелет", 25, 10, 3, isBoss) { }

        public override int PerformAttack(Player player)
        {
            Console.WriteLine($"{Name} атакует и наносит {Attack} урона (игнорирует защиту)!");
            return Attack; // Урон игнорирует защиту
        }
    }

    // Класс Мага
    public class Mage : Enemy
    {
        private double freezeChance;

        public Mage(bool isBoss = false) : base("Маг", 20, 12, 1, isBoss)
        {
            freezeChance = isBoss ? 0.3 : 0.2; // 20% базовый, 30% у босса
        }

        public override int PerformAttack(Player player)
        {
            bool isFreeze = random.NextDouble() < freezeChance;
            int damage = Attack;

            Console.WriteLine($"{Name} атакует и наносит {damage} урона!");

            if (isFreeze)
            {
                Console.WriteLine($"{Name} накладывает заморозку! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }

            return damage;
        }
    }

    // Классы боссов
    public class VVG : Goblin
    {
        public VVG() : base(true)
        {
            Name = "ВВГ";
            MaxHP = (int)(MaxHP * 2.0);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }
    }

    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base(true)
        {
            Name = "Ковальский";
            MaxHP = (int)(MaxHP * 2.5);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }

    public class ArchmageCPP : Mage
    {
        public ArchmageCPP() : base(true)
        {
            Name = "Архимаг C++";
            MaxHP = (int)(MaxHP * 1.8);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
        }
    }

    public class PestovC : Skeleton
    {
        private double freezeChance;

        public PestovC() : base(true)
        {
            Name = "Пестов С--";
            MaxHP = (int)(MaxHP * 1.3);
            CurrentHP = MaxHP;
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
            freezeChance = 0.35; // 35% шанс заморозки
        }

        public override int PerformAttack(Player player)
        {
            bool isFreeze = random.NextDouble() < freezeChance;
            int damage = Attack;

            Console.WriteLine($"{Name} атакует и наносит {damage} урона (игнорирует защиту)!");

            if (isFreeze)
            {
                Console.WriteLine($"{Name} накладывает заморозку! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }

            return damage;
        }
    }

    // Главный класс игры
    public class Game
    {
        private Player player;
        private Random random;
        private int turnCount;

        public Game()
        {
            player = new Player(100);
            random = new Random();
            turnCount = 0;
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в текстовую рогалик-игру!");
            Console.WriteLine("Цель: выживать как можно дольше, побеждая врагов и собирая снаряжение.");
            Console.WriteLine("Каждые 10 ходов вас ждет встреча с боссом!\n");

            while (player.IsAlive())
            {
                turnCount++;
                Console.WriteLine($"\n=== ХОД {turnCount} ===");
                player.DisplayStats();

                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                    ContinueGame();
                    continue;
                }

                // Случайное событие: 50% враг, 50% сундук
                if (random.Next(2) == 0) // 0 - враг, 1 - сундук
                {
                    EncounterEnemy();
                }
                else
                {
                    OpenChest();
                }

                if (!player.IsAlive())
                {
                    Console.WriteLine("\n=== ИГРА ОКОНЧЕНА ===");
                    Console.WriteLine($"Вы продержались {turnCount} ходов!");
                    break;
                }

                ContinueGame();
            }
        }

        private void EncounterEnemy()
        {
            Enemy enemy;

            // Каждые 10 ходов - босс
            if (turnCount % 10 == 0)
            {
                enemy = GenerateBoss();
                Console.WriteLine($"!!! Появился босс: {enemy.Name} !!!");
            }
            else
            {
                enemy = GenerateRandomEnemy();
                Console.WriteLine($"Появился враг: {enemy.Name}");
            }

            enemy.DisplayStats();
            Combat(enemy);
        }

        private void OpenChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            Item item = GenerateRandomItem();
            Console.WriteLine($"В сундуке: {item.GetStats()}");

            if (item is Potion)
            {
                item.Use(player);
            }
            else if (item is Weapon weapon)
            {
                Console.WriteLine($"Ваше текущее оружие: {player.CurrentWeapon.GetStats()}");
                Console.WriteLine("Хотите взять новое оружие? (1 - да, 2 - нет)");

                if (GetYesNoInput())
                {
                    weapon.Use(player);
                }
                else
                {
                    Console.WriteLine("Вы оставили оружие в сундуке.");
                }
            }
            else if (item is Armor armor)
            {
                Console.WriteLine($"Ваши текущие доспехи: {player.CurrentArmor.GetStats()}");
                Console.WriteLine("Хотите взять новые доспехи? (1 - да, 2 - нет)");

                if (GetYesNoInput())
                {
                    armor.Use(player);
                }
                else
                {
                    Console.WriteLine("Вы оставили доспехи в сундуке.");
                }
            }
        }

        private Item GenerateRandomItem()
        {
            int itemType = random.Next(3);
            return itemType switch
            {
                0 => new Potion(),
                1 => GenerateRandomWeapon(),
                2 => GenerateRandomArmor(),
                _ => new Potion()
            };
        }

        private Weapon GenerateRandomWeapon()
        {
            string[] weaponNames = { "Стальной меч", "Острый кинжал", "Боевой топор", "Магический посох", "Лук охотника" };
            string name = weaponNames[random.Next(weaponNames.Length)];
            int attack = random.Next(5, 16); // Атака от 5 до 15
            return new Weapon(name, attack);
        }

        private Armor GenerateRandomArmor()
        {
            string[] armorNames = { "Кожаная броня", "Кольчуга", "Латные доспехи", "Магический плащ", "Доспехи стража" };
            string name = armorNames[random.Next(armorNames.Length)];
            int defense = random.Next(2, 8); // Защита от 2 до 7
            return new Armor(name, defense);
        }

        private bool GetYesNoInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "1") return true;
                if (input == "2") return false;
                Console.WriteLine("Неверный ввод. Введите 1 (да) или 2 (нет).");
            }
        }

        private void ContinueGame()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }