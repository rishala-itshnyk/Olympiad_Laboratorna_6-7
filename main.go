package main

import (
	"container/list"
	"fmt"
)

type Position struct {
	x, y int
}

var directions = []Position{
	{2, 1}, {1, 2}, {-1, 2}, {-2, 1},
	{-2, -1}, {-1, -2}, {1, -2}, {2, -1},
}

// Перевірка, чи можна потрапити в клітину
func isValid(x, y int, board [8][8]bool) bool {
	return x >= 0 && x < 8 && y >= 0 && y < 8 && !board[x][y]
}

// Пошук найкоротшого шляху
func findShortestPath(start, end Position, blocked [8][8]bool) (int, []Position) {
	visited := [8][8]bool{}
	prev := [8][8]Position{}
	queue := list.New()

	visited[start.x][start.y] = true
	queue.PushBack(start)

	for queue.Len() > 0 {
		curr := queue.Remove(queue.Front()).(Position)

		if curr == end {
			path := []Position{}
			for curr != start {
				path = append([]Position{curr}, path...)
				curr = prev[curr.x][curr.y]
			}
			path = append([]Position{start}, path...)
			return len(path) - 1, path
		}

		// Пробуємо всі можливі ходи
		for _, dir := range directions {
			newX, newY := curr.x+dir.x, curr.y+dir.y
			if isValid(newX, newY, blocked) && !visited[newX][newY] {
				visited[newX][newY] = true
				prev[newX][newY] = curr
				queue.PushBack(Position{newX, newY})
			}
		}
	}

	return -1, nil
}

func printBoard(start, end Position, blocked [8][8]bool) {
	var board [8][8]string

	// Заповнюємо дошку блоками та порожніми клітинами
	for i := 0; i < 8; i++ {
		for j := 0; j < 8; j++ {
			if blocked[i][j] {
				board[i][j] = "#"
			} else {
				board[i][j] = "."
			}
		}
	}

	board[start.x][start.y] = "S"
	board[end.x][end.y] = "E"

	fmt.Println("Шахова дошка:")
	for i := 0; i < 8; i++ {
		for j := 0; j < 8; j++ {
			fmt.Print(board[i][j], " ")
		}
		fmt.Println()
	}
}

func main() {
	var startX, startY, endX, endY int
	var blockedCount int

	fmt.Print("Введіть початкову позицію коня (x1 y1): ")
	fmt.Scan(&startX, &startY)
	fmt.Print("Введіть кінцеву позицію коня (x2 y2): ")
	fmt.Scan(&endX, &endY)

	fmt.Print("Введіть кількість заблокованих клітин: ")
	fmt.Scan(&blockedCount)
	blocked := [8][8]bool{}
	for i := 0; i < blockedCount; i++ {
		var bx, by int
		fmt.Print("Введіть заблоковану клітину (x y): ")
		fmt.Scan(&bx, &by)
		blocked[bx][by] = true
	}

	start := Position{startX - 1, startY - 1}
	end := Position{endX - 1, endY - 1}

	steps, path := findShortestPath(start, end, blocked)
	if steps == -1 {
		fmt.Println("Неможливо дістатися до кінцевої клітини.")
		return
	}

	history := []string{}
	for _, p := range path {
		history = append(history, fmt.Sprintf("(%d, %d)", p.x+1, p.y+1))
	}
	fmt.Println("Історія кроків:", history)

	printBoard(start, end, blocked)
}
