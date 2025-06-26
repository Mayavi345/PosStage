using Stage.Data.Models;
using Utilities;
using Utilities.Observer;
using PosStage;
using PosStage.MVVM.Models;
using Stage.BLL.BLL.Service;
using PosStage.MVVM.Models.Implement_Model;
using NLog;
using Utilities.Nlog;
using Stage.Data.Models.Implement_Model;
using System.Collections.ObjectModel;

namespace Stage.BLL.BLL
{
    public class MainDataCenter : Singleton<MainDataCenter>
    {
        public MainDataCenter()
        {
            ShoppingCart = new ShoppingCartService();
            ProductSettingManager = new ProductSettingManager();
            InitMessageSubject();
        }

        public IShoppingCartService ShoppingCart { get; }
        public IShopSerivce ShopSerivce => _shopSerivce;
        private IShopSerivce _shopSerivce;

        public IProductSettingManager ProductSettingManager { get; }

        #region MessageSubject
        private void InitMessageSubject()
        {
            _registerMemberSubject = new MessageSubject();
            _refreshCartSubject = new MessageSubject();
            _refreshProductView = new MessageSubject();
            _refreshNavBar = new MessageSubject();
        }
        public MessageSubject MemberInfoSubject => _registerMemberSubject;
        private MessageSubject _registerMemberSubject;

        public MessageSubject RefreshCartSubject => _refreshCartSubject;
        private MessageSubject _refreshCartSubject;

        public MessageSubject RefreshProductView => _refreshProductView;
        private MessageSubject _refreshProductView;

        public MessageSubject RefreshNavBar => _refreshNavBar;
        private MessageSubject _refreshNavBar;
        #endregion


        public void SetIShopService(IShopSerivce shopService)
        {
            _shopSerivce = shopService;
        }
        public void LoadData() {
            ProductSettingManager.LoadData();
        }
    }

}
