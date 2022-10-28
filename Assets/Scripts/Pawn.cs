using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int direction = (Team == Team.White) ? 1 : -1;

        //one up/down
        if (board[CurrentX, CurrentY + direction] == null ) { 
            moves.Add(new Vector2Int(CurrentX, CurrentY + direction));
        }

        //two moves up but only at start
        if (board[CurrentX, CurrentY + direction] == null)
        {
            if (Team == Team.White && CurrentY == 1 && board[CurrentX, CurrentY + (direction * 2)] == null)
            {
                moves.Add(new Vector2Int(CurrentX, CurrentY + (2 * direction)));
            }
            if (Team == Team.Black && CurrentY == 6 && board[CurrentX, CurrentY + (direction * 2)] == null)
            {
                moves.Add(new Vector2Int(CurrentX, CurrentY + (2 * direction)));
            }
        }

        //kill move
        if (CurrentX != 0) {
            if (board[CurrentX - 1, CurrentY + direction] != null && board[CurrentX - 1, CurrentY + direction].Team != Team) {
                moves.Add(new Vector2Int(CurrentX - 1, CurrentY + direction));
            }
        }

        if (CurrentX != tileCountX - 1)
        {
            if (board[CurrentX + 1, CurrentY + direction] != null && board[CurrentX + 1, CurrentY + direction].Team != Team)
            {
                moves.Add(new Vector2Int(CurrentX + 1, CurrentY + direction));
            }
        }


        return moves;
    }

    //https://en.wikipedia.org/wiki/En_passant
    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> currentAvailableMoves)
    {
        int direction = (Team == Team.White) ? 1 : -1;

        //was their a move already done
        if (moveList.Count > 0) {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            //was the last move - a pawn move?
            if (board[lastMove[1].x, lastMove[1].y].Type == ChessPieceType.Pawn) {
                //does the pawn move - was double move ? 
                //Mathf.Abs was used as the direction of pawn move can be opposite
                if (Mathf.Abs(lastMove[0].y - lastMove[1].y) == 2) {
                    //was the move from other team
                    //well not necessary since the game is turn based
                    if (board[lastMove[1].x, lastMove[1].y].Team != Team) {
                        //the other pawn is on same Y - on right/left side
                        if (lastMove[1].y == CurrentY) {
                            //on my left side
                            if (lastMove[1].x == CurrentX - 1) {
                                currentAvailableMoves.Add(new Vector2Int(CurrentX - 1, CurrentY + direction));

                                return SpecialMove.EnPassant;
                            }
                           //on my right side
                            if (lastMove[1].x == CurrentX + 1)
                            {
                                currentAvailableMoves.Add(new Vector2Int(CurrentX + 1, CurrentY + direction));

                                return SpecialMove.EnPassant;
                            }
                        }
                    }
                }
            }
        }


        //promotion - queening
        if (Team == Team.White && CurrentY == 6 || Team == Team.Black && CurrentY == 1) {
            return SpecialMove.Promotion;
        }


        return SpecialMove.None;
    }
}
