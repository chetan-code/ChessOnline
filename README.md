# ChessOnline


https://github.com/chetan-code/ChessOnline/assets/57151493/b3f3f125-d328-456a-a91b-4a31b46ea308

How it works :
## Chessboard Class

The **Chessboard class** serves as the backbone of this chess game project on Unity. It encompasses a range of functionalities crucial for the game's mechanics and gameplay. Here's how it works:

### Core Mechanics Management
The Chessboard class is responsible for handling key game mechanics such as creating the chessboard, positioning and moving pieces, and managing teams and turns. It utilizes enumerations to define teams and special moves, providing a structured foundation for gameplay.

### Board Visualization
The class generates the chessboard as a grid of GameObjects, each representing a tile. These tiles are enhanced with materials and scripts to facilitate interactions and highlighting of valid moves.

### Piece Interactions
Pieces, including rooks, knights, and more, are spawned, positioned, and moved using techniques like raycasting and lerping. The class supports various special moves like En Passant, Castling, and Promotion, adding depth and complexity to the gameplay.

### Multiplayer Integration
For multiplayer experiences, the Chessboard class manages team assignments and handles turn-based gameplay. It coordinates player actions and ensures a smooth experience for both local and remote multiplayer scenarios.

### UI Components and Victory
The class handles UI components for victory screens and rematch functionality. It triggers the display of victory screens based on the winning team and allows players to request a rematch.

### Code Organization
The codebase is structured into distinct sections, each catering to specific functionalities. This includes segments for game logic, UI management, and networking interactions. Despite handling multiple responsibilities, the code maintains clarity by separating concerns and offering a coherent structure.

The Chessboard class plays a pivotal role in this Unity-based chess game. It seamlessly integrates core mechanics, user interface, and multiplayer interactions, providing a holistic gaming experience that encompasses both intricate chess strategies and engaging gameplay dynamics.
