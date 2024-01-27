using System;
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
        public int width = 6;
        public int height = 6;
        public HexCell CellPrefab;
        
        /// <summary>
        /// 每个cell坐标显示
        /// </summary>
        public GameObject CellLablePrefab;

        public Canvas GridCanvas;
    
        //点击颜色
        public Color defaultColor = Color.white;
        public Color TouchColor = Color.blue;

        private HexMesh mHexMesh;
        
        private HexCell[] mCells;
        
        
        private void Awake()
        {
            mHexMesh = GetComponentInChildren<HexMesh>();
            
            mCells = new HexCell[width * height];

            int index = 0;
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCellAt(x, z, index++);
                }
            }
        }

        private void Start()
        {
            mHexMesh.BuildMesh(mCells);
        }

        private void CreateCellAt(int x, int z, int index)
        {
            Vector3 localPos;
            localPos.x = (x + (z % 2 == 1 ? 0.5f : 0 )) * (HexMetrics.innerRadius * 2);     //x坐标是内半径的2倍
            localPos.z = z * (HexMetrics.outerRadius * 1.5f);  //z坐标是外半径的1.5倍
            localPos.y = 0;
            
            var cell = GameObject.Instantiate<HexCell>(CellPrefab, transform, false);
            cell.transform.localPosition = localPos;
            cell.GetComponent<MeshRenderer>().enabled = false;
            cell.HexCoord = HexCoord.FromOffsetCoord(x,z);
            cell.CellColor = defaultColor;
            
            mCells[index] = cell;

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
            int index = coord.X + coord.Z * width + coord.Z / 2;
            //修改颜色 重建GridMesh
            HexCell clickedCell = mCells[index];
            clickedCell.CellColor = TouchColor;
            mHexMesh.BuildMesh(mCells);
        }
    }
}