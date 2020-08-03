using UnityEngine;

public class Selector : MonoBehaviour
{
    //keeps track of the currently selected object. Is null when no object is selected
    private CubeBehaviour _selectedCube = null;
    private float _rayCastLength = 10;
    [SerializeField]
    private LayerMask layerMask;

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckRayCastTarget();
        }
    }
    //Checks with a RayCast whether a suitable target has been found. 
    private void CheckRayCastTarget()
    {
        if (_selectedCube != null)
        {
            _selectedCube.enabled = false;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _rayCastLength, layerMask))
        {
            SelectTarget(hit);
        }
        else
        {
            _selectedCube = null;
        }

    }

    //enables the cubeBehaviour of the target, which activates it and implicitly selects it
    private void SelectTarget(RaycastHit pHit)
    {
        _selectedCube = pHit.transform.gameObject.GetComponent<CubeBehaviour>();
        _selectedCube.enabled = true;
    }
}
