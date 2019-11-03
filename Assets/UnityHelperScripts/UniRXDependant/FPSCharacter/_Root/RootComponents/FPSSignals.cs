using System;
using UniRx;

public class FPSSignals
{
    public IObservable<CharacterMovementOutput> SourceProvider { get; private set; }
    Subject<IObservable<CharacterMovementOutput>> SourceProviderSubject;

    FPSRoot m_root;

    public void Initialize(FPSRoot root)
    {
        m_root = root;
        SourceProviderSubject = new Subject<IObservable<CharacterMovementOutput>>().AddTo(root.AttachBehaviour);
        SourceProvider = SourceProviderSubject.Switch();
    }

    public void RequestNewCharacterCoreMovementSource(I_CharacterCoreMovement source)
    {
        SourceProviderSubject.OnNext(source.OnCharacterMovementOutput);
    }

    public void RequestResetCharacterMovementSource()
    {
        SourceProviderSubject.OnNext(Observable.Never<CharacterMovementOutput>());
    }
}