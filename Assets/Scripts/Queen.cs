using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{

    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        //rook moves + bishop moves
        List<Vector2Int> moves = new List<Vector2Int>();

        #region rook

        int x;
        int y; ;


        //up
        x = CurrentX;
        for (y = CurrentY + 1; y < tileCountY; y++)
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

        #endregion

        #region bishop 

        //top right
        for (x = CurrentX + 1, y = CurrentY + 1; x < tileCountX && y < tileCountY; x++, y++)
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

        //top left
        for (x = CurrentX - 1, y = CurrentY + 1; x >= 0 && y < tileCountY; x--, y++)
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
        for (x = CurrentX + 1, y = CurrentY - 1; x < tileCountX && y >= 0; x++, y--)
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
        for (x = CurrentX - 1, y = CurrentY - 1; x >= 0 && y >= 0; x--, y--)
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

        #endregion

        return moves;
    }

}


