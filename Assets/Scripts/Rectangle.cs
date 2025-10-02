using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Rectangle
{
    public Rectangle(
        float width,
        float height,
        float minX,
        float maxX,
        float minY,
        float maxY,
        bool isSplitable
    )
    {
        this.width = width;
        this.height = height;
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        this.isSplitable = isSplitable;

        centerX = (minX + maxX) * 0.5f;
        centerY = (minY + maxY) * 0.5f;

        square = width * height;
        ratio = width / height;
    }
    public float width;
    public float height;
    public float minX;
    public float maxX;
    public float centerX;
    public float minY;
    public float maxY;
    public float centerY;
    public float square;
    public float ratio;
    public bool isSplitable;

    // 인스펙터 창에서 확인 하기 위해 Public 으로 선언
    //public float width { get; private set; }
    //public float height { get; private set; }
    //public float minX { get; private set; }
    //public float maxX { get; private set; }
    //public float centerX { get; private set; }
    //public float minY { get; private set; }
    //public float maxY { get; private set; }
    //public float centerY { get; private set; }
    //public float square { get; private set; }
    //public float ratio { get; private set; }
    //public bool isSplitable { get; private set; }
    public List<Rectangle> Split(float space = 0f, float minLength = 0f, bool isPinwheel = false)
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        if (isSplitable)
        {
            if (isPinwheel)
            {
                if (Random.Range(0, 2) == 0)
                {
                    rectangles.AddRange(SplitPinwheel(space, minLength));
                }
                else
                {
                    rectangles.AddRange(SplitPinwheelReverse(space, minLength));
                }
            }
            else
            {
                if (width > height)
                {
                    rectangles.AddRange(SplitHorizontal(space, minLength, 0.6f));
                }
                else
                {
                    rectangles.AddRange(SplitVertical(space, minLength, 0.6f));
                }
            }
        }

        return rectangles;
    }
    private List<Rectangle> SplitHorizontal(float space = 0f, float minLength = 0f, float ratio = 0f)
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        float pie = width - minLength * 2 - space;

        float _minLength = height * ratio;
        float _pie = width - _minLength * 2 - space;
        if (_minLength > minLength && _pie > 0)
        {
            minLength = _minLength;
            pie = _pie;
        }

        float w1, w2;
        if (pie > 0)
        {
            w1 = minLength + pie * Random.Range(minLength / width, 1 - minLength / width);
            w2 = width - w1 - space;

            rectangles.Add(new Rectangle(
                width: w1,
                height: height,
                minX: minX,
                maxX: minX + w1,
                minY: minY,
                maxY: maxY,
                isSplitable: true
            ));

            rectangles.Add(new Rectangle(
                width: w2,
                height: height,
                minX: minX + w1 + space,
                maxX: maxX,
                minY: minY,
                maxY: maxY,
                isSplitable: true
            ));
        }
        else
        {
            rectangles.Add(new Rectangle(width, height, minX, maxX, minY, maxY, false));
        }

        return rectangles;
    }

    private List<Rectangle> SplitVertical(float space = 0f, float minLength = 0f, float ratio = 0f)
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        float pie = height - minLength * 2 - space;

        float _minLength = width * ratio;
        float _pie = height - _minLength * 2 - space;
        if (_minLength > minLength && _pie > 0)
        {
            minLength = _minLength;
            pie = _pie;
        }

        float h1, h2;
        if (pie > 0)
        {
            h1 = minLength + pie * Random.Range(minLength / height, 1f - minLength / height);
            h2 = height - h1 - space;

            rectangles.Add(new Rectangle(
                width: width,
                height: h1,
                minX: minX,
                maxX: maxX,
                minY: minY,
                maxY: minY + h1,
                isSplitable: true
            ));

            rectangles.Add(new Rectangle(
                width: width,
                height: h2,
                minX: minX,
                maxX: maxX,
                minY: minY + h1 + space,
                maxY: maxY,
                isSplitable: true
            ));
        }
        else
        {
            rectangles.Add(new Rectangle(width, height, minX, maxX, minY, maxY, false));
        }

        return rectangles;
    }

    private List<Rectangle> SplitPinwheel(float space = 0f, float minLength = 0f, float ratio = 0f)
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        float widthPie = width - minLength * 3 - space * 2;
        float heightPie = height - minLength * 3 - space * 2;

        // wdith ratio
        float wr5 = Random.Range(0.3333f, 0.6667f);
        float wr2 = Random.Range(0f, 1f - wr5);
        float wr4 = 1f - wr5 - wr2;

        // width
        float w5 = minLength + widthPie * wr5;
        float w2 = minLength + widthPie * wr2;
        float w4 = minLength + widthPie * wr4;
        float w1 = w5 + w4 + space;
        float w3 = w5 + w2 + space;

        // height ratio
        float hr5 = Random.Range(0f, 1f);
        float hr1 = Random.Range(0f, 1f - hr5);
        float hr3 = 1f - hr5 - hr1;

        // height
        float h5 = minLength + heightPie * hr5;
        float h1 = minLength + heightPie * hr1;
        float h3 = minLength + heightPie * hr3;
        float h2 = h5 + h1 + space;
        float h4 = h5 + h3 + space;

        Rectangle r1 = new Rectangle(w1, h1, minX, minX + w1, maxY - h1, maxY, true);
        Rectangle r2 = new Rectangle(w2, h2, maxX - w2, maxX, maxY - h2, maxY, true);
        Rectangle r3 = new Rectangle(w3, h3, maxX - w3, maxX, minY, minY + h3, true);
        Rectangle r4 = new Rectangle(w4, h4, minX, minX + w4, minY, minY + h4, true);
        Rectangle r5 = new Rectangle(w5, h5, maxX - w3, minX + w1, maxY - h2, minY + h4, true);

        rectangles.Add(r1);
        rectangles.Add(r2);
        rectangles.Add(r3);
        rectangles.Add(r4);
        rectangles.Add(r5);

        return rectangles;
    }

    private List<Rectangle> SplitPinwheelReverse(float space = 0f, float minLength = 0f)
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        float widthPie = width - minLength * 3 - space * 2;
        float heightPie = height - minLength * 3 - space * 2;

        // width ratio
        float wr5 = Random.Range(0.3333f, 0.6667f);
        float wr1 = Random.Range(0f, 1f - wr5);
        float wr3 = 1f - wr5 - wr1;

        // width
        float w5 = minLength + widthPie * wr5;
        float w1 = minLength + widthPie * wr1;
        float w3 = minLength + widthPie * wr3;
        float w2 = w5 + w3 + space;
        float w4 = w5 + w1 + space;

        // height ratio
        float hr5 = Random.Range(0f, 1f);
        float hr2 = Random.Range(0f, 1f - hr5);
        float hr4 = 1f - hr5 - hr2;

        // height
        float h5 = minLength + heightPie * hr5;
        float h2 = minLength + heightPie * hr2;
        float h4 = minLength + heightPie * hr4;
        float h1 = h5 + h2 + space;
        float h3 = h5 + h4 + space;

        Rectangle r1 = new Rectangle(w1, h1, minX, minX + w1, maxY - h1, maxY, true);
        Rectangle r2 = new Rectangle(w2, h2, maxX - w2, maxX, maxY - h2, maxY, true);
        Rectangle r3 = new Rectangle(w3, h3, maxX - w3, maxX, minY, minY + h3, true);
        Rectangle r4 = new Rectangle(w4, h4, minX, minX + w4, minY, minY + h4, true);
        Rectangle r5 = new Rectangle(w5, h5, maxX - w2, minX + w4, maxY - h1, minY + h3, true);

        rectangles.Add(r1);
        rectangles.Add(r2);
        rectangles.Add(r3);
        rectangles.Add(r4);
        rectangles.Add(r5);

        return rectangles;
    }
}
