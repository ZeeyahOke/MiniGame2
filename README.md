# Blasters

Blasters is a 2D top-down survival game built in Unity 6. You control a cube on the right side of an arena, and enemies keep spawning from the left. Shoot them before they reach you. Survive the round or die trying. Your score gets added to a top-5 leaderboard that persists across runs.

This was built as an OOP and design patterns assignment, so most of the code decisions are structured around demonstrating those principles cleanly.

## How to Play

- **WASD** or **Arrow Keys** to move
- **Spacebar** to attack (bullets fire leftward)
- Kill enemies to score points. Easy = 1, Medium = 3, Hard = 5.
- Grab pink health pickups when your HP drops.
- The round ends when the timer runs out or when your HP hits zero.

## Setup

1. Clone the repo:
   ```
   git clone https://github.com/ZeeyahOke/MiniGame2.git
   ```
2. Open Unity Hub and add the project folder.
3. Open the project in Unity 6 (any Unity 6 version should work).
4. Open the scene at `Assets/Scenes/SampleScene`.
5. Press Play.

## Project Structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs      // Singleton, score, timer, events, leaderboard, IDamageable
│   ├── Player.cs           // Movement, attack, health
│   ├── PlayerAttack.cs     // Bullet behavior
│   ├── Enemy.cs            // Abstract base + state machine
│   ├── EasyEnemy.cs        // Easy tier stats
│   ├── MediumEnemy.cs      // Medium tier stats
│   ├── HardEnemy.cs        // Hard tier stats
│   ├── HealthPickup.cs     // Restores HP on contact
│   ├── EnemySpawner.cs     // Spawns enemies + pickups, nearest-target search
│   ├── UIManager.cs        // Observer subscriber, updates all UI
│   └── GameOverPanel.cs    // End screen + leaderboard display
├── Prefabs/
├── Scenes/
└── Sprites/
```

## OOP Principles

- **Encapsulation** — Private fields with public read-only properties throughout. See `GameManager.cs` and `Player.cs` for the clearest examples.
- **Inheritance** — Abstract `Enemy` base class with three concrete subclasses (`EasyEnemy`, `MediumEnemy`, `HardEnemy`). The subclasses only configure their stats; all behavior is inherited.
- **Abstraction** — The `Enemy` class is abstract (can't be instantiated). The `IDamageable` interface abstracts "can take damage" from what the object actually is.
- **Polymorphism** — `PlayerAttack` calls `TakeDamage()` on any `IDamageable`, so bullets work on any damageable object without changes.

## Design Patterns

- **Singleton** — `GameManager` and `EnemySpawner` both use the singleton pattern with a static `Instance` property. Any script can access them globally.
- **Observer** — `GameManager` fires C# events (`OnScoreChanged`, `OnHealthChanged`, `OnRoundEnded`) that `UIManager` and `GameOverPanel` subscribe to.
- **State** — `Enemy` uses an `EnemyState` enum (`Spawning`, `Moving`, `Dying`) with a switch statement. Behavior changes based on the current state.

## Algorithms

- **Nearest-target search** — Linear scan in `EnemySpawner.FindNearestEnemy()`, used by the UI to show the closest threat distance. Uses squared distance for the comparison to skip an unnecessary square root.
- **Insertion sort** — In `GameManager.InsertScoreIntoLeaderboard()`. Sorts new scores into the top-5 leaderboard, which is persisted with PlayerPrefs.

## Notes

The scene uses 2D physics throughout. Enemies use Circle Collider 2D, the player and walls use Box Collider 2D, and the projectile uses Box Collider 2D as a trigger. Sorting layers are set up so the player renders on top of enemies, and projectiles render on top of everything except UI.

## Author

Fawziyyah Oke — Year 3 Game Development, African Leadership University
