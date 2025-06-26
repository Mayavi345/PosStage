using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL.Service;
using Stage.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIComponent;
using UIComponent.BindingModel;
using Utilities;
using Utilities.Nlog;

namespace Stage.Presentation.MVVM.MemberManager.ViewModel
{
    public class MemberSingleViewModel : SingleViewFormViewModelBase
    {
        #region Field
        private List<ComboBoxItem<MB1002_ConsumptionLevelRule>> _comboBoxItems;
        private IMemberService _memberSerivce;
        public override List<Action> ValidateActions { get; set; }

        #endregion
        #region Constructor
        public MemberSingleViewModel(IMemberService memberService)
        {
            MemberModel = new MemberModel();
            _memberSerivce = memberService;
            LevelComboBoxList = new ObservableCollection<ComboBoxItem<MB1002_ConsumptionLevelRule>>(GetLevelComboBoxList());
            GenderList = EmployeeModel.GetGenderList();

            InitDefaultData();

        }

        private void InitDefaultData()
        {
            SelectedLevelComboBox = LevelComboBoxList.FirstOrDefault();
            GenderSelectedItem = GenderList.FirstOrDefault();
        }
        #endregion
        #region Properties
        private ObservableCollection<ComboBoxItem<MB1002_ConsumptionLevelRule>> _levelComboBoxList;
        public ObservableCollection<ComboBoxItem<MB1002_ConsumptionLevelRule>> LevelComboBoxList
        {
            get => _levelComboBoxList;
            set
            {
                _levelComboBoxList = value;
                RaisePropertyChanged(nameof(LevelComboBoxList));
            }
        }
        private MemberModel _memberModel;
        public MemberModel MemberModel
        {
            get => _memberModel;
            set
            {
                _memberModel = value;
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(PhoneNumber));
                RaisePropertyChanged(nameof(SelectedLevelComboBox));

                //_memberModel.ConsumptionLevel = GetConsumptionLevel();
                RaisePropertyChanged(nameof(MemberModel));
            }
        }
        private List<GenderModel> _genderList;
        public List<GenderModel> GenderList
        {
            get { return _genderList; }
            set
            {
                if (_genderList != value)
                {
                    _genderList = value;
                    RaisePropertyChanged(nameof(GenderList));
                }
            }
        }
        private GenderModel _genderSelectedItem;
        public GenderModel GenderSelectedItem
        {
            get { return _genderSelectedItem; }
            set
            {
                _genderSelectedItem = value;
                MemberModel.Gender = GenderSelectedItem;
                RaisePropertyChanged(nameof(GenderSelectedItem));
            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入名稱")]
        public string Name
        {
            get { return MemberModel.Name; }
            set
            {

                MemberModel.Name = value;
                _errorMessageHelper.ClearErrors(this, "Name");

                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(MemberModel));


            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入電話")]
        [RegularExpression(@"09\d{8}", ErrorMessage = "請輸入有效的手機號碼")]
        public string PhoneNumber
        {
            get { return MemberModel.PhoneNumber; }
            set
            {
                MemberModel.PhoneNumber = value;
                _errorMessageHelper.ClearErrors(this, "PhoneNumber");

                RaisePropertyChanged(nameof(PhoneNumber));
                RaisePropertyChanged(nameof(MemberModel));
            }
        }
        private ComboBoxItem<MB1002_ConsumptionLevelRule> _selectedLevelComboBox;
        public ComboBoxItem<MB1002_ConsumptionLevelRule> SelectedLevelComboBox
        {
            get => _selectedLevelComboBox;
            set
            {
                _selectedLevelComboBox = value;
                MemberModel.ConsumptionLevel = SelectedLevelComboBox.Object;
                RaisePropertyChanged(nameof(SelectedLevelComboBox));
                RaisePropertyChanged(nameof(MemberModel));

            }
        }
        #endregion
        #region Command

        #endregion
        #region Public Method
        public override void InitSingleViewData<T>(T data)
        {
            var tempMemberModel = data as MemberModel;
            //TODO 此處需要優化
            var tempSelectComboBox = LevelComboBoxList.Where(x => x.DisplayName == tempMemberModel.ConsumptionLevel.Name).FirstOrDefault();
            SelectedLevelComboBox = tempSelectComboBox;
            MemberModel = new MemberModel(tempMemberModel);
        }

        public override void InitValidate()
        {
            ValidateActions = new List<Action>();
            ValidateActions.Add(() =>
            {
                var value = Name;
                var propertyName = "Name";
                var validationContext = this;
                _errorMessageHelper.ValidateProperty(value, propertyName, validationContext);
            });
            ValidateActions.Add(() =>
            {
                _errorMessageHelper.ValidateProperty(PhoneNumber, "PhoneNumber", this);
            });

        }
        #endregion
        #region Private Method
        public override void ClearData()
        {
            MemberModel = new MemberModel();
            SelectedLevelComboBox = LevelComboBoxList[0];
            _errorMessageHelper.ClearAllError();
            InitDefaultData();

        }
        private List<ComboBoxItem<MB1002_ConsumptionLevelRule>> GetLevelComboBoxList()
        {
            var response = _memberSerivce.GetLevelList();
            if (response.IsSuccess)
            {
                if (response == null)
                {
                    LogManagerSingleton.Instance.PrintLog("No data", NLog.LogLevel.Error);
                    return null;
                }
                var tempList = response.Data;
                _comboBoxItems = new List<ComboBoxItem<MB1002_ConsumptionLevelRule>>();
                foreach (MB1002_ConsumptionLevelRule item in tempList)
                {
                    _comboBoxItems.Add(new ComboBoxItem<MB1002_ConsumptionLevelRule>()
                    {
                        DisplayName = item.Name,
                        ID = item.MemberLevel.ToString(),
                        Object = item
                    });
                }
                return _comboBoxItems;
            }
            else
            {
                throw new Exception("List<ComboBoxItem> GetLevelComboBoxList() : " + response.Message);
            }
        }

        #endregion

    }
}
