// Kotlin

import java.io.File
import java.io.InputStream

val sample: List<String> = listOf(
    "0,9 -> 5,9",
    "8,0 -> 0,8",
    "9,4 -> 3,4",
    "2,2 -> 2,1",
    "7,0 -> 7,4",
    "6,4 -> 2,0",
    "0,9 -> 2,9",
    "3,4 -> 1,4",
    "0,0 -> 8,8",
    "5,5 -> 8,2",
)

data class Point(val x: Int, val y: Int) {
    override fun toString(): String {
        return "($x, $y)"
    }
}

fun sortMinMax(a: Int, b: Int): List<Int> {
    // Returns a list of integers,
    // sorted by min to max
    if (a > b) {
        return listOf(b, a)
    }
    else {
        return listOf(a, b)
    }
}

fun addToHashMap(mapObj: HashMap<Point, Int>, point: Point) {
    if (mapObj.containsKey(point)) {
        mapObj[point] = mapObj[point]!!+1
    }
    else {
        mapObj[point] = 1
    }
}

fun solveProblem(data: List<String>): List<Int> {
    // Parse the data first
    val linearPointsCount: HashMap<Point, Int> = hashMapOf() // Point(x,y) -> count
    val diagonalPointsCount: HashMap<Point, Int> = hashMapOf()
    var linearSum = 0
    var totalSum = 0
    for(lineStr in data) {
        val points: MutableList<Int> = mutableListOf()
        lineStr.split(" -> ").forEach { it.split(",").forEach { points.add(it.toInt()) } }
        if (points[0] == points[2]) {
            // x1 == x2, so iterate on y
            val x = points[0]
            val iterationY = sortMinMax(points[1], points[3])
            for(y in iterationY[0] until iterationY[1]+1) {
                val point = Point(x, y)
                // Increase the count for this Point
                addToHashMap(linearPointsCount, point)
                addToHashMap(diagonalPointsCount, point)
            }
        }
        else if (points[1] == points[3]) {
            // y1 == y2, iterate on x
            val y = points[1]
            val iterationX = sortMinMax(points[0], points[2])
            for(x in iterationX[0] until iterationX[1]+1) {
                val point = Point(x, y)
                // Increase count
                addToHashMap(linearPointsCount, point)
                addToHashMap(diagonalPointsCount, point)
            }
        }
        else {
            // Diagonal
            // In this one, we must keep the order of the points
            var (x1, y1) = arrayOf(points[0], points[1])
            var (x2, y2) = arrayOf(points[2], points[3])
            var verticalProgression: Int = if (y1 > y2) { -1 } else { 1 } // If y1 is larger, we move backwards, otherwise forwards
            var horizontalProgression: Int = if (x1 > x2) { -1 } else { 1 } // If x1 is larger, we move backwards
            while((y1 != y2) and (x1 != x2)) {
                val point = Point(x1, y1)
                // Increase count
                addToHashMap(diagonalPointsCount, point)
                y1 += verticalProgression
                x1 += horizontalProgression
            }
            // Off-by-one error in the loop, correct here.
            addToHashMap(diagonalPointsCount, Point(x1, y1))
        }
    }
    for(point in linearPointsCount.keys) {
        if (linearPointsCount[point]!! > 1) {
            linearSum++
        }
    }
    // The sum for the total sum includes both the linear and diagonal values
    // If the point also exists in the linear hash map, it must be at least 2
    for(point in diagonalPointsCount.keys) {
        if (diagonalPointsCount[point]!! > 1) {
            totalSum++
        }
    }
    return listOf(linearSum, totalSum)
}

fun main() {
    val inputStream: InputStream = File("input.txt").inputStream()
    val inputString = inputStream.bufferedReader().use { it.readText() }
    val inputStringList: List<String> = inputString.split("\n")
    val trimmedInput = inputStringList.toMutableList()
    trimmedInput.removeAt(trimmedInput.size-1)
    println(solveProblem(sample))
    println(solveProblem(trimmedInput))
}
