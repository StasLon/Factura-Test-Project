using UnityEngine;

public interface IVFX
{
    void PlayAt(Vector3 position, Quaternion rotation);
    void OnVFXFinished();
    VFXType GetVFXType();
}