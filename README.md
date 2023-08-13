# ChessOnline


https://github.com/chetan-code/ChessOnline/assets/57151493/b3f3f125-d328-456a-a91b-4a31b46ea308

How it works :
Chessboard Class :

The Chessboard class manages a chess game's core mechanics. It handles board creation, piece positioning, and movement. Enumerations define teams and special moves. Tiles are generated as GameObjects with materials and scripts. Pieces (rooks, knights, etc.) are spawned, positioned, and moved via raycasting and lerping. Special moves like EnPassant, Castling, and Promotion are supported. Multiplayer elements include team management and turn handling. Dead pieces are displayed on the side. This class forms a fundamental part of a Unity-based chess game, offering gameplay essentials like piece interactions, move validation, and special move execution.

The code includes methods for simulating piece movements, checking for checkmate scenarios, highlighting valid moves, handling UI components for victory and rematch, and managing network interactions for both local and remote multiplayer experiences. The project employs a 2D array to represent the chessboard and tracks the positions of chess pieces. The code is organized into several sections, each catering to specific functionalities such as game logic, UI handling, and networking. It offers a clear structure for understanding game mechanics, multiplayer interactions, and user interface design in a Chess gaming context.

- I know this class could have been divided into several other classes - as it handles mutliple reponsibilty at this point namely - chess core mechanics, UI handling  and networking.
