using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LambdaTween : MonoBehaviour
{
    public delegate void FUNCRUN(float fTime, params object[] args);
    public delegate void FUNCDONE(params object[] args);

    private class LambdaTweener
    {
        public bool bPause = false;
        public bool bStop = false;
        public bool bFinish = false;
        public float fTime = 1.0f;
        public float fTimeScale = 1.0f;
        public Action<float> pFuncrun = null;
        public Action pFuncdone = null;
        public void Run()
        {
            if (bStop)
            {
                bFinish = true;
                pFuncdone?.Invoke();
            }
            else if (!bPause)
            {
                fTime -= Time.deltaTime * fTimeScale;
                if (fTime < 0.0f)
                {
                    fTime = 0.0f;
                    bStop = true;
                }
                pFuncrun?.Invoke(fTime);
            }
        }
    }

    private System.Collections.Generic.List<LambdaTweener> tweeners = new System.Collections.Generic.List<LambdaTweener>();

    public void LambdaTweenAdd(float ftime, Action<float> pFuncrun, Action pFuncdone)
    {
        LambdaTweener tweener = new LambdaTweener();
        tweener.pFuncrun = pFuncrun;
        tweener.pFuncdone = pFuncdone;
        tweener.fTimeScale = 1.0f / ftime;

        if (tweeners.Count == 0)
            StartCoroutine(RunSC());
        tweeners.Add(tweener);
    }

    public void LambdaTweenStop()
    {
        foreach (LambdaTweener tweener in tweeners)
            tweener.bStop = true;
    }
    public void LambdaTweenComplete()
    {

        foreach (LambdaTweener tweener in tweeners)
        {
            tweener.fTime = 0.0f;
            tweener.pFuncrun?.Invoke(0.0f);
            tweener.bStop = true;
        }
        LambdaTweenUpdateNow();
    }
    private void LambdaTweenUpdateNow()
    {
        foreach (LambdaTweener tweener in tweeners.ToArray())
        {
            tweener.Run();
        }
        foreach (LambdaTweener tweener in tweeners.ToArray())
        {
            if (tweener.bFinish)
                tweeners.Remove(tweener);
        }
    }
    IEnumerator RunSC()
    {
        yield return null;
        while (tweeners.Count > 0)
        {
            LambdaTweenUpdateNow();
            yield return null;
        }
    }


    public void LambdaTweenMove(float fTime, GameObject go, Vector3 vPosInit, Vector3 vPosFinal)
    {
        go.transform.localPosition = Vector3.Lerp(vPosInit, vPosFinal, 1.0f - fTime);
    }

    public void LambdaTweenScale(float fTime, GameObject go, Vector3 vScalInit, Vector3 vScalFinal)
    {
        go.transform.localScale = Vector3.Lerp(vScalInit, vScalFinal, 1.0f - fTime);
    }

    public void LambdaTweenRotate(float fTime, GameObject go, Vector3 vRotInit, Vector3 vRotFinal)
    {
        go.transform.localRotation = Quaternion.Euler(Vector3.Lerp(vRotInit, vRotFinal, 1.0f - fTime));
    }
}