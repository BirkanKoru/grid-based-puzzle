# Grid-Based Puzzle Game Framework with Level Editor

This project is a grid-based puzzle game framework designed in Unity. It includes a customizable **Level Editor** to create and manage game levels directly within the Unity Editor. The system is modular, scalable, and designed to support various puzzle mechanics such as match-3 games or tile-based puzzles.

---

## Features

### Gameplay Framework
- **Dynamic Grid System**: Create and manage grids of any size.
- **Interactive Items**: Items with customizable attributes (e.g., health, types, icons).
- **Animations**: Smooth animations for item interactions using DOTween.

### Level Editor
- **Custom Unity Editor Integration**: Create and modify levels in a dedicated Unity Editor window.
- **Save and Load Levels**: Persistent storage of level configurations.
- **Item Customization**: Add, remove, and update item models dynamically.
- **Visual Map Editor**: Drag-and-drop interface to design grid layouts.

### Extensibility
- **Flexible Item Types**: Support for different item behaviors (e.g., color blocks, obstacles, breakables).
- **Scalable Grid Logic**: Designed to handle grids of various dimensions efficiently.

---

## Prerequisites

Ensure the following software is installed:
- Unity Editor (tested with Unity 2021.3+)
- A compatible C# IDE
- com.unity.feature.2d (Tutorial needs 2D)

---

## Usage

### Level Editor
1. Open the Level Editor window from the Unity toolbar (Puzzle > LevelEditor).
2. Use the Items Tab to create or edit items:
   - Add icons, define item health, and specify item types (e.g., Color, Breakable, Obstacle).
3. Use the Levels Tab to create and edit levels:
   - Set the grid size (rows and columns).
   - Place items on the grid using the drag-and-drop interface.
4. Save levels, which will be stored as .txt files in the Resources/LevelData/ folder.

---

## Project Structure

### Core Scripts
- **GridController.cs**: Manages grid initialization and gameplay logic.
- **GridPoint.cs**: Represents individual points on the grid.
- **GridOperations.cs**: Handles match detection and grid updates.
- **PlayerController.cs**: Processes player input and grid interactions.
- **Item.cs**: Manages individual item behaviors.

### Level Editor Scripts
- **LEController.cs**: Handles the Unity Editor window for level creation.
- **LEBase.cs**: Manages item models and editor operations.
- **LEItemModel.cs**: Defines item attributes (e.g., type, health, icons).
