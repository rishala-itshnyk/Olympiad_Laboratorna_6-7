from collections import deque

directions = [(2, 1), (1, 2), (-1, 2), (-2, 1), (-2, -1), (-1, -2), (1, -2), (2, -1)]

# Перевірка, чи можна потрапити в клітину
def is_valid(x, y, board):
    return 0 <= x < 8 and 0 <= y < 8 and not board[x][y]

# Пошук найкоротшого шляху
def find_shortest_path(start, end, blocked):
    visited = [[False] * 8 for _ in range(8)]
    prev = [[None] * 8 for _ in range(8)]
    queue = deque([start])
    visited[start[0]][start[1]] = True

    while queue:
        curr = queue.popleft()

        if curr == end:
            path = []
            while curr != start:
                path.append(curr)
                curr = prev[curr[0]][curr[1]]
            path.append(start)
            path.reverse()
            return len(path) - 1, path

        # Пробуємо всі можливі ходи
        for dir in directions:
            new_x, new_y = curr[0] + dir[0], curr[1] + dir[1]
            if is_valid(new_x, new_y, blocked) and not visited[new_x][new_y]:
                visited[new_x][new_y] = True
                prev[new_x][new_y] = curr
                queue.append((new_x, new_y))

    return -1, []

# Виведення шахової дошки
def print_board(start, end, blocked):
    board = [['.' for _ in range(8)] for _ in range(8)]

    for i in range(8):
        for j in range(8):
            if blocked[i][j]:
                board[i][j] = '#'
    
    board[start[0]][start[1]] = 'S'
    board[end[0]][end[1]] = 'E'

    print("Шахова дошка:")
    for row in board:
        print(" ".join(row))

# Головна функція
def main():
    start_x, start_y = map(int, input("Введіть початкову позицію коня (x1 y1): ").split())
    end_x, end_y = map(int, input("Введіть кінцеву позицію коня (x2 y2): ").split())
    
    blocked_count = int(input("Введіть кількість заблокованих клітин: "))
    blocked = [[False] * 8 for _ in range(8)]
    
    for _ in range(blocked_count):
        bx, by = map(int, input("Введіть заблоковану клітину (x y): ").split())
        blocked[bx-1][by-1] = True

    start = (start_x - 1, start_y - 1)
    end = (end_x - 1, end_y - 1)

    steps, path = find_shortest_path(start, end, blocked)
    if steps == -1:
        print("Неможливо дістатися до кінцевої клітини.")
        return

    history = [f"({p[0]+1}, {p[1]+1})" for p in path]
    print("Історія кроків:", " -> ".join(history))

    print_board(start, end, blocked)

if __name__ == "__main__":
    main()
