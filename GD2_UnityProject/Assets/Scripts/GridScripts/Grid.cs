using GridScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [SerializeField]
    private int _widthBoard=5;

    [SerializeField]
    private int _heightBoard=5;

    public int Width => _widthBoard;
    public int Hieght => _heightBoard;

    [SerializeField]
    private GameObject _gridColliderPrefab = null;

    [SerializeField]
    private GameObject _borderBlocks = null;

    private Dictionary<GridPosition, GameObject> _gridColliders = new Dictionary<GridPosition, GameObject>();
    
    void Start()
    {
        InitGrid();
        InitGridBorders();
    }

    private void InitGridBorders()
    {
        InitBottomBorder();
        InitSideBorder();
    }

    private void InitSideBorder()
    {
        float prefabWidth = GetPrefabWidth();
        float prefabHeight = GetPrefabHeight();

        for (int y = 0; y < _heightBoard; y++)
        {
            Vector3 instantiatePosition = new Vector3(-prefabWidth, prefabHeight*y, 0);
            GameObject newGridCollider = Instantiate(_borderBlocks, instantiatePosition, Quaternion.identity, this.transform);
        }

        for (int y = 0; y < _heightBoard; y++)
        {
            Vector3 instantiatePosition = new Vector3(prefabWidth*_widthBoard, prefabHeight * y, 0);
            GameObject newGridCollider = Instantiate(_borderBlocks, instantiatePosition, Quaternion.identity, this.transform);
        }
    }

    private void InitBottomBorder()
    {
        float prefabWidth = GetPrefabWidth();
        float prefabHeight = GetPrefabHeight();

        for (int x = 0; x < _widthBoard; x++)
        {
            Vector3 instantiatePosition = new Vector3(prefabWidth * x, -prefabHeight, 0);
            GameObject newGridCollider = Instantiate(_borderBlocks, instantiatePosition, Quaternion.identity, this.transform);
        }
    }

    private void InitGrid()
    {
        float prefabWidth = GetPrefabWidth();
        float prefabHeight = GetPrefabHeight();

        for (int y = 0; y <_heightBoard; y++)
        {
            for (int x = 0; x <_widthBoard; x++)
            {
                Vector3 instantiatePosition = new Vector3(prefabWidth * x, prefabHeight * y, 0);
                GridPosition gridPosition = new GridPosition { X = x, Y = y };

                GameObject newGridCollider = Instantiate(_gridColliderPrefab, instantiatePosition, Quaternion.identity, this.transform);
                newGridCollider.name = x.ToString() + ":" +y.ToString();
                _gridColliders.Add(gridPosition, newGridCollider);
            }
        }
    }

    private float GetPrefabHeight()=>_gridColliderPrefab.GetComponent<Transform>().localScale.y;

    private float GetPrefabWidth()=> _gridColliderPrefab.GetComponent<Transform>().localScale.x;
}
