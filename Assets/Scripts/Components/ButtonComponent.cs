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
        [SerializeField] private string value;
        #endregion Inspector Variables

        #region Public Variables
        public event Action<ButtonType, string> OnButtonPressed;
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        private void Start()
        {
            buttonText.text = value;

            button.onClick.AddListener(() =>
            {
                OnButtonPressed?.Invoke(type, value);
            });
        }
        #endregion Monobehaviour Methods

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        internal ButtonType GetButtonType()
        {
            return type;
        }

        internal string GetButtonValue()
        {
            return value;
        }
        #endregion Public Methods
    }
}
