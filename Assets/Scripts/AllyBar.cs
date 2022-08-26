using UnityEngine;

public class AllyBar : MonoBehaviour
{
    private bool alreadyPassed;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (alreadyPassed)
            return;
        if (col.CompareTag("Ball"))
        {
            FindObjectOfType<MapManager>().AllyBarPassed();
            alreadyPassed = true;
        }
    }
 
    public void SetPassed()
    {
        if (alreadyPassed)
            return;
        FindObjectOfType<MapManager>().AllyBarPassed();
        alreadyPassed = true;
    }
}
