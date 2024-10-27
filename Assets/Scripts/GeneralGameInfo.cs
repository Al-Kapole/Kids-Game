using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGameInfo
{
    private static GeneralGameInfo instance;
    public static GeneralGameInfo Instance
    {
        get
        {
            return instance == null ? instance = new GeneralGameInfo() : instance;
        }
    }


    public Coroutine Tremple(MonoBehaviour _corrBase, Transform _transform, float _tremplePower, float _sec)
    {
        return _corrBase.StartCoroutine(Tremple(_transform, _tremplePower, _sec)); 
    }
    private IEnumerator Tremple(Transform _transform, float _tremplePower, float _sec)
    {
        Vector3 originalPos = _transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / _sec;
            _transform.position = originalPos + new Vector3(Random.Range(-_tremplePower, _tremplePower), Random.Range(-1f, 1f));
            yield return null;
        }
        _transform.position = originalPos;
    }
}
