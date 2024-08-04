using System.Security.AccessControl;

namespace Common.Services;

public class AudioButtonsStateService
{
    public event Action? OnUpdate;
    public bool MicrophoneMute { get; private set; }
    public bool SpeakerphoneOn { get; private set; }
    public bool BluetoothOn { get; private set; }

    public void SetMicrophoneMute(bool mute)
    {
        if (MicrophoneMute == mute)
            return;

        MicrophoneMute = mute;
        OnUpdate?.Invoke();
    }
    
    public void SetSpeakerphoneOn(bool on)
    {
        if (SpeakerphoneOn == on)
            return;

        SpeakerphoneOn = on;
        OnUpdate?.Invoke();
    }
    
    public void SetBluetoothOn(bool on)
    {
        if (BluetoothOn == on)
            return;

        BluetoothOn = on;
        OnUpdate?.Invoke();
    }
}