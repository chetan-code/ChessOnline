using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        //top right
        int x = CurrentX + 1;
        int y = CurrentY + 2;
        
        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0) {
            if (board[x, y] == null || board[x, y].Team != Team) { 
                moves.Add(new Vector2Int(x, y));
            }
        }

        //top left
        x = CurrentX - 1;
        y = CurrentY + 2;
        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }

        //bottom right
        x = CurrentX + 1;
        y = CurrentY - 2;

        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }


        //bottom left
        x = CurrentX - 1;
        y = CurrentY - 2;

        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }


        //right top
        x = CurrentX + 2;
        y = CurrentY + 1;

        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }

        //left top
        x = CurrentX - 2;
        y = CurrentY + 1;

        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }

        //right bottom
        x = CurrentX + 2;
        y = CurrentY - 1;

        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }

        //left bottom
        x = CurrentX - 2;
        y = CurrentY - 1;

        if (x < tileCountX && x >= 0 && y < tileCountY && y >= 0)
        {
            if (board[x, y] == null || board[x, y].Team != Team)
            {
                moves.Add(new Vector2Int(x, y));
            }
        }

        return moves;
    }

}
