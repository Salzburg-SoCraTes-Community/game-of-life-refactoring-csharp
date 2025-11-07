using Xunit;

namespace GameOfLife.Tests;

public class GameOfLifeIntegrationTests
{
    [Fact]
    public void EmptyGrid_AfterOneTick_RemainsEmpty()
    {
        var repo = new GameOfLifeRepository(10, 10);

        repo.Tick();

        Assert.Equal(0, repo.CountLivingCells());
    }

    [Fact]
    public void SingleCell_Dies_DueToUnderpopulation()
    {
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(5, 5, true);

        repo.Tick();

        Assert.False(repo.GetCell(5, 5));
        Assert.Equal(0, repo.CountLivingCells());
    }

    [Fact]
    public void Block_RemainsStable()
    {
        // Block pattern (2x2)
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(4, 4, true);
        repo.SetCell(4, 5, true);
        repo.SetCell(5, 4, true);
        repo.SetCell(5, 5, true);

        repo.Tick();

        Assert.True(repo.GetCell(4, 4));
        Assert.True(repo.GetCell(4, 5));
        Assert.True(repo.GetCell(5, 4));
        Assert.True(repo.GetCell(5, 5));
        Assert.Equal(4, repo.CountLivingCells());
    }

    [Fact]
    public void Blinker_OscillatesPeriod2_Horizontal()
    {
        // Blinker pattern (horizontal line of 3)
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(4, 5, true);
        repo.SetCell(5, 5, true);
        repo.SetCell(6, 5, true);

        Assert.Equal(3, repo.CountLivingCells());

        // After one tick, should be vertical
        repo.Tick();

        Assert.False(repo.GetCell(4, 5));
        Assert.True(repo.GetCell(5, 4));
        Assert.True(repo.GetCell(5, 5));
        Assert.True(repo.GetCell(5, 6));
        Assert.False(repo.GetCell(6, 5));
        Assert.Equal(3, repo.CountLivingCells());
    }

    [Fact]
    public void Blinker_OscillatesPeriod2_Complete()
    {
        // Blinker pattern
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(4, 5, true);
        repo.SetCell(5, 5, true);
        repo.SetCell(6, 5, true);

        // After two ticks, should return to original state
        repo.Tick();
        repo.Tick();

        Assert.True(repo.GetCell(4, 5));
        Assert.True(repo.GetCell(5, 5));
        Assert.True(repo.GetCell(6, 5));
        Assert.False(repo.GetCell(5, 4));
        Assert.False(repo.GetCell(5, 6));
        Assert.Equal(3, repo.CountLivingCells());
    }

    [Fact]
    public void Glider_MovesCorrectly_FirstStep()
    {
        // Glider pattern
        var repo = new GameOfLifeRepository(20, 20);
        repo.SetCell(2, 1, true);
        repo.SetCell(3, 2, true);
        repo.SetCell(1, 3, true);
        repo.SetCell(2, 3, true);
        repo.SetCell(3, 3, true);

        Assert.Equal(5, repo.CountLivingCells());

        repo.Tick();

        // After one tick
        Assert.Equal(5, repo.CountLivingCells());
        Assert.True(repo.GetCell(1, 2));
        Assert.True(repo.GetCell(3, 2));
        Assert.True(repo.GetCell(2, 3));
        Assert.True(repo.GetCell(3, 3));
        Assert.True(repo.GetCell(2, 4));
    }

    [Fact]
    public void Glider_MovesCorrectly_FourSteps()
    {
        // Glider should move diagonally
        var repo = new GameOfLifeRepository(20, 20);
        repo.SetCell(2, 1, true);
        repo.SetCell(3, 2, true);
        repo.SetCell(1, 3, true);
        repo.SetCell(2, 3, true);
        repo.SetCell(3, 3, true);

        for (int i = 0; i < 4; i++)
        {
            repo.Tick();
        }

        // After 4 ticks, glider should have moved one cell down and right
        Assert.Equal(5, repo.CountLivingCells());
        Assert.True(repo.GetCell(3, 2));
        Assert.True(repo.GetCell(4, 3));
        Assert.True(repo.GetCell(2, 4));
        Assert.True(repo.GetCell(3, 4));
        Assert.True(repo.GetCell(4, 4));
    }

    [Fact]
    public void Overpopulation_CellDies()
    {
        // Cell with 4 neighbors dies
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(5, 5, true); // Center cell
        repo.SetCell(4, 4, true);
        repo.SetCell(5, 4, true);
        repo.SetCell(6, 4, true);
        repo.SetCell(4, 5, true);

        repo.Tick();

        Assert.False(repo.GetCell(5, 5)); // Center should die from overpopulation
    }

    [Fact]
    public void DeadCell_WithThreeNeighbors_BecomesAlive()
    {
        // Dead cell with exactly 3 neighbors becomes alive
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(4, 4, true);
        repo.SetCell(5, 4, true);
        repo.SetCell(6, 4, true);

        Assert.False(repo.GetCell(5, 5)); // This cell is dead

        repo.Tick();

        Assert.True(repo.GetCell(5, 5)); // Should become alive
    }

    [Fact]
    public void SetPattern_LoadsCorrectly_OFormat()
    {
        var repo = new GameOfLifeRepository(15, 15);
        string blinkerPattern = @"
...
OOO
...";

        repo.SetPattern(5, 5, blinkerPattern);

        Assert.True(repo.GetCell(5, 7));
        Assert.True(repo.GetCell(6, 7));
        Assert.True(repo.GetCell(7, 7));
        Assert.Equal(3, repo.CountLivingCells());
    }

    [Fact]
    public void SetPattern_LoadsCorrectly_MultipleFormats()
    {
        var repo = new GameOfLifeRepository(15, 15);
        string pattern = @"
O.O
.*.
1-1";

        repo.SetPattern(5, 5, pattern);

        Assert.True(repo.GetCell(5, 6));
        Assert.False(repo.GetCell(6, 6));
        Assert.True(repo.GetCell(7, 6));
        Assert.False(repo.GetCell(5, 7));
        Assert.True(repo.GetCell(6, 7));
        Assert.False(repo.GetCell(7, 7));
        Assert.True(repo.GetCell(5, 8));
        Assert.False(repo.GetCell(6, 8));
        Assert.True(repo.GetCell(7, 8));
        Assert.Equal(5, repo.CountLivingCells());
    }

    [Fact]
    public void Clear_RemovesAllCells()
    {
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(1, 1, true);
        repo.SetCell(2, 2, true);
        repo.SetCell(3, 3, true);
        repo.Tick();
        repo.Tick();

        repo.Clear();

        Assert.Equal(0, repo.CountLivingCells());
        Assert.Equal(0, repo.GetGeneration());
    }

    [Fact]
    public void Generation_IncrementsCorrectly()
    {
        var repo = new GameOfLifeRepository(10, 10);
        repo.SetCell(4, 4, true);
        repo.SetCell(4, 5, true);
        repo.SetCell(5, 4, true);
        repo.SetCell(5, 5, true);

        Assert.Equal(0, repo.GetGeneration());

        repo.Tick();
        Assert.Equal(1, repo.GetGeneration());

        repo.Tick();
        Assert.Equal(2, repo.GetGeneration());

        repo.Tick();
        Assert.Equal(3, repo.GetGeneration());
    }

    [Fact]
    public void GetState_ReturnsCorrectRepresentation()
    {
        var repo = new GameOfLifeRepository(5, 5);
        repo.SetCell(1, 1, true);
        repo.SetCell(2, 2, true);
        repo.SetCell(3, 3, true);

        string state = repo.GetState();

        Assert.Contains("█", state);
        Assert.Contains("·", state);
        string[] lines = state.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(5, lines.Length);
    }

    [Fact]
    public void Toad_OscillatesPeriod2()
    {
        // Toad pattern
        var repo = new GameOfLifeRepository(15, 15);
        repo.SetCell(5, 6, true);
        repo.SetCell(6, 6, true);
        repo.SetCell(7, 6, true);
        repo.SetCell(4, 7, true);
        repo.SetCell(5, 7, true);
        repo.SetCell(6, 7, true);

        int initialCount = repo.CountLivingCells();
        Assert.Equal(6, initialCount);

        repo.Tick();
        Assert.Equal(6, repo.CountLivingCells());

        repo.Tick();
        Assert.Equal(6, repo.CountLivingCells());

        // Should return to original state
        Assert.True(repo.GetCell(5, 6));
        Assert.True(repo.GetCell(6, 6));
        Assert.True(repo.GetCell(7, 6));
        Assert.True(repo.GetCell(4, 7));
        Assert.True(repo.GetCell(5, 7));
        Assert.True(repo.GetCell(6, 7));
    }

    [Fact]
    public void Beacon_OscillatesPeriod2()
    {
        // Beacon pattern
        var repo = new GameOfLifeRepository(15, 15);
        repo.SetCell(5, 5, true);
        repo.SetCell(6, 5, true);
        repo.SetCell(5, 6, true);
        repo.SetCell(8, 7, true);
        repo.SetCell(7, 8, true);
        repo.SetCell(8, 8, true);

        Assert.Equal(6, repo.CountLivingCells());

        repo.Tick();
        Assert.Equal(8, repo.CountLivingCells());

        repo.Tick();
        Assert.Equal(6, repo.CountLivingCells());
    }

    [Fact]
    public void EdgeCells_BehaviorCorrect()
    {
        // Test cells at the edge of the grid
        var repo = new GameOfLifeRepository(5, 5);
        repo.SetCell(0, 0, true);
        repo.SetCell(0, 1, true);
        repo.SetCell(1, 0, true);

        repo.Tick();

        // These should form a stable block
        Assert.True(repo.GetCell(0, 0));
        Assert.True(repo.GetCell(0, 1));
        Assert.True(repo.GetCell(1, 0));
        Assert.True(repo.GetCell(1, 1));
        Assert.Equal(4, repo.CountLivingCells());
    }

    [Fact]
    public void OutOfBounds_GetCell_ReturnsFalse()
    {
        var repo = new GameOfLifeRepository(10, 10);

        Assert.False(repo.GetCell(-1, 5));
        Assert.False(repo.GetCell(5, -1));
        Assert.False(repo.GetCell(10, 5));
        Assert.False(repo.GetCell(5, 10));
    }

    [Fact]
    public void OutOfBounds_SetCell_DoesNotCrash()
    {
        var repo = new GameOfLifeRepository(10, 10);

        repo.SetCell(-1, 5, true);
        repo.SetCell(5, -1, true);
        repo.SetCell(10, 5, true);
        repo.SetCell(5, 10, true);

        // Should not crash and no cells should be alive
        Assert.Equal(0, repo.CountLivingCells());
    }

    [Fact]
    public void LargeGrid_PerformanceTest()
    {
        var repo = new GameOfLifeRepository(100, 100);

        // Add a glider
        repo.SetCell(50, 50, true);
        repo.SetCell(51, 51, true);
        repo.SetCell(49, 52, true);
        repo.SetCell(50, 52, true);
        repo.SetCell(51, 52, true);

        // Run 20 generations
        for (int i = 0; i < 20; i++)
        {
            repo.Tick();
        }

        Assert.Equal(5, repo.CountLivingCells());
    }
}
