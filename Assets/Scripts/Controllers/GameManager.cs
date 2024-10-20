using System;

public class GameManager : Singleton<GameManager> {
    private GameStates _state;

    public event EventHandler OnWinState;
    public event EventHandler OnLoseState;
    public event EventHandler OnGameStart;

    private void Start() {
        State = GameStates.Start;
    }

    public GameStates State {
        get {
            return _state;
        }
        set {
            _state = value;

            switch (_state) {
                case GameStates.Start:
                OnGameStart?.Invoke(this, EventArgs.Empty);
                break;
                case GameStates.Win:
                OnWinState?.Invoke(this, EventArgs.Empty);
                break;
                case GameStates.Lose:
                OnLoseState?.Invoke(this, EventArgs.Empty);
                break;
                default:
                break;
            }
        }
    }

}
