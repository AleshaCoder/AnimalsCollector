using UnityEngine;

public class Place : MonoBehaviour
{ 
    [SerializeField] private bool _free = true;

    public bool IsFree => _free;

    public void Take()
    {
        if (IsFree == false)
        {
            Debug.LogError("Place already have taken. Free place for next using");
            return;
        }
        _free = false;
    }

    public void Free()
    {
        if (IsFree == true)
        {
            Debug.LogWarning("Place already free.");
            return;
        }
        _free = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
