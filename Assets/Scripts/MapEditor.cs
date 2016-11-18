using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapEditor : MonoBehaviour
{
    public int Height = 100;
    public int Width = 10;
    public int cellSize = 1;

    private GameObject map_root;
    public Camera EditCamera;
    [Range(0,100)]
    public float mouseDragSpeed = 5;

    public Vector3 originPoint;
    private Dictionary<int,GameObject> gridDictionary=new Dictionary<int, GameObject>();
    public void Awake()
    {
        map_root=new GameObject("MapRoot");
        CameraToStartPos();
    }



    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            EditCamera.transform.position += Time.deltaTime * new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * mouseDragSpeed;
        }


        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 worldPos = EditCamera.ScreenToWorldPoint(Input.mousePosition);
            if (IsInRange(worldPos))
            {
                Vector2 coord = GetCellCoord(worldPos);
                Vector3 spawnPoint = GetPosition((int)coord.x, (int)coord.y);
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.parent = map_root.transform;
                cell.name = "cell" + (int)coord.x + "_" + (int)coord.y;
                Vector3 offset =new Vector3(-cellSize*0.5f,cellSize*0.5f,0);
                spawnPoint += offset;
                cell.transform.position = spawnPoint;

            }
        }
    }


    
    
    public void OnDrawGizmos()
    {
        Vector3 offset=Vector3.zero;//=new Vector3(-cellSize*0.5f,cellSize*0.5f,0);
        for (int row = 0; row < Height+1; row++)
        {
            Vector3 rowStart = GetPosition(0, row) + offset;
            Vector3 rowEnd = GetPosition(Width, row)+offset;
            Gizmos.DrawLine(rowStart, rowEnd);
        }

        for (int col = 0; col < Width+1; col++)
        {

            Vector3 colStart = GetPosition(col, 0)+offset;
            Vector3 colEnd = GetPosition(col, Height)+offset;
            Gizmos.DrawLine(colEnd, colStart);
        }
    }

    public Vector3 GetPosition(int x, int y)
    {
        return originPoint+ new Vector3(x*cellSize,y*cellSize,0);
    }

    public bool IsInRange(Vector3 worldPos)
    {
        if (worldPos.x < originPoint.x || (worldPos.x > originPoint.x + cellSize*Width))
        {
            return false;
        }

        if (worldPos.y > originPoint.y + cellSize*Height || worldPos.y < originPoint.y)
        {
            return false; 
        }

        return true;
    }

    public Vector2 GetCellCoord(Vector3 worldPos)
    {
        return new Vector2((worldPos.x - originPoint.x + cellSize) / cellSize, (worldPos.y - originPoint.y /*+ cellSize * 0.5f*/) / cellSize);
    }

    public void CameraToStartPos()
    {
        Vector3 pos = GetPosition(Width / 2, 0);
        EditCamera.transform.position = new Vector3(pos.x, pos.y, EditCamera.transform.position.z);
    }

    public void CameraToEndPos()
    {
        Vector3 pos = GetPosition(Width / 2, Height);
        EditCamera.transform.position = new Vector3(pos.x, pos.y, EditCamera.transform.position.z);
    }

    public void EmptySelection()
    {
        
    }
}
