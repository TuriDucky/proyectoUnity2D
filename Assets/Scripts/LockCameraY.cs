using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y co-ordinate
/// </summary>
[SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraY : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    public float posY;
    public float posX;
    public bool isFollowingY;
    public bool isFollowingX;

    void Start()
    {
        isFollowingY = true;
        isFollowingX = true;
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (isFollowingY == false){
            if (stage == CinemachineCore.Stage.Finalize){
            var pos = state.RawPosition;
            pos.y = posY;
            state.RawPosition = pos;
            }
        } 
        if (isFollowingX == false){
            if (stage == CinemachineCore.Stage.Finalize){
            var pos = state.RawPosition;
            pos.x = posX;
            state.RawPosition = pos;
            }
        }
    }

    public void setY(){
        posY = transform.position.y;
        isFollowingY = false;
    }

    public void setX(){
        posX = transform.position.x;
        isFollowingX = false;
    }

    public void startFollowing(){
        isFollowingY = true;
        isFollowingX = true;
    }
}
