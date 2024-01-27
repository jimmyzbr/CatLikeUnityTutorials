using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace CatLike.HexMap.Codes
{
    /// <summary>
    /// Hexmap Grid 
    /// </summary>
    public class HexGridUsePlane : MonoBehaviour
    {
        public int width = 6;
        public int height = 6;
        public HexCell CellPrefab;
        
        /// <summary>
        /// 每个cell坐标显示
        /// </summary>
        public GameObject CellLablePrefab;

        public Canvas LableCanvas;
        
        
        private HexCell[] mCells;
        
        private void Awake()
        {
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

        private void CreateCellAt(int x, int z, int index)
        {
            //var localPos = new Vector3(x, 0, z) * 10;  //标准正方形网格

            Vector3 localPos;
            localPos.x = (x + (z % 2 == 1 ? 0.5f : 0 )) * (HexMetrics.innerRadius * 2);     //x坐标是内半径的2倍
            localPos.z = z * (HexMetrics.outerRadius * 1.5f);  //z坐标是外半径的1.5倍
            localPos.y = 0;
            
            var cell = GameObject.Instantiate(CellPrefab, transform, false);
            cell.transform.localPosition = localPos;
            mCells[index] = cell;

            CreateCellLabel(x, z,localPos);
        }

        
        /// <summary>
        /// 显示坐标label
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        private void CreateCellLabel(int x, int z,Vector3 cellLocalPos)
        {
            var cellLabel = Instantiate(CellLablePrefab, LableCanvas.transform, false);
            cellLabel.name = $"[{x},{z}]";
            TMP_Text tmpText = cellLabel.GetComponent<TMP_Text>();
            tmpText.rectTransform.anchoredPosition = new Vector2(cellLocalPos.x, cellLocalPos.z);
            tmpText.text = $"{x},{z}";
        }
    }
}