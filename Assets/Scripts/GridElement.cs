using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using System;

[RequireComponent(typeof(RectTransform))]
public class GridElement : MonoBehaviour
{
    [SerializeField] private CanvasGroup glowCG;
    [SerializeField] private Image maskImage;
    [SerializeField] private Image coreImage;
    public Rectangle rectangle;

    public void SetRectangle(Rectangle rectangle)
    {
        this.rectangle = rectangle;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(rectangle.width, rectangle.height);
        rectTransform.anchoredPosition = new Vector2(rectangle.centerX, rectangle.centerY);
    }

    public void SetTexture(Sprite sprite)
    {
        if (sprite != null)
        {
            coreImage.sprite = sprite;
            coreImage.SetNativeSize();

            RectTransform coreRT = coreImage.GetComponent<RectTransform>();
            coreRT.sizeDelta = GetBestFitCropSize(coreRT, maskImage.GetComponent<RectTransform>());
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //public Building building;
    //public void SetBuilding(Building building)
    //{
    //    this.building = building;

    //    SetTexture(building.sprite);
    //}
    //public void HighlightAndDestory()
    //{
    //    Sequence seq = DOTween.Sequence();

    //    seq.Append(glowCG.DOFade(1f, .5f));
    //    seq.Append(glowCG.DOFade(0f, .5f));
    //    seq.AppendCallback(() => { Destroy(gameObject); });
    //}
    //public void Show()
    //{
    //    CanvasGroup cg = GetComponent<CanvasGroup>();
    //    cg.alpha = 0f;
    //    cg.DOFade(1f, .5f);
    //}

    //public void Hide(Action OnComplete = null)
    //{
    //    CanvasGroup cg = GetComponent<CanvasGroup>();
    //    cg.DOFade(0f, .5f).OnComplete(() => OnComplete?.Invoke());
    //}
    // target을 parent에 꽉 채우도록 하는 SizeDelta값을 반환한다.
    // parent의 여백이 없이 채우는 것이 목적이기 때문에 target이 일부 잘릴 수 있다.
    // 해당 메소드를 실행 하기 전에 target의 Image.SetNativeSize()를 실행한다.
    private Vector2 GetBestFitCropSize(RectTransform target, RectTransform parent)
    {
        float targetWidth = target.rect.width;
        float targetHeight = target.rect.height;

        float parentWidth = parent.rect.width;
        float parentHeight = parent.rect.height;

        float ratio = parentWidth / targetWidth;

        if (targetHeight * ratio < parentHeight)
        {
            ratio = parentHeight / targetHeight;
        }

        return new Vector2(targetWidth * ratio, targetHeight * ratio);
    }
}
