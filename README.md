# Business Tour Mod

## Outdated ⚠️

This project is very old, and the technique it uses for changing the game values is outdated. This Mod will **NOT** work anymore, and is only here for educational and historical purpose. 

## Overview

The **Business Tour Mod** project was a modding tool for the game *Business Tour*. This tool allowed players to manipulate in-game values such as money, bonuses, and player states by accessing and modifying the game's memory. The Mod includes features like adding or removing money, granting infinite jail time, and controlling in-game bonuses.

## Features

- **Infinite Jail**: Toggle infinite jail time for any player.
- **Money Mod**: Add or remove money for any player.
- **Bonus Mod**: Control various in-game bonuses like free pair, free double, free card, and free reroll.
- **Anti-Jail**: Prevent players from being sent to jail.
- **Turn Mod**: Manipulate turn conditions to control game flow.

## Requirements

- Windows Operating System
- .NET Framework
- *Business Tour* game

## Getting Started

### Prerequisites

1. Install the .NET Framework if not already installed.
2. Ensure the *Business Tour* game is installed and running.

### Installation

1. Clone the repository or download the project files.
2. Open the project in an IDE that supports C# (e.g., Visual Studio).
3. Build the project to generate the executable file.

### Running the Mod

1. Start the *Business Tour* game.
2. Run the compiled executable of the Business Tour Mod.
3. The Mod will attempt to find the running instance of *Business Tour*.

## Usage

### Infinite Jail

Toggle infinite jail time for any player:
1. Select a player from the list.
2. Click the "Infinite Jail" button.

### Money Mod

Add or remove money for any player:
1. Select a player from the list.
2. Specify the amount of money to add or remove.
3. Click the "Add Money" or "Remove Money" button.

### Bonus Mod

Control various in-game bonuses:
1. Check or uncheck the desired bonus options (Free Pair, Free Double, Free Card, Free Reroll).

### Turn Mod

Manipulate turn conditions:
1. Click the "Not First Turn" button to adjust the turn conditions.

## Code Structure

### Main Components

- **BusinessTourMod.cs**: Initializes the Mod and provides core functionalities like finding the game process and handling UI interactions.
- **MoneyMod.cs**: Implements money manipulation features.
- **Mem.cs**: Contains functions for reading and writing memory of the game process.
- **HotelsFirstTurn.cs**: Implements the turn manipulation feature.
- **UpdateValues.cs**: Continuously updates memory addresses for various mods and handles bonus and jail states.
- **Program.cs**: Entry point of the application, performs initial checks and validations.

### Memory Handling

The `Memory` class in `Mem.cs` provides functions for:
- Reading and writing different data types (bytes, integers, floats, doubles, strings).
- Finding process handles and base addresses.
- Resolving pointer addresses with offsets.

## Contributing

Contributions to this project are welcome. If you find any bugs or have suggestions for new features, please open an issue or submit a pull request.

## Disclaimer

This tool is for educational purposes only. Using cheats in online games can result in bans or other consequences. Use this tool at your own risk.

## Contact

For any inquiries or support, please contact `maxxypurple` on Discord.