using UnityEngine;
using System.IO;


//apparantly Json can´t use multi-dimensional arrays by default
//which is why I decided to make 8 regular arrays, For each columns one
//One big 1d array would also have worked, but the format was really unclear, so I decided to do this instead.

//Loads the config file, processes it and passes the Information on, initializes the Game State and then starts the "Game"
public class LoadConfig : MonoBehaviour
{
    private class ConfigData
    {
        public bool[] GridDataRow1;
        public bool[] GridDataRow2;
        public bool[] GridDataRow3;
        public bool[] GridDataRow4;
        public bool[] GridDataRow5;
        public bool[] GridDataRow6;
        public bool[] GridDataRow7;
        public bool[] GridDataRow8;


    }


    private Vector2Int _arraySize = new Vector2Int(8,8);
    private bool[,] _gridPositions;

    private GridManager _gridManager;

    //Serves as the parent to the cubes to get rid of the position deterrend and to organize the unity Hierachy.
    [SerializeField, Tooltip("Put in the  CubeHolder Object")]
    private GameObject _cubeHolder;

    [SerializeField, Tooltip("Put in the moveable Cube prefab")]
    private GameObject _cubePrefab;

    [SerializeField, Tooltip("Put in the Selector object")]
    private GameObject _selector;

    

    //First thing to be called in all of the game
    public void Awake()
    {
        _gridManager = new GridManager();

        ProcessConfigData(LoadFile());
        //SaveData();
        PassInformations();
        InitializeGameState();
        StartGame();
    }

    //Loads information from The JSON config file in this case
    //could be made to accept Filenames and used as general function in bigger projects
    private ConfigData LoadFile()
    {
        string json = File.ReadAllText(Application.dataPath + "/Config.json");
        ConfigData loadedConfigData = JsonUtility.FromJson<ConfigData>(json);

        return loadedConfigData;
        

    }

    //Converts the 8 array into the 2d array
    private void ProcessConfigData(ConfigData pConfigData)
    {
        _gridPositions = new bool[_arraySize.x, _arraySize.y];
        bool[] _dataToFillIn = new bool[8];

        for (int i = 0; i < _gridPositions.GetLength(0); i++)
        {
            switch(i)
            {

                case 1:
                    _dataToFillIn = pConfigData.GridDataRow1;
                    break;
                case 2:
                    _dataToFillIn = pConfigData.GridDataRow2;
                    break;
                case 3:
                    _dataToFillIn = pConfigData.GridDataRow3;
                    break;
                case 4:
                    _dataToFillIn = pConfigData.GridDataRow4;
                    break;
                case 5:
                    _dataToFillIn = pConfigData.GridDataRow5;
                    break;
                case 6:
                    _dataToFillIn = pConfigData.GridDataRow6;
                    break;
                case 7:
                    _dataToFillIn = pConfigData.GridDataRow7;
                    break;
                default:
                    _dataToFillIn = pConfigData.GridDataRow8;
                    break;
            }
            for (int k = 0; k < _gridPositions.GetLength(1); k++)
            {
                _gridPositions[i, k] = _dataToFillIn[k];
            }
        }
    }
    //meant for testing only
    //Could be starting point to save game sessions of course, but that would not happen with a config file but an encrypted SaveFile
    public void SaveData()
    {
        ConfigData configData = new ConfigData();

        configData.GridDataRow1 = new bool[_arraySize.x];
        configData.GridDataRow2 = new bool[_arraySize.x];
        configData.GridDataRow3 = new bool[_arraySize.x];
        configData.GridDataRow4 = new bool[_arraySize.x];
        configData.GridDataRow5 = new bool[_arraySize.x];
        configData.GridDataRow6 = new bool[_arraySize.x];
        configData.GridDataRow7 = new bool[_arraySize.x];
        configData.GridDataRow8 = new bool[_arraySize.x];


        string json = JsonUtility.ToJson(configData);

        File.WriteAllText(Application.dataPath + "/Config.json", json);
        Debug.Log(json);

    }



    //sends information to classes requiring it (in this case only GridManager)
    private void PassInformations()
    {
        _gridManager.Initialize(_gridPositions);
    }

    //Iterates over the Loaded array and Initializes the cubes based on the information
    //As described in the task this does Spawn the cubes. Pooling might be an option but I decided against it because I believe loading them once is better then increasing the game size i this case. Even though it does not matter in these dimensions at all
    private void InitializeGameState()
    {
        for (int i = 0; i< _gridPositions.GetLength(0); i++)
        {
            for (int k = 0; k < _gridPositions.GetLength(1); k++)
            {
                if (_gridPositions[i, k] == true)
                {
                    GameObject cube = Instantiate(_cubePrefab, new Vector3(i, 0, k) - _cubeHolder.transform.position, Quaternion.identity, _cubeHolder.transform);
                    CubeBehaviour cubeBehaviour = cube.GetComponent<CubeBehaviour>();
                    cubeBehaviour.Initialize(new Vector2(i, k));
                    cubeBehaviour.enabled = false;
                }
            }
        }
    }

    //Enables Objects and scripts for the Game to start
    private void StartGame()
    {

        _selector.SetActive(true);
    }



}
