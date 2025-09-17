using Calculator.Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Calculator.Component
{
    public class ButtonComponent : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private ButtonType type;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private char value;
        #endregion Inspector Variables

        #region Public Variables
        public event Action<ButtonType, char> OnButtonPressed;
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                OnButtonPressed?.Invoke(type, value);
            });
        }
        #endregion Monobehaviour Methods

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        #endregion Public Methods
    }
}
