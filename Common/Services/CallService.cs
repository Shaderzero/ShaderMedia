using Common.Services.Interfaces;

namespace Common.Services;

public class CallService
{
    private readonly ICallManagerNew? _callManager;

    public CallService(ICallManagerNew? callManager)
    {
        _callManager = callManager;
    }

    public void Start()
    {
        _callManager?.Start();
    }

    public void Stop()
    {
        _callManager?.Stop();
    }

    public void SetMicrophoneMute(bool enable)
    {
        _callManager?.SetMicrophoneMute(enable);
    }
    
    public void SetSpeakerphone(bool enable)
    {
        _callManager?.SetSpeakerphoneOn(enable);
    }

    public void SetBluetooth(bool enable)
    {
        _callManager?.SetBluetoothOn(enable);
    }
}