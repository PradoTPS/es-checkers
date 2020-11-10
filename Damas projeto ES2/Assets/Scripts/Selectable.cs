using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    GameObject mygo = null;
    public bool IsSelected
    {
        get 
        { 
            return isSelected;
        }

        set
        {
            isSelected = value;
            State = isSelected ? SelectableColor.selected : SelectableColor.initial;
            UpdateColorAcorddingToState();
        }
    }

    public bool IsHovered { get => isHovered; set => isHovered = value; }
    public bool CanHover { get => canHover; set => canHover = value; }

    protected bool canHover = false;
    protected bool isSelected = false;
    protected bool isHovered = false;

    public SelectableColor State
    { 
        get => state;
        set
        {
            state = value;
            UpdateColorAcorddingToState();
        } 
    }
    private SelectableColor state;

    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private Color movementSelected = Color.green;
    [SerializeField] private Color tileInCheck = Color.red;
    [SerializeField] private Color hoveredColor = Color.grey;
    private Color initialColor = Color.white;   //overriden in awake

    private SpriteRenderer myRenderer = null;
    #region Unity Functions

    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        initialColor = myRenderer.color;

    }

    protected virtual void OnMouseEnter()
    {
        isHovered = true;
        UpdateColorAcorddingToState();
    }

    protected virtual void OnMouseExit()
    {
        isHovered = false;
        UpdateColorAcorddingToState();
    }

    protected virtual void OnMouseDown()
    {
        isSelected = isSelected ? false : true;
        UpdateColorAcorddingToState();
    }

    #endregion

    #region Protected Functions

    protected void UpdateColorAcorddingToState()
    {
        Color colorToSet;

        switch (State)
        {

            case SelectableColor.hovered:
                colorToSet = hoveredColor;
                break;

            case SelectableColor.selected:
                colorToSet = selectedColor;
                break;

            case SelectableColor.movementSelected:
                colorToSet = movementSelected;
                break;

            case SelectableColor.tileInCheck:
                colorToSet = tileInCheck;
                break;

            default:
                colorToSet = initialColor;
                break;
           
        }

        SetColor(colorToSet);
    }

    #endregion


    private void SetColor(Color newColor)
    {
        myRenderer.color = newColor;
    }

}

public enum SelectableColor
{
    initial,
    hovered,
    selected,
    tileInCheck,
    movementSelected
}
