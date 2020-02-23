using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SUHScripts.Tests
{
    public class TEST_ObserveNoiseGenerators : MonoBehaviour
    {
        [SerializeField] PerlinNoiseData_ConstantPopper m_nextNoise = default;
        [SerializeField] KeyCode m_pushNextNoise = KeyCode.Space;
        [SerializeField] KeyCode m_terminateNoise = KeyCode.T;
        [SerializeField] Transform m_target = default;

        void Start()
        {
            var noiseStream = new Subject<INoiseGenerator>().AddTo(this);
            var terminateStream = this.WhenKey(m_terminateNoise).First();


            var d =
                this.UpdateAsObservable()
                .Select(_ => Time.deltaTime)
                .ObserveNoiseGenerators(noiseStream.TakeDuring(terminateStream), v => v.Average())
                .Subscribe(v => m_target.localPosition = v)
                .AddTo(this);


            this.WhenKey(m_pushNextNoise)
                .Subscribe(_ => noiseStream.OnNext(m_nextNoise.Pop()))
                .AddTo(this);

        }

    }

    [System.Serializable]
    public class PerlinNoiseData_ConstantPopper
    {
        [SerializeField] float frequency = 1;
        [SerializeField] float amplitude = 1;

        public INoiseGenerator Pop() =>
            new PerlinNoiseData_Constant(frequency, amplitude);
    }

}
