using UnityEngine;


//Handles where  all the cubes are and handles movement requests, looking for them whether it is possible.
//This is only a managing script it does not need Monobehaviour functions
public class GridManager 
{
    //if a field in this array is true it means that the field is taken
    //static so that no matter which instacne is updated, all instances are Up to date
    private static bool[,] STCgridPositions;

    private static bool STCisInitialized = false;
    public void Initialize(bool[,] pGridPositions)
    {
        if (!STCisInitialized)
        {
            STCisInitialized = true;
            STCgridPositions = pGridPositions;
        }
        else
        {
            Debug.Log("Unauthorized initialisation detected and denied");
        }
    }

    //Looks whether the new coordinates are valid and whether the sapce is already taken, if not it updates the position and executes internally(no concern for the cube itself)
    public bool Movepermission(int pNewGridX, int pNewGridY, int pOldGridX, int pOldGridY)
    {
        bool permissionGranted = false;


        //Debug.Log("potential new x" + pNewGridX + "  potential new y" + pNewGridY + "  old x" + pOldGridX + "  old y" + pOldGridY);
        if (pNewGridX >= 0 && pNewGridY >= 0 && pNewGridX < STCgridPositions.GetLength(0) && pNewGridY < STCgridPositions.GetLength(1))
        {
            if(!STCgridPositions[pNewGridX, pNewGridY])
            {
                permissionGranted = true;
                UpdatePositionGrid(pNewGridX, pNewGridY, pOldGridX, pOldGridY);
            }
        }
        //Debug.Log(permissionGranted);
        return permissionGranted;
    }

    //Moves the cubes information in the array around.
    private void UpdatePositionGrid(int pNewGridX, int pNewGridY, int pOldGridX, int pOldGridY)
    {
        STCgridPositions[pNewGridX, pNewGridY] = true;
        STCgridPositions[pOldGridX, pOldGridY] = false;
    }
}
