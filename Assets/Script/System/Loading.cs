using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static int nextSceneIdx;

    [SerializeField]
    Image progessBar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(int index)
    {
        nextSceneIdx = index;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation ao;
        ao = SceneManager.LoadSceneAsync(nextSceneIdx);
        ao.allowSceneActivation = false;
        float timer = 0.0f;

        while (!ao.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if(ao.progress < 0.9f)
            {
                progessBar.fillAmount = Mathf.Lerp(progessBar.fillAmount, ao.progress, timer);
                if (progessBar.fillAmount >= ao.progress) timer = 0f;
            }
            else
            {
                progessBar.fillAmount = Mathf.Lerp(progessBar.fillAmount, 1f, timer);
                if(progessBar.fillAmount == 1f)
                {
                    ao.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
