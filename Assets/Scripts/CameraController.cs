using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform _Xform_Camera;
    private Transform _Xform_Parent;

    private Vector3 _LocalRotation;
    private float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    void Start()
    {
        this._Xform_Camera = this.transform;
        this._Xform_Parent = this.transform.parent;
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CameraDisabled = !CameraDisabled;
        
        if (!CameraDisabled){
            if (Input.GetMouseButton(1)){
                if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
                    _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                    _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;
                    //To not flip camera over top
                    _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, -90f, 90f);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel")!= 0f){
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

                ScrollAmount *= (this._CameraDistance * 0.3f);
                this._CameraDistance += ScrollAmount * -1f;
                this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.75f, 10f);
            }
        }
        

         //actual camera rig transform
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._Xform_Parent.rotation = Quaternion.Lerp(this._Xform_Parent.rotation, QT, Time.deltaTime * OrbitDampening);
        if(this._Xform_Camera.localPosition.z != this._CameraDistance * -1f){
            this._Xform_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._Xform_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
        
    }
}