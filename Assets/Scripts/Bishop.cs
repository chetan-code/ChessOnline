using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        //top right
        for (int x = CurrentX + 1, y = CurrentY + 1; x < tileCountX && y < tileCountY; x++, y++) {
            if (board[x, y] == null) { 
                moves.Add(new Vector2Int(x, y));
            }

            if (board[x, y] != null && board[x, y].Team != Team) {
                moves.Add(new Vector2Int(x, y));
                break;
            }

            if (board[x, y] != null && board[x, y].Team == Team) {
                break;
            }

        }

        //top left
        for (int x = CurrentX - 1, y = CurrentY + 1; x >= 0 && y < tileCountY; x--, y++)
        {
            if (board[x, y] == null)
            {
                moves.Add(new Vector2Int(x, y));
            }

            if (board[x, y] != null && board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }

            if (board[x, y] != null && board[x, y].Team == Team)
            {
                break;
            }

        }

        //Bottom right
        for (int x = CurrentX + 1, y = CurrentY - 1; x < tileCountX && y >= 0; x++, y--)
        {
            if (board[x, y] == null)
            {
                moves.Add(new Vector2Int(x, y));
            }

            if (board[x, y] != null && board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }

            if (board[x, y] != null && board[x, y].Team == Team)
            {
                break;
            }

        }


        //Bottom left
        for (int x = CurrentX - 1, y = CurrentY - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (board[x, y] == null)
            {
                moves.Add(new Vector2Int(x, y));
            }

            if (board[x, y] != null && board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
                break;
            }

            if (board[x, y] != null && board[x, y].Team == Team)
            {
                break;
            }

        }


        return moves;
    }
}
