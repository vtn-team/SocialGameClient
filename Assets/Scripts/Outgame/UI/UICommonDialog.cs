using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Outgame.UICommonDialogInfo;

namespace Outgame
{
    public enum DialogType
    {
        OKOnly,
        YesNo
    }


    public class UICommonDialogInfo : UIInformationBase
    {
        public delegate void PushCallback(int button);
        public string Title;
        public string MainText;
        public DialogType Type;
        public PushCallback Callback;
    }

    public class UICommonDialog : UIStackableView
    {
        [SerializeField] TMPro.TextMeshProUGUI _title;
        [SerializeField] TMPro.TextMeshProUGUI _mainText;
        [SerializeField] UIView _okButton1;
        [SerializeField] UIView _okButton2;
        [SerializeField] UIView _ngButton;

        UICommonDialogInfo _dialogInfo;

        protected override void AwakeCall()
        {
            ViewId = ViewID.CommonDialog;
        }

        public override void SetupFromInfo()
        {
            _dialogInfo = _info as UICommonDialogInfo;
            _title.text = _dialogInfo.Title;
            _mainText.text = _dialogInfo.MainText;

            switch (_dialogInfo.Type)
            {
                case DialogType.OKOnly:
                    _okButton1.Active();
                    _okButton2.Disactive();
                    _ngButton.Disactive();
                    break;

                case DialogType.YesNo:
                    _okButton1.Disactive();
                    _okButton2.Active();
                    _ngButton.Active();
                    break;
            }
        }

        public void Click(int type)
        {
            _dialogInfo.Callback(type);
            UIManager.Back();
        }

        static public void OpenOKDialog(string title, string mainText, PushCallback callback)
        {
            UIManager.StackView(ViewID.CommonDialog, new UICommonDialogInfo()
            {
                Title = title,
                MainText = mainText,
                Type = DialogType.OKOnly,
                Callback = callback
            });
        }
        static public void OpenYesNoDialog(string title, string mainText, PushCallback callback)
        {
            UIManager.StackView(ViewID.CommonDialog, new UICommonDialogInfo()
            {
                Title = title,
                MainText = mainText,
                Type = DialogType.YesNo,
                Callback = callback
            });
        }
    }
}
