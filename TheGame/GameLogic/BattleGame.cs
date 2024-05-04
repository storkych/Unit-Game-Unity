﻿// Singleton - это паттерн проектирования, который гарантирует, что у класса есть только один экземпляр, и предоставляет глобальную точку доступа к этому экземпляру.
public class BattleGame
{
    // Instance - хранит единственный экземпляр класса BattleGame.
    private Army initialArmy1;
    private Army initialArmy2;
    private static BattleGame instance;
    private static readonly object lockObj = new object();


    public Army Army1 { get; set; }
    public Army Army2 { get; set; }

    // Приватный конструктор, который вызывается только внутри класса, что предотвращает создание экземпляров извне.
    private protected BattleGame()
    {
        initialArmy1 = new Army("Левая армия");
        initialArmy2 = new Army("Правая армия");
    }

    // Instance - предоставляет глобальную точку доступа к единственному экземпляру класса BattleGame. Если экземпляр не создан, он создается при первом вызове.
    public static BattleGame Instance // убрать public конструктор
    {
        get
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance ??= new BattleGame();
                }
            }
            return instance;
        }
    }

    public void CreateArmies()
    {
        FrontManager.GetInstance().Printer("Создание левой армии:");
        initialArmy1.CreateArmy(450);

        FrontManager.GetInstance().Printer("Создание правой армии:");
        initialArmy2.CreateArmy(450);

        // Создаем копии армий для текущего хода
        Army1 = new Army("Левой армии");
        Army2 = new Army("Правой армии");

        // Копируем состояние из начальных армий
        Army.CopyArmyState(initialArmy1, Army1);
        Army.CopyArmyState(initialArmy2, Army2);
    }

    public void PlayUntilEnd()
    {
        while (Army1.IsAlive() && Army2.IsAlive())
        {
            Army1.MakeMove(Army2);
            if (!Army2.IsAlive()) // Проверяем, остались ли живые юниты в армии 2 после хода армии 1
            {
                FrontManager.GetInstance().Printer($"{Army1.Name} победила!");
                return; // Завершаем игру, если армия 2 уничтожена
            }

            Army2.MakeMove(Army1);
            if (!Army1.IsAlive()) // Проверяем, остались ли живые юниты в армии 1 после хода армии 2
            {
                FrontManager.GetInstance().Printer($"{Army2.Name} победила!");
                return; // Завершаем игру, если армия 1 уничтожена
            }
        }

        // Если цикл выше прервался без объявления победителя, проверяем, кто выиграл
        if (!Army1.IsAlive() && Army2.IsAlive())
        {
            FrontManager.GetInstance().Printer($"{Army2.Name} победила!");
        }
        else if (Army1.IsAlive() && !Army2.IsAlive())
        {
            FrontManager.GetInstance().Printer($"{Army1.Name} победила!");
        }
        else
        {
            FrontManager.GetInstance().Printer("Битва окончена ничьей."); // В случае, если обе армии были уничтожены одновременно
        }
    }

    public static int ReadIntegerInput()
    {
        while (true)
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (FormatException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введите число.");
            }
            catch (OverflowException)
            {
                FrontManager.GetInstance().Printer("Ошибка ввода. Введено слишком большое число.");
            }
        }
    }
    
}
