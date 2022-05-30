
using UnityEditor;
using UnityEngine;

namespace BattleBandits
{


    public class MapEditor : EditorWindow
    {
        [MenuItem("[Custom Tools]/Map Editor #m", false, 0)]
        internal static void Init()
        {

            var window = (MapEditor)GetWindow(typeof(MapEditor), false, "Map Editor");
            mapData = null;
            levelName = "";
            mapHolder = (MapHolder)Resources.Load<MapHolder>("MapHolder");
            mapContentData = (MapContent)Resources.Load<MapContent>("MapPropContent");
            delete = false;
            CheckExistingParent();
            currentDirection = Direction.Down;
            LoadTextures();
        }
        static Texture2D ground;
        static Texture2D destructible;
        static Texture2D nonDestructible;
        static Texture2D decorative;
        static Texture2D player;
        static Texture2D enemy;
        static Texture2D cliff1up;
        static Texture2D cliff2up;
        static Texture2D cliff3up;

        static Texture2D cliff1down;
        static Texture2D cliff2down;
        static Texture2D cliff3down;

        static Texture2D cliff1right;
        static Texture2D cliff2right;
        static Texture2D cliff3right;

        static Texture2D cliff1left;
        static Texture2D cliff2left;
        static Texture2D cliff3left;

        static Texture2D paintBrush;

        static Texture2D cliff;

        static string iconPathPrefix = "Icons/";
        static void LoadTextures()
        {
            ground = Resources.Load(iconPathPrefix + "ground", typeof(Texture2D)) as Texture2D;
            destructible = Resources.Load(iconPathPrefix + "destructible", typeof(Texture2D)) as Texture2D;
            nonDestructible = Resources.Load(iconPathPrefix + "nonDestructible", typeof(Texture2D)) as Texture2D;
            decorative = Resources.Load(iconPathPrefix + "decorative", typeof(Texture2D)) as Texture2D;
            player = Resources.Load(iconPathPrefix + "player", typeof(Texture2D)) as Texture2D;
            enemy = Resources.Load(iconPathPrefix + "enemy", typeof(Texture2D)) as Texture2D;
            cliff = Resources.Load(iconPathPrefix + "cliff", typeof(Texture2D)) as Texture2D;

            cliff1up = Resources.Load(iconPathPrefix + "cliff1up", typeof(Texture2D)) as Texture2D;
            cliff2up = Resources.Load(iconPathPrefix + "cliff2up", typeof(Texture2D)) as Texture2D;
            cliff3up = Resources.Load(iconPathPrefix + "cliff3up", typeof(Texture2D)) as Texture2D;

            cliff1down = Resources.Load(iconPathPrefix + "cliff1down", typeof(Texture2D)) as Texture2D;
            cliff2down = Resources.Load(iconPathPrefix + "cliff2down", typeof(Texture2D)) as Texture2D;
            cliff3down = Resources.Load(iconPathPrefix + "cliff3down", typeof(Texture2D)) as Texture2D;

            cliff1left = Resources.Load(iconPathPrefix + "cliff1left", typeof(Texture2D)) as Texture2D;
            cliff2left = Resources.Load(iconPathPrefix + "cliff2left", typeof(Texture2D)) as Texture2D;
            cliff3left = Resources.Load(iconPathPrefix + "cliff3left", typeof(Texture2D)) as Texture2D;

            cliff1right = Resources.Load(iconPathPrefix + "cliff1right", typeof(Texture2D)) as Texture2D;
            cliff2right = Resources.Load(iconPathPrefix + "cliff2right", typeof(Texture2D)) as Texture2D;
            cliff3right = Resources.Load(iconPathPrefix + "cliff3right", typeof(Texture2D)) as Texture2D;

            paintBrush = Resources.Load(iconPathPrefix + "Brush", typeof(Texture2D)) as Texture2D;
        }

        public CellContentType currentContentType;
        const int btnSize = 20;
        const int gridSize = 60;

        public static MapHolder mapHolder;
        public static MapData mapData;
        public static MapContent mapContentData;
        static bool delete;
        static bool paint;
        static Direction currentDirection;
        static string levelName = "";

        Color gridColor = Color.white;
        void OnDeleteEvent()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Tab)
                {
                    delete = !delete;
                }
            }
            if (delete)
            {
                gridColor = Color.yellow;
            }
            else
            {
                gridColor = Color.white;
            }
        }

        void OnPaintEvent()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.F)
                {
                    paint = !paint;
                }
            }
        }

        void OnArrowKeysEvents()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.UpArrow)
                {
                    currentDirection = Direction.Up;
                }

                if (e.keyCode == KeyCode.DownArrow)
                {
                    currentDirection = Direction.Down;

                }

                if (e.keyCode == KeyCode.LeftArrow)
                {

                    currentDirection = Direction.Left;
                }

                if (e.keyCode == KeyCode.RightArrow)
                {
                    currentDirection = Direction.Right;

                }
            }
        }
        void Update()
        {
            Repaint();
        }


        Vector2 gridScrollPos;
        void OnGUI()
        {
            OnDeleteEvent();
            OnPaintEvent();
            OnArrowKeysEvents();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(300));
                {
                    if (mapData)
                        DrawContentButtons();

                    EditorGUILayout.Separator();

                    DrawDataButtons();

                    EditorGUILayout.Separator();

                    DrawOperationButtons();

                    EditorGUILayout.Separator();


                    DrawMapLoadButtons();
                    EditorGUILayout.EndVertical();
                }

                gridScrollPos = EditorGUILayout.BeginScrollView(gridScrollPos);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    if (mapData)
                    {
                        DrawGrid();
                    }
                    else
                    {
                        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

                        GUILayout.Box("LOAD OR CREATE MAP", style, GUILayout.Width(950), GUILayout.Height(950));
                    }

                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
            }


        }

        void DrawDataButtons()
        {
            if (mapData)
            {
                mapData.maxEnemies = EditorGUILayout.IntField("Max enemies: ", mapData.maxEnemies);
                mapData.initEnemies = EditorGUILayout.IntField("Init enemies: ", mapData.initEnemies);
                GUILayout.Space(10);
                mapData.snowLevel = EditorGUILayout.FloatField("Snow Level: ", mapData.snowLevel);
            }
        }

        private void DrawGrid()
        {
            GUI.color = gridColor;
            Event e = Event.current;
            for (int y = 0; y < gridSize; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < gridSize; x++)
                {
                    //if cell has at least one content
                    Texture2D cellTexture = null;
                    if (mapData.cells[(y * gridSize) + x].cellContent.cellContentType != CellContentType.None)
                    {
                        cellTexture = GetCellTexture(mapData.cells[(y * gridSize) + x].cellContent.cellContentType, mapData.cells[(y * gridSize) + x].cellContent.direction);

                    }


                    if (GUILayout.Button(cellTexture, GUI.skin.box, GUILayout.Height(btnSize), GUILayout.Width(btnSize)))
                    {
                        OnCellClicked(y, x);
                    }
                    if (paint)
                    {

                        // if (e.type == EventType.KeyDown)
                        // {
                        //     if (e.keyCode == KeyCode.Space)
                        //     {
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        Vector2 rectPos = new Vector2(lastRect.x + btnSize / 2f, lastRect.y + btnSize / 2f);
                        //rectPos = GUIUtility.GUIToScreenPoint(rectPos);
                        Vector2 mousePos = (Event.current.mousePosition);
                        float distance = Vector2.Distance(rectPos, mousePos);

                        Rect mouseRect = new Rect(mousePos.x + 10, mousePos.y + 10, btnSize, btnSize);
                        GUI.DrawTexture(mouseRect, paintBrush, ScaleMode.ScaleAndCrop, true);
                        if (distance < (btnSize * 1.4f) / 2f)
                        {
                            OnCellClicked(y, x);
                        }
                        // }
                        // }
                    }


                }
                EditorGUILayout.EndHorizontal();
            }
            GUI.color = Color.white;
        }

        private void OnCellClicked(int x, int y)
        {
            int cellNumber = (x * gridSize) + y;
            //Debug.LogFormat("{0}, {1} = {2}", x, y, cellNumber);
            //Debug.Log(mapData.cells[cellNumber].position);

            if (delete)
            {
                mapData.RemoveContent(cellNumber);
            }
            else
            {
                mapData.AddContent(cellNumber, currentContentType, currentDirection);
            }
        }

        private void DrawContentButtons()
        {
            foreach (CellContentType cct in System.Enum.GetValues(typeof(CellContentType)))
            {
                if (cct == currentContentType)
                {
                    GUI.color = Color.grey;
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(cct.ToString()))
                {
                    currentContentType = cct;
                }
                GUILayout.Box(GetCellTexture(cct), GUILayout.Height(btnSize), GUILayout.Width(btnSize));
                EditorGUILayout.EndHorizontal();

                GUI.color = Color.white;
            }
        }

        private void DrawOperationButtons()
        {
            if (!mapData)
            {
                GUILayout.Label("Create a new map:");
            }
            else
            {
                GUILayout.Label("Level Number: " + mapData.levelNumber);

            }

            if (!mapData)
            {
                levelName = GUILayout.TextField(levelName);
                if (GUILayout.Button("New"))
                {
                    if (string.IsNullOrWhiteSpace(levelName))
                    {
                        Debug.Log("Enter valid name!");
                    }
                    else
                    {

                        //check if level with same name exists
                        if (!mapHolder.MapAlreadyExists(levelName))
                        {
                            mapData = (MapData)ScriptableObjectUtility.CreateAsset<MapData>("Assets/Resources/Maps", levelName);
                            mapData.mapName = levelName.ToString();
                            mapData.SetCellPositions();
                            mapHolder.AddMap(mapData);
                        }
                        else
                        {
                            Debug.Log("Map Already Exists");
                        }
                    }
                }
            }

            if (GUILayout.Button("Refresh"))
            {
                mapHolder.RefreshMapDatabase();
            }

            if (mapData)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Generate Map"))
                {
                    GenerateMap();
                }
                if (parent)
                {
                    if (GUILayout.Button("X", GUILayout.MaxWidth(btnSize)))
                    {
                        ClearSpawnedMap();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Close"))
                {
                    mapData = null;
                }
                if (GUILayout.Button("Clear"))
                {
                    if (EditorUtility.DisplayDialog("Clearing level data!",
                                   "Clear level data forreal, bro? ", "Yes", "No"))
                    {

                        for (int i = 0; i < gridSize * gridSize; i++)
                        {
                            mapData.RemoveContent(i);
                        }
                    }
                }
            }

        }

        Vector2 scrollPos;

        void DrawMapLoadButtons()
        {
            if (!mapHolder) return;
            if (mapHolder.mapData.Count > 0)
                GUILayout.Label("Load");

            for (int i = 0; i < mapHolder.mapData.Count; i++)
            {
                GUI.backgroundColor = Color.yellow;

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(mapHolder.mapData[i].name))
                {
                    //already loaded
                    if (mapData && mapHolder.mapData[i].mapName == mapData.mapName)
                    {
                        Debug.Log("Already loaded");
                    }
                    else
                    {
                        mapData = mapHolder.GetMapData(mapHolder.mapData[i].name);

                    }

                }


                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("x", GUILayout.MaxWidth(btnSize)))
                {
                    mapHolder.RemoveMap(mapHolder.mapData[i]);
                }
                EditorGUILayout.EndHorizontal();

            }

            GUI.backgroundColor = Color.white;
        }

        public Texture2D GetCellTexture(CellContentType cct, Direction dir = Direction.None)
        {

            if (dir == Direction.None)
            {
                dir = currentDirection;
            }
            Texture2D tex = null;

            switch (cct)
            {
                case CellContentType.Ground:
                    tex = ground;
                    break;
                case CellContentType.Destructible:
                    tex = destructible;
                    break;
                case CellContentType.NonDestructible:
                    tex = nonDestructible;
                    break;
                case CellContentType.Decorative:
                    tex = decorative;
                    break;
                case CellContentType.Player:
                    tex = player;
                    break;
                case CellContentType.Enemy:
                    tex = enemy;
                    break;
                case CellContentType.Cliff:
                    tex = cliff;
                    break;

                case CellContentType.Cliff1:

                    switch (dir)
                    {
                        case Direction.Up:
                            tex = cliff1up;
                            break;

                        case Direction.Down:
                            tex = cliff1down;

                            break;

                        case Direction.Left:
                            tex = cliff1left;

                            break;

                        case Direction.Right:
                            tex = cliff1right;

                            break;
                    }

                    break;

                case CellContentType.Cliff2:
                    switch (dir)
                    {
                        case Direction.Up:
                            tex = cliff2up;
                            break;

                        case Direction.Down:
                            tex = cliff2down;

                            break;

                        case Direction.Left:
                            tex = cliff2left;

                            break;

                        case Direction.Right:
                            tex = cliff2right;

                            break;
                    }
                    break;

                case CellContentType.Cliff3:
                    switch (dir)
                    {
                        case Direction.Up:
                            tex = cliff3up;
                            break;

                        case Direction.Down:
                            tex = cliff3down;

                            break;

                        case Direction.Left:
                            tex = cliff3left;

                            break;

                        case Direction.Right:
                            tex = cliff3right;

                            break;
                    }
                    break;
            }
            return tex;
        }

        static GameObject parent;

        private static void CheckExistingParent()
        {
            if (mapData)
                parent = GameObject.Find(mapData.name);
        }

        public void ClearSpawnedMap()
        {
            if (!parent)
            {
                parent = new GameObject(mapData.name);
            }
            else
            {
                for (int i = parent.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(parent.transform.GetChild(i).gameObject);
                }
                DestroyImmediate(parent.gameObject);
            }
        }

        public void GenerateMap()
        {
            ClearSpawnedMap();
            if (!parent)
            {
                parent = new GameObject(mapData.name);
            }

            int[] dirAngle = { 0, 0, 180, 270, 90 };

            for (int i = 0; i < mapData.cells.Length; i++)
            {
                Cell cell = mapData.cells[i];
                CellContent cellContent = cell.cellContent;
                CellContentType cellContentType = cellContent.cellContentType;

                if (cellContentType == CellContentType.None)
                {
                    continue;
                }

                int randomIndex = Random.Range(0, mapContentData.contents[(int)cellContent.cellContentType].Objects.Length);

                GameObject prop = mapContentData.contents[(int)cellContent.cellContentType].Objects[randomIndex];

                if (prop)
                {
                    if (cellContentType != CellContentType.Ground)
                    {
                        if ((int)cellContentType < (int)CellContentType.Cliff)
                        {
                            // placing ground below other props, excluding cliffs
                            randomIndex = Random.Range(0, mapContentData.contents[(int)CellContentType.Ground].Objects.Length);
                            GameObject groundProp = mapContentData.contents[(int)CellContentType.Ground].Objects[randomIndex];
                            Vector3 GroundPosition = new Vector3(cell.position.y - (gridSize / 2), groundProp.transform.position.y, -cell.position.x + (gridSize / 2));
                            Instantiate(groundProp, GroundPosition, Quaternion.Euler(new Vector3(0, dirAngle[Random.Range(1, dirAngle.Length)], 0)), parent.transform);
                        }
                    }

                    Vector3 propPosition = new Vector3(cell.position.y - (gridSize / 2), prop.transform.position.y, -cell.position.x + (gridSize / 2));
                    Quaternion propRotation;

                    switch (cellContentType)
                    {
                        case CellContentType.Destructible:
                            propRotation = Quaternion.Euler(new Vector3(0, dirAngle[Random.Range(1, dirAngle.Length)], 0));
                            break;

                        case CellContentType.NonDestructible:
                            propRotation = Quaternion.Euler(new Vector3(0, dirAngle[Random.Range(1, dirAngle.Length)], 0));
                            break;

                        case CellContentType.Decorative:
                            propRotation = Quaternion.Euler(new Vector3(0, dirAngle[Random.Range(1, dirAngle.Length)], 0));
                            break;

                        case CellContentType.Ground:
                            propRotation = Quaternion.Euler(new Vector3(0, dirAngle[Random.Range(1, dirAngle.Length)], 0));
                            break;

                        default:
                            // for All type of cliffs
                            propRotation = Quaternion.Euler(new Vector3(0, dirAngle[(int)cellContent.direction], 0));
                            break;
                    }

                    //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), propPosition, propRotation, parent.transform);
                    Instantiate(prop, propPosition, propRotation, parent.transform);
                }
            }

        }
    }
}