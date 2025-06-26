namespace PosStage.MVVM.Models.Implement_Model
{
    public interface IComboBoxGenericItem<T>
    {
        public string DisplayValue { get; set; }
        public T Value { get; set; }

    }
}
