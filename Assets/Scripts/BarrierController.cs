using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f;

    void Start()
    {
        SoundManager.instance.SEPlay(SEType.Barrier); //�o���A������������

        Destroy(gameObject, deleteTime);
    }
}
