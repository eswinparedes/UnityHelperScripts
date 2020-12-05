namespace SUHScripts
{
    public interface IPayloadObserver<TPayload>
    {
        void ObservePayload(TPayload payLoad);
    }
}