using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class waveFunction : MonoBehaviour
{
    [SerializeField]
    private int CornersCount = 2, WavesCount = 6; 
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    [SerializeField]
    private float amplitude = 1, wavelength = 1, speed = 1, xClamp = 0.002f;

    private void SetWaves() {
        Spline waterSpline = spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();

        for (int i = CornersCount; i < waterPointsCount - CornersCount; i++) {
            waterSpline.RemovePointAt(CornersCount);
        }

        Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
        Vector3 waterTopRightCorner = waterSpline.GetPosition(2);
        float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;

        float spacingPerWave = waterWidth / (WavesCount+1);

        for (int i = WavesCount; i > 0; i--) {
            int index = CornersCount;

            float xPosition = waterTopLeftCorner.x + (spacingPerWave * i);
            Vector3 wavePoint = new Vector3(xPosition, waterTopLeftCorner.y, waterTopLeftCorner.z);
            waterSpline.InsertPointAt(index, wavePoint);
            waterSpline.SetHeight(index, 0.1f);
            waterSpline.SetCorner(index, false);
        }
    }

    private void UpdateWaves() {
        Spline waterSpline = spriteShapeController.spline;
        int waterPointsCount = waterSpline.GetPointCount();

        Vector3 waterTopLeftCorner = waterSpline.GetPosition(1);
        Vector3 waterTopRightCorner = waterSpline.GetPosition(waterPointsCount - 2);
        float waterWidth = waterTopRightCorner.x - waterTopLeftCorner.x;

        float spacingPerWave = waterWidth / (WavesCount+1);

        for (int i = 2; i < waterPointsCount - 2; i++) {
            Vector3 currentPoint = waterSpline.GetPosition(i);
            Vector3 modifyPoint = new Vector3(currentPoint.x, currentPoint.y, currentPoint.z);
            float x = modifyPoint.x;

            float k = (2 * Mathf.PI * Mathf.Deg2Rad) / wavelength;
            float f = k * (x - speed * Time.fixedTime);
            modifyPoint.x = (waterTopLeftCorner.x + (spacingPerWave * (i - 1)) + ( xClamp * k * Mathf.Cos((spacingPerWave * (i - 1)) - speed * Time.fixedTime)));
            modifyPoint.y = amplitude * Mathf.Sin(f);            
            waterSpline.RemovePointAt(i);
            waterSpline.InsertPointAt(i, modifyPoint);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWaves();   
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaves();
    }
}
