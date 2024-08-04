namespace Common.Services.Interfaces;

public interface ICallManager
{
    bool AudioManagerActivated { get; set; }

    bool IsSpeakerphoneOn();
    void TrySelectBluetoothDevice(bool enable);
    void UpdateIconsState();
    void ManualTurnScreenOff();
    void ManualTurnScreenOn();
    void SetKeepScreenOn(bool keepScreenOn);
    void SetMicrophoneMute(bool enable);
    void SetSpeakerphoneOn(bool enable);
    void Start(CallService callService);
    bool StartBusytone();
    void StartProximitySensor();
    void StartRingback();
    void Stop();
    void StopBusytone();
    void StopProximitySensor();
    void StopRingback();
    void StopRingtone();
    void TurnScreenOff();
    void TurnScreenOn();
}