using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int x;
        int y; ;


        //up
        x = CurrentX;
        for (y = CurrentY + 1; y < tileCountY; y++)
        {
            //empty tile
            if (board[x, y] == null) { 
                moves.Add(new Vector2Int(x, y));
            }

            //not empty
            if (board[x, y] != null ) {
                //enemy piece
                if (board[x, y].Team != Team) {
                    moves.Add(new Vector2Int(x, y));
                    break;
                }
                //our piece
                if (board[x, y].Team == Team) {
                    break;
                }
            }

        }

        //down
        x = CurrentX;
        for (y = CurrentY - 1; y >= 0; y--)
        {
            //empty tile
            if (board[x, y] == null)
            {
                moves.Add(new Vector2Int(x, y));
            }

            //not empty
            if (board[x, y] != null)
            {
                //enemy piece
                if (board[x, y].Team != Team)
                {
                    moves.Add(new Vector2Int(x, y));
                    break;
                }
                //our piece
                if (board[x, y].Team == Team)
                {
                    break;
                }
            }

        }


        //right
        y = CurrentY;
        for (x = CurrentX + 1; x < tileCountX; x++)
        {
            //empty tile
            if (board[x, y] == null)
            {
                moves.Add(new Vector2Int(x, y));
            }

            //not empty
            if (board[x, y] != null)
            {
                //enemy piece
                if (board[x, y].Team != Team)
                {
                    moves.Add(new Vector2Int(x, y));
                    break;
                }
                //our piece
                if (board[x, y].Team == Team)
                {
                    break;
                }
            }
        }

        //left
        y = CurrentY;
        for (x = CurrentX - 1; x >= 0; x--)
        {
            //empty tile
            if (board[x, y] == null)
            {
                moves.Add(new Vector2Int(x, y));
            }

            //not empty
            if (board[x, y] != null)
            {
                //enemy piece
                if (board[x, y].Team != Team)
                {
                    moves.Add(new Vector2Int(x, y));
                    break;
                }
                //our piece
                if (board[x, y].Team == Team)
                {
                    break;
                }
            }
        }

        return moves;
    }
}
