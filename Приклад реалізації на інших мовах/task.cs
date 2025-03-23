using System;
using System.Collections.Generic;

class Program
{
    // Структура для зберігання позицій
    struct Position
    {
        public int x, y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // Напрями для ходів коня
    static Position[] directions = new Position[]
    {
        new Position(2, 1), new Position(1, 2), new Position(-1, 2), new Position(-2, 1),
        new Position(-2, -1), new Position(-1, -2), new Position(1, -2), new Position(2, -1)
    };

    // Перевірка, чи можна потрапити в клітину
    static bool IsValid(int x, int y, bool[,] board)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8 && !board[x, y];
    }

    // Пошук найкоротшого шляху
    static (int, List<Position>) FindShortestPath(Position start, Position end, bool[,] blocked)
    {
        bool[,] visited = new bool[8, 8];
        Position[,] prev = new Position[8, 8];
        Queue<Position> queue = new Queue<Position>();

        visited[start.x, start.y] = true;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Position curr = queue.Dequeue();

            if (curr.x == end.x && curr.y == end.y)
            {
                List<Position> path = new List<Position>();
                while (curr.x != start.x || curr.y != start.y)
                {
                    path.Insert(0, curr);
                    curr = prev[curr.x, curr.y];
                }
                path.Insert(0, start);
                return (path.Count - 1, path);
            }

            // Пробуємо всі можливі ходи
            foreach (var dir in directions)
            {
                int newX = curr.x + dir.x;
                int newY = curr.y + dir.y;
                if (IsValid(newX, newY, blocked) && !visited[newX, newY])
                {
                    visited[newX, newY] = true;
                    prev[newX, newY] = curr;
                    queue.Enqueue(new Position(newX, newY));
                }
            }
        }

        return (-1, null);  // Якщо шлях не знайдено
    }

    // Виведення дошки
    static void PrintBoard(Position start, Position end, bool[,] blocked)
    {
        string[,] board = new string[8, 8];

        // Заповнюємо дошку блоками та порожніми клітинами
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (blocked[i, j])
                    board[i, j] = "#";
                else
                    board[i, j] = ".";
            }
        }

        board[start.x, start.y] = "S";
        board[end.x, end.y] = "E";

        Console.WriteLine("Шахова дошка:");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        int startX, startY, endX, endY, blockedCount;

        Console.Write("Введіть початкову позицію коня (x1 y1): ");
        var startPos = Console.ReadLine().Split();
        startX = int.Parse(startPos[0]);
        startY = int.Parse(startPos[1]);

        Console.Write("Введіть кінцеву позицію коня (x2 y2): ");
        var endPos = Console.ReadLine().Split();
        endX = int.Parse(endPos[0]);
        endY = int.Parse(endPos[1]);

        Console.Write("Введіть кількість заблокованих клітин: ");
        blockedCount = int.Parse(Console.ReadLine());

        bool[,] blocked = new bool[8, 8];
        for (int i = 0; i < blockedCount; i++)
        {
            Console.Write("Введіть заблоковану клітину (x y): ");
            var blockedCell = Console.ReadLine().Split();
            int bx = int.Parse(blockedCell[0]);
            int by = int.Parse(blockedCell[1]);
            blocked[bx - 1, by - 1] = true;  // Враховуємо, що координати в C# мають бути від 0
        }

        Position start = new Position(startX - 1, startY - 1);
        Position end = new Position(endX - 1, endY - 1);

        var (steps, path) = FindShortestPath(start, end, blocked);
        if (steps == -1)
        {
            Console.WriteLine("Неможливо дістатися до кінцевої клітини.");
            return;
        }

        List<string> history = new List<string>();
        foreach (var p in path)
        {
            history.Add($"({p.x + 1}, {p.y + 1})");  // Перетворюємо на 1-індексацію
        }

        Console.WriteLine("Історія кроків: " + string.Join(" -> ", history));
        PrintBoard(start, end, blocked);
    }
}
