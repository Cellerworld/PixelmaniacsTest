using System.Collections;
using UnityEngine;

//Moves the Cube in single steps through the grid by using WASD
public class CubeBehaviour : MonoBehaviour
{
    private GridManager _gridManager;
    private static float _timeNeededforMove;
    private Vector3 _moveDirection;
    private Vector2 _gridPos;

    private bool _isMoving;
    private bool _isInitialized;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private Renderer _myRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _myRenderer = this.GetComponent<Renderer>();
        _gridManager = new GridManager();
        _isMoving = false;
        _timeNeededforMove = 0.5f;
    }

    //Marks the object visually as selected 
    private void OnEnable()
    {
        _myRenderer.material.SetColor("_Color", Color.red);
    }

    //Marks the object visually as deselected 
    private void OnDisable()
    {
        _myRenderer.material.SetColor("_Color", Color.blue);
    }

    //Initialize teh objetct with its position
    public void Initialize(Vector2 pGridPos)
    {
        if(!_isInitialized)
        {
            _isInitialized = true;
            _gridPos = pGridPos;
        }
        else
        {
            Debug.Log("Unauthorized initialisation detected and denied");
        }
    }

    //To avoid every cube to run update every frame, I deactivate all non selected cubes
    void Update()
    {
        if (!_isMoving)
        {
            CheckMoveRequest();
        }
    }

    //Checks whetehr there is a moving request, if yes then it asks for permission form the GridManager and starts the movement coroutine when allowed.
    private void CheckMoveRequest()
    {
        //Currently this allows movement with arrow keys aswell, can be taken out in teh project settings -> input, but I do not think it is necessary
        //also allows diagonal movement. I simply don´t know if the tasks asks for that or not, but if not, it can be solved by checking on the sum of the _moveDirection values, if it is bigger than 1, decide on a priority axis and set the other to 0.
        //Alternitively you could also say that is not a valid input and not move (add optional feedback to inform the player).
        _moveDirection = new Vector3(Mathf.CeilToInt(Input.GetAxis("Horizontal")), 0, Mathf.CeilToInt(Input.GetAxis("Vertical")));
        if (_moveDirection.magnitude > 0 && _gridManager.Movepermission((int)(_gridPos.x + _moveDirection.x),
                                                                        (int)(_gridPos.y + _moveDirection.z),
                                                                       (int)(_gridPos.x),
                                                                       (int)(_gridPos.y))
            )
        {
            StartCoroutine("Move");
        }
    }

    //Moves the objects steadily. The distance travelled here is based on my knowledge that the distances are 1.
    //Makign an extra variable to multiply the _moveDirection with 1 did seem unnecessary. Giving an option to scale the baord and cubes does also not add anything to the game, which is why I went with this
    private IEnumerator Move()
    {
        _isMoving = true;
        Vector3 startPos = this.transform.position;
        Vector3 endPos = startPos + _moveDirection;
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / _timeNeededforMove;
            this.transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return _waitForEndOfFrame;
        }
        _isMoving = false;
        _gridPos += new Vector2(_moveDirection.x, _moveDirection.z);
        yield return null;
    }
}
