# Conway's Game of Life - Refactoring Exercise

This is an intentionally "ugly" but functionally correct implementation of Conway's Game of Life in C#. It's designed as a foundation for a refactoring exercise.

## Running the Tests

```bash
dotnet test
```

## Building the Project

```bash
dotnet build
```

## Refactoring Opportunities

The code has been intentionally written with several code smells and refactoring opportunities:

### God Class
- `GameOfLifeRepository` does everything - cell management, game logic, rendering, pattern loading, etc.

### Long Methods
- `Tick()` method has nested loops and complex logic that could be extracted
- Neighbor counting logic is verbose and repetitive

### Magic Numbers
- Hard-coded values like `0`, `1`, `2`, `3` without named constants
- No clear distinction between alive/dead states

### Duplicate Code
- Neighbor checking has 8 similar if statements
- Multiple loops iterating over the grid in the same way
- Similar cell checking logic in multiple methods

### Poor Naming
- `w` and `h` instead of `width` and `height`
- `count` is used for different purposes
- Method names could be more descriptive

### Mutable Public State
- `generation` is a public field
- Direct array access

### No Separation of Concerns
- Grid representation mixed with game rules
- Rendering logic in the repository
- Pattern parsing in the same class

### No Abstractions
- No interfaces or abstract classes
- Direct coupling to implementation details
- Hard to test individual components

### Potential Issues
- `IsStable()` method has a logical bug (doesn't actually reset the grid properly)
- No encapsulation of the grid state
- Pattern loading accepts multiple formats inconsistently

## Test Coverage

All 23 integration tests pass and cover:
- Basic Conway's Game of Life rules (birth, death, survival)
- Classic patterns (Block, Blinker, Glider, Toad, Beacon)
- Edge cases (boundaries, empty grids)
- Pattern loading
- State management

The implementation is functionally correct despite the poor structure!
