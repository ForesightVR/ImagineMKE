using UnityEngine;
using System.Collections;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    public Vector3 realPosition = Vector3.zero;
    public Vector3 positionAtLastPacket = Vector3.zero;

    public Quaternion realRotation = Quaternion.identity;
    public Quaternion rotationAtLastPacket = Quaternion.identity;

    public double currentTime = 0.0;
    public double currentPacketTime = 0.0;
    public double lastPacketTime = 0.0;
    public double timeToReachGoal = 0.0;

    void Update()
    {
        if (!photonView.IsMine)
        {
            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, realRotation, (float)(currentTime / timeToReachGoal));
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector3)transform.position);
            stream.SendNext((Quaternion)transform.rotation);
        }
        else
        {
            currentTime = 0.0;

            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;

            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();

            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
        }
    }
}