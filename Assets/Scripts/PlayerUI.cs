using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviour
    {

        #region Private Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        private PlayerManager target;
        float characterControllerHeight;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;

        #endregion
                

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _canvasGroup = this.GetComponent<CanvasGroup>();

            this.transform.SetParent(GameObject.Find("MainCanvas").GetComponent<Transform>(), false);
        }

        // Update is called once per frame
        void Update()
        {

            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }

            
        }

        private void LateUpdate()
        {
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y = characterControllerHeight;

                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        #endregion

        #region Public Methods

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;                
            }


            this.target = _target;
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();

            CharacterController characterController = _target.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }

            if (playerNameText != null)
            {
                playerNameText.text = this.target.photonView.Owner.NickName;
            }
        }

        #endregion


        
    }

}
