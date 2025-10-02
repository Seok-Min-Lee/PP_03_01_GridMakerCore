using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    [SerializeField] private RectTransform gridTransform;
    [SerializeField] private GameObject elementPrefab;

    [SerializeField] private int padding; // 여백
    [SerializeField] private int space; // 그리드 간 여백
    [SerializeField] private int minLength; // 최소 길이
    [SerializeField] private int splitCount;    // 분할 개수

    private List<Rectangle> rectangles = new List<Rectangle>();
    private List<GridElement> gridElements = new List<GridElement>();
    private void Start()
    {
        CreateGrid();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CreateGrid();
        }
    }
    private void CreateGrid()
    {
        rectangles = CreateRectangles();
        gridElements.AddRange(CreateGridElements(rectangles.Count - gridElements.Count));

        SetElementRectTransforms(gridElements, rectangles);

    }
    private List<GridElement> CreateGridElements(int count)
    {
        List<GridElement> elements = new List<GridElement>();

        for (int i = 0; i < count; i++)
        {
            GridElement element = GameObject.Instantiate(elementPrefab, gridTransform).GetComponent<GridElement>();

            elements.Add(element);
        }

        return elements;
    }
    private List<Rectangle> CreateRectangles()
    {
        List<Rectangle> rectangles = new List<Rectangle>();

        float minX = padding;
        float minY = padding;

        float width = gridTransform.sizeDelta.x - padding * 2;
        float height = gridTransform.sizeDelta.y - padding * 2;

        rectangles.Add(new Rectangle(
            width: width,
            height: height,
            minX: minX,
            minY: minY,
            maxX: minX + width,
            maxY: minY + height,
            isSplitable: true
        ));

        // 더 이상 분리할 수 없는 것들
        List<Rectangle> unSplitables = new List<Rectangle>();
        // 우선적으로 분리할 것들
        List<Rectangle> forces = new List<Rectangle>();
        while (true)
        {
            int gridCount = unSplitables.Count + rectangles.Count + forces.Count;

            if (gridCount >= splitCount)
            {
                break;
            }

            List<Rectangle> tempList = forces.Count > 0 ? forces : rectangles;
            Rectangle rectangle = tempList[0];

            // 바람개비 모양으로 분리할 것인지 체크
            bool isSplitPinwheel = splitCount - gridCount > 4 &&
                                    rectangle.ratio > 0.625f &&
                                    rectangle.ratio < 1.6f &&
                                    rectangle.width > minLength * 4 &&
                                    rectangle.height > minLength * 4;

            List<Rectangle> splits = rectangle.Split(space, minLength, isSplitPinwheel);


            if (splits.Count > 0)
            {
                foreach (Rectangle split in splits)
                {
                    if (split.ratio < 0.625f || split.ratio > 1.6f)
                    {
                        forces.Add(split);
                    }
                    else
                    {
                        rectangles.Add(split);
                    }
                }

                tempList.Remove(rectangle);
            }
            else
            {
                unSplitables.Add(rectangle);
                tempList.Remove(rectangle);

                if (rectangles.Count == 0)
                {
                    break;
                }
            }
        }

        rectangles.AddRange(unSplitables);
        rectangles.AddRange(forces);

        Debug.Log(rectangles.Count);

        return rectangles;
    }


    private void SetElementRectTransforms(IEnumerable<GridElement> elements, IEnumerable<Rectangle> rectangles)
    {
        int elementCount = elements.Count();
        int rectangleCount = rectangles.Count();

        for (int i = 0; i < elementCount; i++)
        {
            GridElement element = elements.ElementAt(i);

            if (i < rectangleCount)
            {
                element.SetRectangle(rectangles.ElementAt(i));
                element.gameObject.SetActive(true);
            }
            else
            {
                element.gameObject.SetActive(false);
            }
        }
    }
}
