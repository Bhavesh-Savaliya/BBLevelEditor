using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleBandits
{

    public class MapHolder : ScriptableObject
    {
        public List<MapData> mapData;

        public void RefreshMapDatabase()
        {
            mapData.Clear();
            MapData[] mapDataArray = ((MapData[])Resources.LoadAll<MapData>("Maps/") as MapData[]);
            mapData.AddRange(mapDataArray.ToList());

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);

            UnityEditor.AssetDatabase.Refresh();
            for (int i = 0; i < mapData.Count; i++)
            {
                UnityEditor.EditorUtility.SetDirty(mapData[i]);
                mapData[i].mapName = mapDataArray[i].name;
                mapData[i].levelNumber = System.Convert.ToInt32(mapData[i].mapName);
            }
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public MapData GetMapData(string mapName)
        {
            for (int i = 0; i < mapData.Count; i++)
            {
                if (mapData[i].name == mapName)
                {
                    return mapData[i];
                }
            }
            return null;
        }

        public MapData GetMapData(int levelNumber)
        {
            for (int i = 0; i < mapData.Count; i++)
            {
                // Debug.LogFormat("level number {0}. map number {1}", levelNumber, mapData[i].levelNumber);

                if (levelNumber.Equals(mapData[i].levelNumber))
                {
                    return mapData[i];
                }
            }
            return mapData[0];
        }

        public void AddMap(MapData mapData)
        {
            this.mapData.Add(mapData);
        }

        public void RemoveMap(MapData mapData)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(mapData));
#endif
            this.mapData.Remove(mapData);
        }

        public bool MapAlreadyExists(string mapName)
        {
            bool doesExist = false;
            for (int i = 0; i < mapData.Count; i++)
            {
                if (mapData[i].mapName == mapName)
                {
                    doesExist = true;
                    break;
                }
            }
            return doesExist;
        }
    }

}
