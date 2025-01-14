using Cinemachine;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class CammeraZoom : MMSingleton<CammeraZoom>
{
    [SerializeField] private Camera cam;

    [SerializeField] private CinemachineVirtualCamera virturalcam;
    //-----------Zomcam//
    public Slider slider;
    public float othorgographicSizeNormal;
    public float othorgographicSizeZoomMin;
    public float othorgographicSizeZoomMax;
    
    //MoveCam--------------//
    private Vector3 StartTouch;
    public bool canMoveCam = true;
    //-----------------

    float scale => (Screen.height / (float)Screen.width) / (float)(1920 / (float)1080);
    private void Start()
    {
        othorgographicSizeZoomMax           = othorgographicSizeNormal * 1.2f;
        virturalcam.m_Lens.OrthographicSize = othorgographicSizeNormal * scale;

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        float x =  Mathf.Lerp(0f,1f, (othorgographicSizeZoomMax-othorgographicSizeNormal) / (othorgographicSizeZoomMax-othorgographicSizeZoomMin));
        slider.value = x;
    }

    private void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }


        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float different = currentMagnitude - prevMagnitude;
            Zoom(different * 0.0005f);
        }

        if (Input.GetMouseButtonDown(0) && canMoveCam)
        {
            StartTouch = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && canMoveCam)
        {
            Vector3 direction = StartTouch - cam.ScreenToWorldPoint(Input.mousePosition);
            //khi cam move theo thi directon tien toi vector0
           // camlookat.transform.position = ClampCamera(camlookat.transform.position + direction);

        }
        if (Input.GetMouseButtonUp(0))
        {
            StartTouch = Vector3.zero;
        }

    }
    private void OnSliderValueChanged(float value)
    {
        virturalcam.m_Lens.OrthographicSize = Mathf.Lerp(othorgographicSizeZoomMax * scale, othorgographicSizeZoomMin*scale, value);
    }
    private Vector3 ClampCamera(Vector3 targerPosition,Vector3 ceterGame)//truyen vao 1 vector3 va tra ve 1 vector3 hop ly voi Cammera
    {
        Vector3 vectorx;
        vectorx = new Vector3(targerPosition.x, targerPosition.y, targerPosition.z);
        float _camwidth = virturalcam.m_Lens.OrthographicSize * cam.aspect;
        //height

        //width
        float mixX = (ceterGame.x - othorgographicSizeNormal * scale * cam.aspect) + _camwidth;
        float maxX = (ceterGame.x + othorgographicSizeNormal * scale * cam.aspect) - _camwidth;

        float mixY = (ceterGame.y - othorgographicSizeNormal * scale * cam.aspect) + _camwidth - 10 * (1 - slider.value);
        float maxY = (ceterGame.y + othorgographicSizeNormal * scale * cam.aspect) - _camwidth - 10 * (1 - slider.value);



        float newX = Mathf.Clamp(vectorx.x, mixX, maxX);

        float newY = Mathf.Clamp(vectorx.y, mixY, maxY);

        return new Vector3(newX, newY, -10);
    }
    private void Zoom(float increment)
    {
        slider.value += increment;
        slider.value = Mathf.Clamp01(slider.value);
    }

}
