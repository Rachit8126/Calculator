using Common;

namespace Calculator.Controller
{
    public class GameController : EntityController
    {
        #region Inspector Variables
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        private void Start()
        {
            OnGameStart();
        }

        private void OnDestroy()
        {
            OnGameOver();
        }
        #endregion Monobehaviour Methods

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        public override void OnGameStart()
        {
            view.SetController(this);
            view.OnGameStart();
        }

        public override void OnGameOver()
        {
            view.OnGameOver();
        }
        #endregion Public Methods
    }
}
