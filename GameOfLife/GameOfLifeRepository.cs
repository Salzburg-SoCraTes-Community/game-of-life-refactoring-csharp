namespace GameOfLife;

// Intentionally ugly implementation with lots of refactoring opportunities:
// - God class doing too much
// - Long methods
// - Magic numbers
// - Duplicate code
// - Poor naming
// - No separation of concerns
// - Mutable state everywhere
// - No abstractions

public class GameOfLifeRepository
{
    private int[,] grid;
    private int w;
    private int h;
    public int generation = 0;

    public GameOfLifeRepository(int width, int height)
    {
        w = width;
        h = height;
        grid = new int[w, h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                grid[i, j] = 0;
            }
        }
    }

    public void SetCell(int x, int y, bool alive)
    {
        if (x >= 0 && x < w && y >= 0 && y < h)
        {
            grid[x, y] = alive ? 1 : 0;
        }
    }

    public bool GetCell(int x, int y)
    {
        if (x >= 0 && x < w && y >= 0 && y < h)
        {
            return grid[x, y] == 1;
        }
        return false;
    }

    public void Tick()
    {
        int[,] newGrid = new int[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int count = 0;

                // Check all 8 neighbors - lots of duplicate code here
                if (x > 0 && y > 0 && grid[x - 1, y - 1] == 1) count++;
                if (y > 0 && grid[x, y - 1] == 1) count++;
                if (x < w - 1 && y > 0 && grid[x + 1, y - 1] == 1) count++;
                if (x > 0 && grid[x - 1, y] == 1) count++;
                if (x < w - 1 && grid[x + 1, y] == 1) count++;
                if (x > 0 && y < h - 1 && grid[x - 1, y + 1] == 1) count++;
                if (y < h - 1 && grid[x, y + 1] == 1) count++;
                if (x < w - 1 && y < h - 1 && grid[x + 1, y + 1] == 1) count++;

                // Conway's rules implemented in a convoluted way
                if (grid[x, y] == 1)
                {
                    if (count == 2 || count == 3)
                    {
                        newGrid[x, y] = 1;
                    }
                    else
                    {
                        newGrid[x, y] = 0;
                    }
                }
                else
                {
                    if (count == 3)
                    {
                        newGrid[x, y] = 1;
                    }
                    else
                    {
                        newGrid[x, y] = 0;
                    }
                }
            }
        }

        grid = newGrid;
        generation++;
    }

    // Method that does too much
    public string GetState()
    {
        string result = "";
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (grid[x, y] == 1)
                {
                    result += "█";
                }
                else
                {
                    result += "·";
                }
            }
            result += "\n";
        }
        return result;
    }

    public int GetWidth()
    {
        return w;
    }

    public int GetHeight()
    {
        return h;
    }

    public int GetGeneration()
    {
        return generation;
    }

    // Duplicate logic with GetCell but slightly different
    public int CountLivingCells()
    {
        int count = 0;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (grid[x, y] == 1)
                {
                    count++;
                }
            }
        }
        return count;
    }

    // Another method that could be refactored
    public void SetPattern(int startX, int startY, string pattern)
    {
        string[] lines = pattern.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y].Trim();
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == 'O' || line[x] == 'o' || line[x] == '1' || line[x] == '*')
                {
                    SetCell(startX + x, startY + y, true);
                }
                else if (line[x] == '.' || line[x] == '-' || line[x] == '0' || line[x] == ' ')
                {
                    SetCell(startX + x, startY + y, false);
                }
            }
        }
    }

    public void Clear()
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                grid[x, y] = 0;
            }
        }
        generation = 0;
    }

    // Method with magic numbers and unclear purpose
    public bool IsStable(int iterations)
    {
        string state1 = GetState();
        for (int i = 0; i < iterations; i++)
        {
            Tick();
        }
        string state2 = GetState();

        // Reset back
        for (int i = 0; i < iterations; i++)
        {
            // This doesn't actually reset properly - another bug/issue to refactor
            Tick();
        }

        return state1 == state2;
    }
}
