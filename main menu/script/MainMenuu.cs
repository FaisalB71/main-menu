using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // لإدارة عناصر واجهة المستخدم

public class MainMenu : MonoBehaviour
{
    [Tooltip("اسم المشهد الذي تريد الانتقال إليه")]
    public string VR7711;

    [Tooltip("نص أو شريط تحميل يظهر أثناء التحميل")]
    public GameObject loadingUI;

    [Tooltip("شريط تقدم التحميل (اختياري)")]
    public Slider progressBar;

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(VR7711))
        {
            StartCoroutine(LoadSceneAsync(VR7711));
        }
        else
        {
            Debug.LogError("اسم المشهد فارغ أو غير مخصص! تأكد من تعيين اسم المشهد في المفتش.");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // إظهار واجهة التحميل
        if (loadingUI != null)
        {
            loadingUI.SetActive(true);
        }

        // بدء تحميل المشهد في الخلفية
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // منع الانتقال المباشر حتى نتحكم فيه

        // تحديث شريط التقدم إذا كان موجودًا
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // تحويل القيمة إلى نسبة مئوية
            Debug.Log("Progress: " + (progress * 100) + "%");

            if (progressBar != null)
            {
                progressBar.value = progress; // تحديث شريط التقدم
            }

            // السماح بتفعيل المشهد عند اكتمال التحميل
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        // إخفاء واجهة التحميل عند الانتهاء
        if (loadingUI != null)
        {
            loadingUI.SetActive(false);
        }
    }
}
