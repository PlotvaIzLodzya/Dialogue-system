using UnityEngine;

public class UnityTranslator: MonoBehaviour
{
    [SerializeField] public CoroutineProvider _coroutineProvider;

    public static CoroutineProvider CoroutineProvider { get; private set; }

    private void Awake()
    {
        CoroutineProvider = _coroutineProvider;
    }
}
