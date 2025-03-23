<?php

$directions = [
    [2, 1], [1, 2], [-1, 2], [-2, 1], [-2, -1], [-1, -2], [1, -2], [2, -1]
];

function isValid($x, $y, $board) {
    return $x >= 0 && $x < 8 && $y >= 0 && $y < 8 && !$board[$x][$y];
}

function findShortestPath($start, $end, $blocked) {
    $visited = array_fill(0, 8, array_fill(0, 8, false));
    $prev = array_fill(0, 8, array_fill(0, 8, null));
    $queue = [$start];
    $visited[$start[0]][$start[1]] = true;

    while (count($queue) > 0) {
        $curr = array_shift($queue);

        if ($curr[0] == $end[0] && $curr[1] == $end[1]) {
            $path = [];
            while ($curr !== null) {
                array_unshift($path, $curr);
                $curr = $prev[$curr[0]][$curr[1]];
            }
            return [count($path) - 1, $path];
        }

        foreach ($directions as $dir) {
            $newX = $curr[0] + $dir[0];
            $newY = $curr[1] + $dir[1];
            if (isValid($newX, $newY, $blocked) && !$visited[$newX][$newY]) {
                $visited[$newX][$newY] = true;
                $prev[$newX][$newY] = $curr;
                array_push($queue, [$newX, $newY]);
            }
        }
    }

    return [-1, []];
}

function printBoard($start, $end, $blocked) {
    $board = array_fill(0, 8, array_fill(0, 8, '.'));

    foreach ($blocked as $block) {
        $board[$block[0]][$block[1]] = '#';
    }

    $board[$start[0]][$start[1]] = 'S';
    $board[$end[0]][$end[1]] = 'E';

    echo "Шахова дошка:\n";
    foreach ($board as $row) {
        echo implode(" ", $row) . "\n";
    }
}

$start = explode(" ", readline("Введіть початкову позицію коня (x1 y1): "));
$end = explode(" ", readline("Введіть кінцеву позицію коня (x2 y2): "));
$blockedCount = readline("Введіть кількість заблокованих клітин: ");

$blocked = [];
for ($i = 0; $i < $blockedCount; $i++) {
    $block = explode(" ", readline("Введіть заблоковану клітину (x y): "));
    $blocked[] = [$block[0] - 1, $block[1] - 1];
}

$start = [$start[0] - 1, $start[1] - 1];
$end = [$end[0] - 1, $end[1] - 1];

list($steps, $path) = findShortestPath($start, $end, $blocked);

if ($steps == -1) {
    echo "Неможливо дістатися до кінцевої клітини.\n";
    exit;
}

$history = array_map(function ($p) { return "({$p[0] + 1}, {$p[1] + 1})"; }, $path);
echo "Історія кроків: " . implode(" -> ", $history) . "\n";

printBoard($start, $end, $blocked);
?>
