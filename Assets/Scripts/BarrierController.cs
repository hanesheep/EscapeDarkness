using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f;

    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
