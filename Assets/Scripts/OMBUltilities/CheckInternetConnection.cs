//public class ConnectionCheckManager : SingletonMonoBehaviour<ConnectionCheckManager>
//{
//    [SerializeField]
//    string connectionCheckUrl = "https://www.google.com/";

//    ReactiveCommand<UnityWebRequest.Result> onConnectionError = default;
//    public IObservable<UnityWebRequest.Result> OnConnectionError => onConnectionError;

//    ReactiveProperty<bool> isConnectionCheckDone = default;
//    public IReadOnlyReactiveProperty<bool> IsConnectionCheckDone => isConnectionCheckDone;

//    protected override void Awake()
//    {
//        onConnectionError = new ReactiveCommand<UnityWebRequest.Result>();

//        isConnectionCheckDone = new ReactiveProperty<bool>(false);
//    }

//    async UniTask Start()
//    {
//        await CheckConnection();
//    }

//    public async UniTask CheckConnection()
//    {
//        using var request = UnityWebRequest.Get(connectionCheckUrl);
//        request.timeout = 30;

//        try
//        {
//            await request.SendWebRequest();
//        }
//        catch
//        {
//            onConnectionError.Execute(request.result);

//            return;
//        }

//        if (request.result != UnityWebRequest.Result.Success)
//        {
//            onConnectionError.Execute(request.result);

//            return;
//        }

//        isConnectionCheckDone
//            .SetValueAndForceNotify(true);
//    }
//}

