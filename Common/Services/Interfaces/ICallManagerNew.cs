namespace Common.Services.Interfaces;

public interface ICallManagerNew
{
    bool IsActivated { get; set; }
    void Start();
    void Stop();
    void SetMicrophoneMute(bool muted);
    void SetBluetoothOn(bool on);
    void SetSpeakerphoneOn(bool on);
}