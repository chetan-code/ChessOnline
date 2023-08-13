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

## ChessPiece Class

The **ChessPiece class** represents individual chess pieces in the game. Each piece is associated with a team and type (pawn, rook, knight, bishop, queen, king). The class provides methods for obtaining available moves based on the current board configuration and supports special moves such as en passant, promotion, and castling. Chess pieces can be selected and deselected, triggering smooth animations. The class incorporates position and scale lerping for seamless transitions. It serves as a fundamental building block for creating and managing diverse chess piece behaviors within the Unity-based chess game.

### Key Features:
- Piece types: Pawn, Rook, Knight, Bishop, Queen, King.
- Available move calculation based on the current board.
- Support for special moves: en passant, promotion, castling.
- Smooth selection and deselection animations.
- Position and scale lerping for visual transitions.

The ChessPiece class plays a pivotal role in defining the attributes and behaviors of individual chess pieces, enriching the game's strategic and visual aspects.

## Queen Class
NOTE : You can look into movements of all individual pieces.
The **Queen class** extends the ChessPiece class to define the behavior of the queen chess piece in the game. The class determines the available moves for the queen on the chessboard, combining the movement patterns of both rooks and bishops. It calculates and adds valid positions to the list of moves for the queen. The code segment showcases the implementation of movement logic in various directions (up, down, left, right, diagonally), considering obstacles, enemy pieces, and board boundaries. The Queen class plays a crucial role in contributing to the diversity of piece behaviors and strategic possibilities within the Unity-based chess game.

### Key Features:
- Movement patterns of both rooks and bishops combined.
- Calculates available moves in multiple directions.
- Handles obstacles, enemy pieces, and board boundaries.
- Enhances strategic depth and diversity of piece behaviors.

The Queen class enriches the game by implementing the versatile movement capabilities of the queen chess piece, contributing to the engaging and dynamic gameplay of the Unity-based chess game.
