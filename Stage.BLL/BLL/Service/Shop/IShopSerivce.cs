using PosStage.MVVM.Models.Implement_Model;

namespace Stage.BLL.BLL.Service
{
    public interface IShopSerivce
    {
        int ProductCount { get; }
        List<IOrderProduct> SelectedProductDic { get; }
        double TotalPrice { get; }

        void AddOrderProduct(IOrderProduct orderProductViewModel);
        void AddProduct(IOrderProduct productUserControlViewModel);
        void RefrshOrderList();
        void RemoveOrderProduct(IOrderProduct product);
    }
}
