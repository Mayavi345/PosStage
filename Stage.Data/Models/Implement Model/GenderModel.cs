using PosStage.MVVM.Models.Implement_Model;

namespace PosStage.MVVM.Models
{
    public class GenderModel : IComboBoxGenericItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayValue { get => Name; set { Name = value; } }

        public int Value { get => Id; set { Id = value; } }
    }
}
