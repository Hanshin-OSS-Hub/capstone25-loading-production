public class AudioRecordingData
{
    public float[] Samples { get; private set; }
    public int Frequency { get; private set; }
    public int Channels { get; private set; }

    public AudioRecordingData(float[] samples, int frequency, int channels)
    {
        Samples = samples;
        Frequency = frequency;
        Channels = channels;
    }
}