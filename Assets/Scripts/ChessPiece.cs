using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChessPieceType { 
   None = 0,
   Pawn = 1,
   Rook = 2,
   Knight = 3,
   Bishop = 4, 
   Queen = 5,
   King = 6
}

public class ChessPiece : MonoBehaviour
{
    public Team Team;
    public ChessPieceType Type;
    public int CurrentX;
    public int CurrentY;

    private Vector3 _desiredPosition;
    private Vector3 _desiredScale;

    private float yMoveOffset = 0.2f;


    private bool _isSelected = false;

    public Vector2Int CurrentPosition => new Vector2Int(CurrentX, CurrentY);


    private void Start()
    {
        transform.rotation = Quaternion.Euler((Team == Team.White) ? new Vector3(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z) : new Vector3(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z));
    }


    public virtual List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board,int tileCountX , int tileCountY) {
        List<Vector2Int> moves = new List<Vector2Int>();

        moves.Add(new Vector2Int(3, 3));
        moves.Add(new Vector2Int(4, 3));
        moves.Add(new Vector2Int(3, 4));
        moves.Add(new Vector2Int(4, 4));

        return moves;
    }

    //enpassant only for pawn
    //promotion only for pawn
    //castling only for king
    public virtual SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> currentAvailableMoves) { 
        return SpecialMove.None;
    }

    public void SetPosition(Vector3 desiredPosition) {
        _desiredPosition = desiredPosition;
        StartCoroutine(PositionLerp(transform.position, desiredPosition, 0.25f));
    }

    public void SetScale(Vector3 desiredScale) {
        _desiredScale = desiredScale;
        transform.localScale = desiredScale;
    }


    public void Select() {
        _isSelected = true;
        StartCoroutine(SmoothLerp(0.15f, Vector3.up * yMoveOffset)); 
    }

    public void Deselect() {
        if (_isSelected)
        {
            _isSelected = false;
            StartCoroutine(SmoothLerp(0.15f, -Vector3.up * yMoveOffset));
        }
    }

    private IEnumerator SmoothLerp(float time, Vector3 valueChange)
    {

        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position + valueChange;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator PositionLerp(Vector3 from,Vector3 to,float time)
    {

        Vector3 startingPos = from;
        Vector3 finalPos = to;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


}
