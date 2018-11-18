using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// Copyright (c) 2016 @cyario
/// https://github.com/cyario/SnapScroll

/// Other Contributor
/// 「uGUI スクロールビューを分割毎にスナップする」 (c) 2015 imatomi
/// https://github.com/imatomix/ScrollRectSnap
/// 「Unity uGUI スクロールビューをスナップさせる」 (c) 2015 TakafumiFukuyo
/// https://qiita.com/TakafumiFukuyo/items/9cb37044f388a81cd841

public class SnapScroll : ScrollRect
{

    public int hIndex;
    public int vIndex;

    public int hPageNum = 3;
    public int vPageNum = 0;
    public float smoothness = 10f;
    public float scrollWeight = 0.1f;

    private Vector2 targetPosition;
    private float hPerPage;
    private float vPerPage;
    private bool dragging;
    private bool forcePositionUpdate = false;

    public void ScrollTo(int hIndex, int vIndex)
    {
        this.hIndex = hIndex;
        this.vIndex = vIndex;
        forcePositionUpdate = true;
    }

    protected override void Awake()
    {
        base.Awake();
        hPerPage = 1f / (float)(hPageNum - 1);
        vPerPage = 1f / (float)(hPageNum - 1);
    }

    public void ReAwake()
    {
        Awake();
    }

    protected override void Start()
    {
        base.Start();
        targetPosition = GetSnapPosition();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        dragging = true;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        UpdateIndex();
        targetPosition = GetSnapPosition();
        dragging = false;
    }

    void Update()
    {
        if (!dragging && normalizedPosition != targetPosition)
        {
            normalizedPosition = Vector2.Lerp(normalizedPosition, targetPosition, smoothness * Time.deltaTime);

            if (Vector2.Distance(normalizedPosition, targetPosition) < 0.009f)
            {
                normalizedPosition = targetPosition;
            }
        }

        if (forcePositionUpdate)
        {
            forcePositionUpdate = false;
            targetPosition = GetSnapPosition();
        }
    }

    void UpdateIndex()
    {
        float xPage, yPage = -1;

        if (horizontal && hPageNum > 0)
        {
            xPage = (normalizedPosition.x / hPerPage);
            float diff = xPage - (1 * hIndex);

            if (diff >= scrollWeight)
            {
                hIndex++;
            }
            else if (diff <= -scrollWeight)
            {
                hIndex--;
            }
        }

        if (vertical && vPageNum > 0)
        {
            yPage = normalizedPosition.y / vPerPage;
            float diff = yPage - (1 * vIndex);

            if (diff >= scrollWeight)
            {
                vIndex++;
            }
            else if (diff <= -scrollWeight)
            {
                vIndex--;
            }
        }
    }

    Vector2 GetSnapPosition()
    {
        return new Vector2(horizontal && hPageNum > 0 ? hIndex * hPerPage : normalizedPosition.x, vertical && vPageNum > 0 ? vIndex * vPerPage : normalizedPosition.y);
    }
}