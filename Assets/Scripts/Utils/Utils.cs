using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.Utils {

    public enum CellColor {
        CellWhite = -1,
        CellBlue = 1,
        CellCyan = 2,
        CellGreen = 3,
        CellOrange = 4,
        CellPurple = 5,
        CellRed = 6,
        CellYellow = 7
    }

    public static class Utils {

        static string materialsPath = "Materials/";
        static string prefabsPath = "Prefabs/";

        public static Material GetMaterial (CellColor cellColor)
        {
            Material materialFound = Resources.Load(
                materialsPath + cellColor.ToString(),
                typeof( Material )
            ) as Material;

            if (materialFound == null)
                Debug.LogError( "Couldn't find resource for Cellcolor " + cellColor.ToString() );

            return materialFound;
        }

        public static GameObject GetPrefab (string prefabName)
        {
            GameObject prefabFound = Resources.Load(
                prefabsPath + prefabName,
                typeof( GameObject )
            ) as GameObject;

            if (prefabFound == null)
                Debug.LogError( "Couldn't find prefab with name " + prefabName );

            return prefabFound;
        }


        public static List<Vector2Int> CopyCells (List<Vector2Int> cellListToCopy)
        {
            List<Vector2Int> copiedList = new List<Vector2Int>();

            for (int i = 0; i < cellListToCopy.Count; i++)
                copiedList.Add( new Vector2Int( cellListToCopy [ i ].x, cellListToCopy [ i ].y ) );

            return copiedList;
        }

        public static void AddOffset (List<Vector2Int> cellList, Vector2Int offset)
        {
            for (int i = 0; i < cellList.Count; i++)
                cellList [ i ] += offset;
        }

    }

}