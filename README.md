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
![puml](https://github.com/chetan-code/ChessOnline/assets/57151493/1a268e4c-9027-4e8b-b392-a1bb9578e130)

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

## NetMessage Class

The `NetMessage` class is a base class for data messages transmitted between the client and the server in a networked application. It serves as a foundation for creating specific message types that can be sent and received by networked entities. Each `NetMessage` instance includes an `OperationCode` that defines the type of operation or message it represents.

### Properties

- `Code`: An `OperationCode` property that represents the type of operation or message associated with the `NetMessage`. It is used for identifying the purpose of the message.

### Methods

- `Serialize(ref DataStreamWriter writer)`: This method is responsible for serializing the data contained within the `NetMessage` instance into a format that can be sent over the network. It uses a `DataStreamWriter` to write the serialized data, including the `OperationCode`.

- `Deserialize(DataStreamReader reader)`: This method is used to deserialize data received from the network back into a `NetMessage` instance. It is implemented in subclasses and is responsible for reading any additional serialized data.

- `ReceiveOnClient()`: This method is invoked on the client-side when a `NetMessage` is received from the server. It can be overridden in subclasses to handle the specific behavior associated with processing the received message on the client.

- `ReceiveOnServer(NetworkConnection clientConnection)`: This method is invoked on the server-side when a `NetMessage` is received from a client. The `clientConnection` parameter identifies the client that sent the message. It can be overridden in subclasses to handle the specific behavior associated with processing the received message on the server.

### Usage

The `NetMessage` class is intended to be subclassed to create specific message types tailored to the needs of the networked application. Subclasses can define additional properties and methods to represent the specific data and behavior associated with different message types. By providing serialization and deserialization methods, as well as receive methods for both client and server, the `NetMessage` class facilitates communication between networked entities.

It's important to note that this class acts as a foundation and should not be directly instantiated. Instead, create subclasses that extend `NetMessage` to define and handle distinct message types.


## NetMakeMove Class

The `NetMakeMove` class represents a specific type of network message used to convey chess move information between the client and the server. This message is used to communicate details about a chess piece's movement on the board, including its original and destination positions, as well as the team identifier.

### Properties

- `originalX`: An integer property representing the X-coordinate of the original position of the chess piece.
- `originalY`: An integer property representing the Y-coordinate of the original position of the chess piece.
- `destinationX`: An integer property representing the X-coordinate of the destination position of the chess piece.
- `destinationY`: An integer property representing the Y-coordinate of the destination position of the chess piece.
- `teamId`: An integer property representing the team identifier associated with the chess piece making the move.

### Constructors

- `NetMakeMove()`: A constructor that initializes the `Code` property to the appropriate operation code for making a move.
- `NetMakeMove(DataStreamReader reader)`: A constructor that initializes the `Code` property and deserializes the received data from a `DataStreamReader`.

### Methods

- `Serialize(ref DataStreamWriter writer)`: This method serializes the data within the `NetMakeMove` instance into a format suitable for network transmission. It includes the operation code and chess move details.
- `Deserialize(DataStreamReader reader)`: This method deserializes the received data from a `DataReaderStream` back into a `NetMakeMove` instance, extracting the chess move details.
- `ReceiveOnClient()`: This method is invoked on the client-side when a `NetMakeMove` message is received from the server. It triggers an event (`NetUtility.C_MAKE_MOVE`) to handle the received move details.
- `ReceiveOnServer(NetworkConnection clientConnection)`: This method is invoked on the server-side when a `NetMakeMove` message is received from a client. It triggers an event (`NetUtility.S_MAKE_MOVE`) to handle the received move details, including the associated client connection.

### Usage

The `NetMakeMove` class is an example of a specialized network message type designed to facilitate communication of chess move information between the client and server. It includes methods for serialization, deserialization, and handling of received data. This class is designed to be used within a broader networked chess application, where it plays a crucial role in updating the game state across the network.

To utilize the `NetMakeMove` class, create instances of it to represent chess move data and use its serialization methods to send data to the network. Register event handlers to the `C_MAKE_MOVE` and `S_MAKE_MOVE` events in the `NetUtility` class to respond to these move messages appropriately on both client and server sides.


## NetUtility Class

The `NetUtility` class serves as a critical component in managing network communication between the client and server within a networked chess game. It defines an enum for categorizing various types of messages and provides event handlers to manage the processing of these messages on both the client and server sides.

### Enums

- `OperationCode`: Enumerates operation codes that represent different types of network messages, including `KEEP_ALIVE`, `WELCOME`, `START_GAME`, `MAKE_MOVE`, and `REMATCH`. These codes are crucial for identifying the nature of incoming and outgoing messages.

### Event Handlers

The `NetUtility` class offers a set of event handlers that are triggered upon receiving specific types of network messages:

#### Client-Side Event Handlers:

- `C_KEEP_ALIVE`: Triggered upon receiving a `NetKeepAlive` message on the client side.
- `C_WELCOME`: Triggered upon receiving a `NetWelcome` message on the client side.
- `C_START_GAME`: Triggered upon receiving a `NetStartGame` message on the client side.
- `C_MAKE_MOVE`: Triggered upon receiving a `NetMakeMove` message on the client side.
- `C_REMATCH`: Triggered upon receiving a `NetRematch` message on the client side (not currently implemented).

#### Server-Side Event Handlers:

- `S_KEEP_ALIVE`: Triggered upon receiving a `NetKeepAlive` message on the server side. Includes the relevant client connection.
- `S_WELCOME`: Triggered upon receiving a `NetWelcome` message on the server side. Includes the relevant client connection.
- `S_START_GAME`: Triggered upon receiving a `NetStartGame` message on the server side. Includes the relevant client connection.
- `S_MAKE_MOVE`: Triggered upon receiving a `NetMakeMove` message on the server side. Includes the relevant client connection.
- `S_REMATCH`: Triggered upon receiving a `NetRematch` message on the server side (not currently implemented). Includes the relevant client connection.

### Methods

- `OnData(DataStreamReader stream, NetworkConnection connection, Server server = null)`: This method reads and processes incoming network data. It accepts a `DataStreamReader` for reading data, a `NetworkConnection` representing the client connection, and an optional `Server` instance (used when reading data on the server side). The method uses the operation code to determine the message type and invokes the appropriate event handler.

### Usage

The `NetUtility` class acts as a central hub for processing incoming network messages based on their operation codes. It deciphers the operation code from received data and directs the message to the corresponding event handler. This class works in conjunction with specific message classes (e.g., `NetKeepAlive`, `NetWelcome`, etc.) to facilitate communication between the client and server within a networked chess game.

To employ the `NetUtility` class, register event handlers for the relevant events on both the client and server sides. Upon data reception, the `OnData` method decodes the operation code and triggers the relevant event handler to manage the message content, whether on the client or server side.

