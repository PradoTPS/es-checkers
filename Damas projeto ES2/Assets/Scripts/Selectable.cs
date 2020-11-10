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
            UpdateColorAcorddingToState();
        }
    }

    public bool IsHovered { get => isHovered; set => isHovered = value; }
    public bool CanHover { get => canHover; set => canHover = value; }

    protected bool canHover = false;
    protected bool isSelected = false;
    protected bool isHovered = false;

    [SerializeField] private Color selectedColor = Color.yellow;
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
        if (isSelected)
        {
            colorToSet = selectedColor;
        }
        else
        {
            colorToSet = isHovered ? hoveredColor : initialColor;
        }

        SetColor(colorToSet);
    }

    #endregion


    private void SetColor(Color newColor)
    {
        myRenderer.color = newColor;
    }




}
