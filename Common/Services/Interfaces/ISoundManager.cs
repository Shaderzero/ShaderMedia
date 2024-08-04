namespace Common.Services.Interfaces;

public interface ISoundManager
{
    void Start(CallService callService);
    void UpdateIconsState();
    void SetMicrophoneMute(bool muted);
    void SetSpeakerphone(bool enable);
}