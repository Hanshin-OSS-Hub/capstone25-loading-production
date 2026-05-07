using UnityEngine;

public class BarrierSkillObject : MonoBehaviour
{
    [SerializeField] private float duration = 3f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}