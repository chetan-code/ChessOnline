using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Material DefaultMaterial;
    public Material HightlightMaterial;
    public Material MoveHightlightMaterial;

    private bool _isHighlihted;
    private bool _isMoveHighlighted;
    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        DefaultMaterial = _renderer.material;

    }

    private void OnMouseEnter()
    {
        HoverHighlight();
    }


    private void OnMouseExit()
    {
        HoverUnhightlight();
    }

    public void AvailableToMoveHighligt() {
        _isMoveHighlighted = true;
        _renderer.material = MoveHightlightMaterial;
    }

    public void AvailableToMoveUnhighligt()
    {
        _isMoveHighlighted = false;
        _renderer.material = DefaultMaterial;
    }

    public void HoverHighlight() {
        if (_isMoveHighlighted) return;
        if (_isHighlihted) { return; }
        _isHighlihted = true;
        _renderer.material = HightlightMaterial;  
    }

    public void HoverUnhightlight() {
        if (_isMoveHighlighted) return;
        if(!_isHighlihted){ return; }
        _renderer.material = DefaultMaterial;
        _isHighlihted = false;
    }


}
    
