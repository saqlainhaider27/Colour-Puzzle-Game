using System;

public class GameManager : Singleton<GameManager> {
    private GameStates _state;

    public event EventHandler OnWinState;
    public event EventHandler OnLoseState;

    public GameStates State {
        get {
            return _state;
        }
        set {
            _state = value;

            switch (_state) {
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
