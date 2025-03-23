const directions = [
    {x: 2, y: 1}, {x: 1, y: 2}, {x: -1, y: 2}, {x: -2, y: 1},
    {x: -2, y: -1}, {x: -1, y: -2}, {x: 1, y: -2}, {x: 2, y: -1}
];

// Перевірка, чи можна потрапити в клітину
function isValid(x, y, board) {
    return x >= 0 && x < 8 && y >= 0 && y < 8 && !board[x][y];
}

// Пошук найкоротшого шляху
function findShortestPath(start, end, blocked) {
    let visited = Array.from({ length: 8 }, () => Array(8).fill(false));
    let prev = Array.from({ length: 8 }, () => Array(8).fill(null));
    let queue = [start];
    visited[start.x][start.y] = true;

    while (queue.length > 0) {
        let curr = queue.shift();

        if (curr.x === end.x && curr.y === end.y) {
            let path = [];
            while (curr !== null) {
                path.unshift(curr);
                curr = prev[curr.x][curr.y];
            }
            return [path.length - 1, path];
        }

        // Пробуємо всі можливі ходи
        for (let dir of directions) {
            let newX = curr.x + dir.x, newY = curr.y + dir.y;
            if (isValid(newX, newY, blocked) && !visited[newX][newY]) {
                visited[newX][newY] = true;
                prev[newX][newY] = curr;
                queue.push({x: newX, y: newY});
            }
        }
    }

    return [-1, []];
}

// Виведення шахової дошки
function printBoard(start, end, blocked) {
    let board = Array.from({ length: 8 }, () => Array(8).fill('.'));

    for (let i = 0; i < 8; i++) {
        for (let j = 0; j < 8; j++) {
            if (blocked[i][j]) board[i][j] = '#';
        }
    }

    board[start.x][start.y] = 'S';
    board[end.x][end.y] = 'E';

    console.log("Шахова дошка:");
    for (let row of board) {
        console.log(row.join(' '));
    }
}

// Головна функція
function main() {
    let start = prompt("Введіть початкову позицію коня (x1 y1):").split(" ").map(Number);
    let end = prompt("Введіть кінцеву позицію коня (x2 y2):").split(" ").map(Number);
    
    let blockedCount = parseInt(prompt("Введіть кількість заблокованих клітин:"));
    let blocked = Array.from({ length: 8 }, () => Array(8).fill(false));
    
    for (let i = 0; i < blockedCount; i++) {
        let [bx, by] = prompt("Введіть заблоковану клітину (x y):").split(" ").map(Number);
        blocked[bx - 1][by - 1] = true;
    }

    let steps, path;
    [steps, path] = findShortestPath({x: start[0] - 1, y: start[1] - 1}, {x: end[0] - 1, y: end[1] - 1}, blocked);
    
    if (steps === -1) {
        console.log("Неможливо дістатися до кінцевої клітини.");
        return;
    }

    let history = path.map(p => `(${p.x + 1}, ${p.y + 1})`).join(" -> ");
    console.log("Історія кроків:", history);

    printBoard({x: start[0] - 1, y: start[1] - 1}, {x: end[0] - 1, y: end[1] - 1}, blocked);
}

main();
