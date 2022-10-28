using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        //moves
        List<Vector2Int> moves = new List<Vector2Int>();

        //right
        if (CurrentX + 1 < tileCountX)
        {
            if (board[CurrentX + 1, CurrentY] == null)
            {
                moves.Add(new Vector2Int(CurrentX + 1, CurrentY));
            }
            else if (board[CurrentX + 1, CurrentY].Team != Team) { 
                moves.Add(new Vector2Int(CurrentX + 1, CurrentY));
            }

            //top right
            if (CurrentY + 1 < tileCountY) {
                if (board[CurrentX + 1, CurrentY + 1] == null)
                {
                    //empty
                    moves.Add(new Vector2Int(CurrentX + 1, CurrentY + 1));
                }
                else if (board[CurrentX + 1, CurrentY + 1].Team != Team)
                {
                    //enemy
                    moves.Add(new Vector2Int(CurrentX + 1, CurrentY + 1));
                }
            }

            //bottom right 
            if (CurrentY - 1 > 0) {
                if (board[CurrentX + 1, CurrentY - 1] == null)
                {
                    //empty block
                    moves.Add(new Vector2Int(CurrentX + 1, CurrentY - 1));
                }
                else if (board[CurrentX + 1, CurrentY - 1].Team != Team)
                {
                    //enemy piece
                    moves.Add(new Vector2Int(CurrentX + 1, CurrentY - 1));
                }
            }
        }
        
        //left
        if (CurrentX - 1 >= 0)
        {
            if (board[CurrentX - 1, CurrentY] == null)
            {
                moves.Add(new Vector2Int(CurrentX - 1, CurrentY));
            }
            else if (board[CurrentX - 1, CurrentY].Team != Team)
            {
                moves.Add(new Vector2Int(CurrentX - 1, CurrentY));
            }

            //top left
            if (CurrentY + 1 < tileCountY)
            {
                if (board[CurrentX - 1, CurrentY + 1] == null)
                {
                    //empty
                    moves.Add(new Vector2Int(CurrentX - 1, CurrentY + 1));
                }
                else if (board[CurrentX - 1, CurrentY + 1].Team != Team)
                {
                    //enemy
                    moves.Add(new Vector2Int(CurrentX - 1, CurrentY + 1));
                }
            }

            //bottom left 
            if (CurrentY - 1 > 0)
            {
                if (board[CurrentX - 1, CurrentY - 1] == null)
                {
                    //empty block
                    moves.Add(new Vector2Int(CurrentX - 1, CurrentY - 1));
                }
                else if (board[CurrentX - 1, CurrentY - 1].Team != Team)
                {
                    //enemy piece
                    moves.Add(new Vector2Int(CurrentX - 1, CurrentY - 1));
                }
            }
        }
        //up
        if (CurrentY + 1 < tileCountY) {
            if (board[CurrentX, CurrentY + 1] == null || board[CurrentX, CurrentY + 1].Team != Team) {
                moves.Add(new Vector2Int(CurrentX, CurrentY + 1));
            }
        }

        //down
        if (CurrentY - 1 >= 0) {
            if (board[CurrentX, CurrentY - 1] == null || board[CurrentX, CurrentY - 1].Team != Team) {
                moves.Add(new Vector2Int(CurrentX, CurrentY - 1));
            }
        }

        return moves;
    }

    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> currentAvailableMoves)
    {
        SpecialMove specialMove = SpecialMove.None;

        //getting move based on intial position of king (x == 4) & y == 0/7 based on team
        var kingMove = moveList.Find(m => m[0].x == 4 && m[0].y == ((Team == Team.White) ? 0 : 7));
        var leftRookMove = moveList.Find(m => m[0].x == 0 && m[0].y == ((Team == Team.White) ? 0 : 7));
        var rightRookMove = moveList.Find(m => m[0].x == 7 && m[0].y == ((Team == Team.White) ? 0 : 7));

        //king hasn't moved
        if (kingMove == null && CurrentX == 4) {
            //white team
            if (Team == Team.White) {

                //left rook hasn;t moved
                if (leftRookMove == null) {
                    //it is present at intial pos
                    if (board[0, 0].Type == ChessPieceType.Rook) {
                        //if it is my team rook...may happen than other team rook is present at same pos
                        if (board[0, 0].Team == Team.White) {
                            //make sure nothing is between king and rook
                            if (board[1, 0] == null && board[2, 0] == null && board[3, 0] == null) {
                                currentAvailableMoves.Add(new Vector2Int(2, 0));
                                specialMove = SpecialMove.Castling;
                            }

                        }
                    }
                }

                //right hasn;t moved
                if (rightRookMove == null)
                {
                    //it is present at intial pos
                    if (board[7, 0].Type == ChessPieceType.Rook)
                    {
                        //if it is my team rook ...may happen than other team rook is present at same pos
                        if (board[7, 0].Team == Team.White)
                        {
                            //make sure nothing is between king and rook
                            if (board[5, 0] == null && board[6, 0] == null)
                            {
                                currentAvailableMoves.Add(new Vector2Int(6, 0));
                                specialMove = SpecialMove.Castling;
                            }
                        }
                    }
                }

            }

            //black team
            if (Team == Team.Black)
            {

                //left rook hasn;t moved
                if (leftRookMove == null)
                {
                    //it is present at intial pos
                    if (board[0, 7].Type == ChessPieceType.Rook)
                    {
                        //if it is my team rook...may happen than other team rook is present at same pos
                        if (board[0, 7].Team == Team.Black)
                        {
                            //make sure nothing is between king and rook
                            if (board[1, 7] == null && board[2, 7] == null && board[3, 7] == null)
                            {
                                currentAvailableMoves.Add(new Vector2Int(2, 7));
                                specialMove = SpecialMove.Castling;
                            }

                        }
                    }
                }

                //right hasn;t moved
                if (rightRookMove == null)
                {
                    //it is present at intial pos
                    if (board[7, 7].Type == ChessPieceType.Rook)
                    {
                        //if it is my team rook ...may happen than other team rook is present at same pos
                        if (board[7, 7].Team == Team.Black)
                        {
                            //make sure nothing is between king and rook
                            if (board[5, 7] == null && board[6, 7] == null)
                            {
                                currentAvailableMoves.Add(new Vector2Int(6, 7));
                                specialMove = SpecialMove.Castling;
                            }
                        }
                    }
                }

            }

        }

        return specialMove;
    }
}
