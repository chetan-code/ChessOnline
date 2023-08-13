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

The Chessboard class plays a pivotal role in this Unity-based chess game. It seamlessly integrates core mechanics, user interface, and multiplayer interactions.

## ChessPiece Class

The **ChessPiece class** represents individual chess pieces in the game. Each piece is associated with a team and type (pawn, rook, knight, bishop, queen, king). The class provides methods for obtaining available moves based on the current board configuration and supports special moves such as en passant, promotion, and castling. Chess pieces can be selected and deselected, triggering smooth animations. The class incorporates position and scale lerping for seamless transitions. It serves as a fundamental building block for creating and managing diverse chess piece behaviors within the Unity-based chess game.

### Key Features:
- Piece types: Pawn, Rook, Knight, Bishop, Queen, King.
- Available move calculation based on the current board.
- Support for special moves: en passant, promotion, castling.
- Smooth selection and deselection animations.
- Position and scale lerping for visual transitions.

## Queen Class
NOTE : You can look into movements of all individual pieces.

The **Queen class** extends the ChessPiece class to define the behavior of the queen chess piece in the game. The class determines the available moves for the queen on the chessboard, combining the movement patterns of both rooks and bishops. It calculates and adds valid positions to the list of moves for the queen. The code segment showcases the implementation of movement logic in various directions (up, down, left, right, diagonally), considering obstacles, enemy pieces, and board boundaries. The Queen class plays a crucial role in contributing to the diversity of piece behaviors and strategic possibilities within the Unity-based chess game.

### Key Features:
- Movement patterns of both rooks and bishops combined.
- Calculates available moves in multiple directions.
- Handles obstacles, enemy pieces, and board boundaries.
- Enhances strategic depth and diversity of piece behaviors.

# Networking 
## Client Class

The `Client` class forms an essential part of the multiplayer infrastructure, facilitating communication between players and the game server. It employs Unity's Networking Transport package to establish and maintain connections. The class follows a singleton pattern, ensuring a single active client instance throughout the game. 

### Initialization and Connection

The `Init` method initializes the client by creating a network driver and attempting a connection to the specified server endpoint (IP address and port). This process is crucial for enabling multiplayer interactions.

### Update and Message Processing

The `Update` method continuously updates the driver to manage connection state and events. It ensures that data is efficiently sent and received between the client and server. The method includes detailed event handling logic, such as identifying connection events, data reception, and disconnection from the server.

### Health Monitoring

The class monitors the health of the connection through the `CheckAlive` method. If the connection is lost unexpectedly, it triggers the `ConnectionDropped` event and shuts down the client.

### Sending and Receiving Data

The `SendToServer` method facilitates the sending of network messages to the server. It serializes the message and sends it using the network driver.

### Event Handling

The class also showcases robust event handling mechanisms. It registers for the `KEEP_ALIVE` event, a ping-pong mechanism between the client and server that ensures both sides remain connected and responsive. The event handling system enhances modularity and readability.

### Singleton Structure

The class implements a singleton pattern through the `Instance` property, ensuring only one instance of the client exists at any given time.

## Server Class

The `Server` class is a pivotal component of the multiplayer functionality, acting as the game's host and facilitating communication between clients. Leveraging Unity's Networking Transport package, this class establishes, manages, and interacts with client connections.

### Initialization and Connection Handling

The `Init` method initializes the server by creating a network driver and binding it to an endpoint. This endpoint comprises essential information such as IP address and port, enabling the driver to listen for incoming connections. The driver subsequently enters a listening state, awaiting connections from clients. The server maintains a native list of connections to effectively manage participants in the multiplayer game.

### Update and Event Handling

The `Update` method serves as the heart of the server's operations. It regularly updates the driver's state, handles events, and ensures seamless data exchange between clients and the server. The method includes a comprehensive event processing system, handling data reception, new connections, and disconnections.

### Connection Management

The class employs the `CleanupConnections` method to remove stale or disconnected connections from the list. It uses the `AcceptNewConnections` method to accept new client connections, dynamically adding them to the list of active connections.

### Broadcast and Data Transmission

The server includes the capability to broadcast messages to all connected clients through the `Broadcast` method. It serializes and transmits data to individual clients using the `SendToClient` method, ensuring efficient data exchange over the network.

### Keep-Alive Mechanism

The class integrates a keep-alive mechanism to monitor client connections' health. It periodically sends a "keep-alive" message to clients to confirm their presence and responsiveness. This mechanism enhances the reliability of the server-client interaction.

### Singleton Structure

The class implements a singleton pattern using the `Instance` property, ensuring only one instance of the server is active at a time.

