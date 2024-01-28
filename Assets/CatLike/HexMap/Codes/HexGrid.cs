using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace CatLike.HexMap.Codes
{
    /// <summary>
    /// Hexmap Grid
    /// </summary>
    public class HexGrid : MonoBehaviour
    {
        public int GridWidth = 6;
        public int GridHeight = 6;
        public HexCell CellPrefab;
        
        /// <summary>
        /// 每个cell坐标显示
        /// </summary>
        public GameObject CellLablePrefab;

        public Canvas GridCanvas;
    
        //点击颜色
        public Color DefaultColor = Color.white;
        public Color TouchColor = Color.blue;
        public List<Color> ColorList = new List<Color>();

        private HexMesh mHexMesh;
        
        private HexCell[] mCells;
        
        
        private void Awake()
        {
            mHexMesh = GetComponentInChildren<HexMesh>();
            
            mCells = new HexCell[GridWidth * GridHeight];

            int index = 0;
            for (int z = 0; z < GridHeight; z++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    CreateCellAt(x, z, index++);
                }
            }
        }

        private void Start()
        {
            mHexMesh.BuildMesh(mCells);
        }

        Color GetRandomColor()
        {
            if (ColorList.Count > 0)
            {
                var index =  UnityEngine.Random.Range(0, ColorList.Count);
                return ColorList[index];
            }
            return DefaultColor;
        }
        
        private void CreateCellAt(int x, int z, int index)
        {
            Vector3 localPos;
            //x坐标是内半径的2倍,奇数行需要往右移动半个格子
            localPos.x = (x + (z % 2 == 1 ? 0.5f : 0 )) * (HexMetrics.innerRadius * 2);
            //z坐标是外半径的1.5倍
            localPos.z = z * (HexMetrics.outerRadius * 1.5f);
            localPos.y = 0;
            
            var cell = GameObject.Instantiate<HexCell>(CellPrefab, transform, false);
            cell.transform.localPosition = localPos;
            cell.name = $"[{x},{z}]";
            cell.gameObject.SetActive(false);
            
            cell.HexCoord = HexCoord.FromOffsetCoord(x,z);
            cell.CellColor = GetRandomColor();
            cell.neighbors = new HexCell[6];
            
            mCells[index] = cell;
            
            
            //设置水平方向上格子相邻关系
            if (x > 0)
            {
                mCells[index].SetNeighbor(HexDirection.W,mCells[index - 1]);
            }
            if (z > 0)
            {
                //偶数行
                if (z % 2 == 0)
                {
                    mCells[index].SetNeighbor(HexDirection.SE,mCells[index - GridWidth]);
                    //偶数行第一个格子没有SW方向邻居
                    if (x > 0)
                    {
                        mCells[index].SetNeighbor(HexDirection.SW,mCells[index - GridWidth - 1]);
                    }
                }
                else
                {
                    mCells[index].SetNeighbor(HexDirection.SW,mCells[index - GridWidth]);
                    //奇数行最后一个格子没有SE方向邻居
                    if (x < GridWidth - 1)
                    {
                        mCells[index].SetNeighbor(HexDirection.SE,mCells[index - GridWidth + 1]);
                    }
                }
            }
            

            //创建cell坐标lable
            CreateCellLabel(cell,index);
        }
        
        /// <summary>
        /// 显示坐标label
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        private void CreateCellLabel(HexCell cell,int cellIndex)
        {
            var cellLocalPos = cell.transform.localPosition;
            var cellLabel = Instantiate(CellLablePrefab, GridCanvas.transform, false);
            cellLabel.name = cell.HexCoord.ToString();
            TMP_Text tmpText = cellLabel.GetComponent<TMP_Text>();
            tmpText.rectTransform.anchoredPosition = new Vector2(cellLocalPos.x, cellLocalPos.z);
            tmpText.text = cell.HexCoord.ToString() + "\ncell: " + cellIndex ;
            
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
                HandleInput();
        }

        void HandleInput()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                TouchCell(hitInfo.point);
            }
        }

        void TouchCell(Vector3 touchWorldPos)
        {
            var touchPos = transform.InverseTransformPoint(touchWorldPos);
            HexCoord coord = HexCoord.FromPosition(touchPos);
            Debug.Log("touchPos:" + touchPos + "coord: " + coord);
            
            //计算点击的cell index 注意:随着Z坐标每增加2 X坐标会减1
            int index = coord.X + coord.Z * GridWidth + coord.Z / 2;
            //修改颜色 重建GridMesh
            HexCell clickedCell = mCells[index];
            clickedCell.CellColor = TouchColor;
            
            //测试
            // for (int i = 0; i < clickedCell.neighbors.Length; i++)
            // {
            //     if (clickedCell.neighbors[i] != null)
            //     {
            //         clickedCell.neighbors[i].CellColor = TouchColor;
            //     }
            // }
            
            mHexMesh.BuildMesh(mCells);
        }
    }
}