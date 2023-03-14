using System.Collections;
using UnityEngine;

public class CoroutineProvider : MonoBehaviour
{
    public Coroutine RunCoroutine(IEnumerator coroutine, bool stopPreviousCoroutine = false)
    {
        return StartCoroutine(coroutine);
    }

    public void Stop(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
