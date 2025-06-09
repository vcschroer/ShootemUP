using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Tempo em segundos até o objeto ser destruído")]
    [SerializeField] private float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
